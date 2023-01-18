﻿using DomainModels.Entities;
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

        if (venda.Status != Status.AguardandoPagamento)
            throw new BadRequestException($"Não é possível realizar uma compra com status: {venda.Status}.");

        unitOfWork.Add(venda);
        UnitOfWork.SaveChanges();

        return venda.Id;
    }

    public Venda BuscarVendaPorId(long id)
    {
        var repository = RepositoryFactory.Repository<Venda>();
        var vendaFound = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id))
            .Include(x => x.Include(x => x.Vendedor))
            .Include(x => x.Include(x => x.Itens));

        return repository.FirstOrDefault(vendaFound);
    }

    public void AtualizarVenda(long id, Venda venda)
    {
        var unitOfWork = UnitOfWork.Repository<Venda>();

        VerificacaoDeStatus(venda.Status);

        var vendaEncontrada = BuscarVendaPorId(id) ??
            throw new NotFoundException($"Venda para o Id: {id} não encontrada.");

        TransicaoDeStatus(vendaEncontrada.Status, venda.Status);

        vendaEncontrada.Status = venda.Status;

        unitOfWork.Update(vendaEncontrada);
        UnitOfWork.SaveChanges();
    }

    private void VerificacaoDeStatus(Status status)
    {
        if (status is Status.Entregue || status is Status.Cancelada)
            throw new BadRequestException($"O status da venda não pode ser atualizado, pois esta venda está com status: {status}.");
    }

    private void TransicaoDeStatus(Status status, Status atualizacaoStatus)
    {
        switch (status)
        {
            case Status.AguardandoPagamento:
                if (atualizacaoStatus != Status.AguardandoPagamento || atualizacaoStatus != Status.Cancelada)
                    throw new BadRequestException($"O status: {status} não pode ser atualizado para o status: {atualizacaoStatus}.");
                break;
            case Status.PagamentoAprovado:
                if (atualizacaoStatus != Status.EnviadoParaTransportadora || atualizacaoStatus != Status.Cancelada)
                    throw new BadRequestException($"O status: {status} não pode ser atualizado para o status: {atualizacaoStatus}.");
                break;
            case Status.EnviadoParaTransportadora:
                if (atualizacaoStatus != Status.Entregue)
                    throw new BadRequestException($"O status: {status} não pode ser atualizado para o status: {atualizacaoStatus}.");
                break;
            default:
                throw new BadRequestException($"Status: {status} não existe.");
        }
    }
}