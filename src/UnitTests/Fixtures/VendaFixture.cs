using Application.Models;
using Bogus;
using Bogus.Extensions.Brazil;
using DomainModels.Entities;
using DomainModels.Enums;

namespace UnitTests.Fixtures;

public static class VendaFixture
{
	public static Venda VendaFake()
	{
		return new Faker<Venda>()
			.CustomInstantiator(x => new Venda(
				dataVenda: x.Date.Recent(),
				vendedor: new Faker<Vendedor>()
					.CustomInstantiator(x => new Vendedor(
                        cpf: x.Person.Cpf(),
						nome: x.Person.FullName,
						telefone: x.Person.Phone)),
				itens: new Faker<Item>()
					.CustomInstantiator(x => new Item(
						name: x.Commerce.ProductName())).Generate(1),
				status: x.PickRandom<Status>()))
            .Generate();
	}

	public static CreateVendaRequest VendaRequestFake()
	{
        var vendaFake = new Faker<CreateVendaRequest>()
            .CustomInstantiator(x => new CreateVendaRequest(
                dataVenda: x.Date.Recent(),
                vendedor: new Faker<Vendedor>()
                    .CustomInstantiator(x => new Vendedor(
                        cpf: x.Person.Cpf(false),
                        nome: x.Person.FullName,
                        telefone: x.Person.Phone)),
                itens: new Faker<Item>()
                    .CustomInstantiator(x => new Item(
                        name: x.Commerce.ProductName())).Generate(1),
                status: Status.AguardandoPagamento))
            .Generate();

        vendaFake.Vendedor.Telefone = "(11) 98354-2892";

        return vendaFake;
    }

    public static UpdateVendaRequest UpdateVendaRequest()
    {
        return new Faker<UpdateVendaRequest>()
            .RuleFor(x => x.Status, x => x.PickRandom<Status>())
            .Generate();
    }
}