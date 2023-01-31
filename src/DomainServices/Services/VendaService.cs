using DomainModels.Entities;
using DomainModels.Enums;
using DomainServices.Exceptions;
using DomainServices.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infraestructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.Services;

public class VendaService : BaseService, IVendaService
{
    public VendaService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory) { }

    public long CadastrarVenda(Venda venda)
    {
        var unitOfWork = UnitOfWork.Repository<Venda>();

        VerificaItensEmVenda(venda);

        if (venda.Status != Status.AguardandoPagamento)
            throw new BadRequestException($"Não é possível realizar uma compra com status: {venda.Status}.");

        unitOfWork.Add(venda);
        UnitOfWork.SaveChanges();

        return venda.Id;
    }

    private void VerificaItensEmVenda(Venda venda)
    {
        foreach (var item in venda.Itens)
        {
            if (item.Name is null)
                throw new BadRequestException($"Não é possível registrar a venda. Um nome de item não foi informado.");
        }
    }

    public Venda BuscarVendaPorId(long id)
    {
        var repository = RepositoryFactory.Repository<Venda>();
        var vendaFound = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id))
            .Include(x => x.Include(x => x.Vendedor), x => x.Include(x => x.Itens));

        return repository.FirstOrDefault(vendaFound);
    }

    public void AtualizarVenda(long id, Venda venda)
    {
        var unitOfWork = UnitOfWork.Repository<Venda>();

        var vendaEncontrada = BuscarVendaPorId(id) ??
            throw new NotFoundException($"Venda para o Id: {id} não encontrada.");

        TransicaoDeStatus(vendaEncontrada.Status, venda.Status);

        vendaEncontrada.Status = venda.Status;

        unitOfWork.Update(vendaEncontrada);
        UnitOfWork.SaveChanges();
    }

    private void TransicaoDeStatus(Status statusDaVenda, Status atualizacaoStatus)
    {
        switch (statusDaVenda)
        {
            case Status.AguardandoPagamento:
                if (!(atualizacaoStatus == Status.PagamentoAprovado || atualizacaoStatus == Status.Cancelada))
                    throw new BadRequestException($"O status: {statusDaVenda} não pode ser atualizado para o status: {atualizacaoStatus}.");
                break;
            case Status.PagamentoAprovado:
                if (!(atualizacaoStatus == Status.EnviadoParaTransportadora || atualizacaoStatus == Status.Cancelada))
                    throw new BadRequestException($"O status: {statusDaVenda} não pode ser atualizado para o status: {atualizacaoStatus}.");
                break;
            case Status.EnviadoParaTransportadora:
                if (atualizacaoStatus != Status.Entregue)
                    throw new BadRequestException($"O status: {statusDaVenda} não pode ser atualizado para o status: {atualizacaoStatus}.");
                break;
            case Status.Entregue:
                throw new BadRequestException($"O status da venda não pode ser atualizado, pois esta venda está com status: {statusDaVenda}.");
            default:
                throw new BadRequestException($"Status: {statusDaVenda} não existe.");
        }
    }
}