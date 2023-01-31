using Bogus;
using Bogus.Extensions.Brazil;
using DomainModels.Entities;

namespace UnitTests.Fixtures;

public static class VendedorFixture
{
	public static Vendedor VendedorFake()
	{
		var vendedorFake = new Faker<Vendedor>()
            .CustomInstantiator(x => new Vendedor(
				cpf: x.Person.Cpf(false),
				nome: x.Person.FullName, 
				telefone: x.Person.Phone))
			.Generate();

        vendedorFake.Telefone = "(11) 98354-2892";
		vendedorFake.Id = 1L;

		return vendedorFake;
    }
}