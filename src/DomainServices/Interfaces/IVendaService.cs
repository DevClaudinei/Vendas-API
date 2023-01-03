using DomainModels.Entities;

namespace DomainServices.Interfaces;

public interface IVendaService
{
    long Create(Venda venda);
    void Update(long id, Venda venda);
    Venda GetById(long id);
}