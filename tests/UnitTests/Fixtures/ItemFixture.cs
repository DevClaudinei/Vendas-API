using Bogus;
using DomainModels.Entities;

namespace UnitTests.Fixtures;

public static class ItemFixture
{
	public static Item ItemFake()
	{
		var itemFake = new Faker<Item>()
			.CustomInstantiator(x => new Item(
				name: x.Commerce.ProductName()))
			.Generate();

		itemFake.Id = 1L;

		return itemFake;
	}
}