using DomainModels.Entities;

namespace DomainServices.Interfaces;

public interface IVendaService
{
    long CadastrarVenda(Venda venda);
    void AtualizarVenda(long id, Venda venda);
    Venda BuscarVendaPorId(long id);
}