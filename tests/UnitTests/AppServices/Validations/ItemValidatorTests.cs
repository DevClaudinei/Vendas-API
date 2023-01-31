using AppServices.Validations;
using FluentAssertions;
using FluentValidation.TestHelper;
using System.Linq;
using UnitTests.Fixtures;

namespace UnitTests.AppServices.Validations;

public class ItemValidatorTests
{
    private readonly ItemValidator _itemValidator = new ();

    [Fact]
    public void Deveria_Passar_Quando_Executar_ItemValidator()
    {
        // Arrange
        var itemFake = ItemFixture.ItemFake();

        // Act
        var result = _itemValidator.TestValidate(itemFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_ItemValidator_Porque_Nome_Do_Produto_Eh_Nulo()
    {
        // Arrange
        var itemFake = ItemFixture.ItemFake();
        itemFake.Name = null;

        // Act
        var result = _itemValidator.TestValidate(itemFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.Errors.Count.Should().Be(2);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_ItemValidator_Porque_Nome_Do_Produto_Eh_Maior_Que_Tamanho_Maximo()
    {
        // Arrange
        var itemFake = ItemFixture.ItemFake();
        itemFake.Name = "Antonio Carlos Bittencourt Albuquerque Alcantara Jr";
        var x = itemFake.Name.Count();

        // Act
        var result = _itemValidator.TestValidate(itemFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.Errors.Count.Should().Be(1);
    }
}