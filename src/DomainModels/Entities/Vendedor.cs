using DomainModels.Interfaces;

namespace DomainModels.Entities;

public class Vendedor : IIdentifiable
{
    protected Vendedor() { }

    public Vendedor(string cpf, string nome, string telefone)
    {
        Cpf = cpf;
        Nome = nome;
        Telefone = telefone;
    }

    public long Id { get; set; }
    public string Cpf { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
}