using Application.Models;

namespace AppServices.Services.Interfaces;

public interface IVendaAppService
{
    long CadastrarVenda(CreateVendaRequest vendaRequest);
    void AtualizarVenda(long id, UpdateVendaRequest updateVendaRequest);
    VendaResult BuscarVendaPorId(long id);
}