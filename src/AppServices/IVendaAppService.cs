using Application.Models;

namespace AppServices;

public interface IVendaAppService
{
    long Create(CreateVendaRequest vendaRequest);
    void Update(long id, UpdateVendaRequest updateVendaRequest);
    VendaResult GetById(long id);
}