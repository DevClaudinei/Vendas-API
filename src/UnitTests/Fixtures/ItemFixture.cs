using Bogus;
using DomainModels.Entities;

namespace UnitTests.Fixtures;

public static class ItemFixture
{
	public static Item ItemFake()
	{
		return new Faker<Item>()
			.CustomInstantiator(x => new Item(
				name: x.Commerce.ProductName()))
			.Generate();
	}
}