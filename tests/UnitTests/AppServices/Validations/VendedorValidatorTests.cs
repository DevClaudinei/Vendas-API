using AppServices.Validations;
using FluentAssertions;
using FluentValidation.TestHelper;
using UnitTests.Fixtures;

namespace UnitTests.AppServices.Validations;

public class VendedorValidatorTests
{
    private readonly VendedorValidator _vendedorValidator =  new ();

    [Fact]
    public void Deveria_Passar_Quando_Executar_VendedorValidator()
    {
        // Arrange
        var vendedorFake = VendedorFixture.VendedorFake();

        // Act
        var result = _vendedorValidator.TestValidate(vendedorFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_VendedorValidator_Porque_Cpf_Eh_Nulo()
    {
        // Arrange
        var vendedorFake = VendedorFixture.VendedorFake();
        vendedorFake.Cpf = "";

        // Act
        var result = _vendedorValidator.TestValidate(vendedorFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
        result.Errors.Count.Should().Be(2);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_VendedorValidator_Porque_Cpf_Eh_Invalido()
    {
        // Arrange
        var vendedorFake = VendedorFixture.VendedorFake();
        vendedorFake.Cpf = "008.216.795-89";

        // Act
        var result = _vendedorValidator.TestValidate(vendedorFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_VendedorValidator_Porque_Nao_Tem_Nome_Do_Vendedor()
    {
        // Arrange
        var vendedorFake = VendedorFixture.VendedorFake();
        vendedorFake.Nome = "";

        // Act
        var result = _vendedorValidator.TestValidate(vendedorFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome);
        result.Errors.Count.Should().Be(4);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_VendedorValidator_Porque_Vendedor_Nao_Tem_Sobrenome()
    {
        // Arrange
        var vendedorFake = VendedorFixture.VendedorFake();
        vendedorFake.Nome = "José";

        // Act
        var result = _vendedorValidator.TestValidate(vendedorFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome);
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_VendedorValidator_Porque_Nome_De_Vendedor_Tem_Caracteres_Invalidos()
    {
        // Arrange
        var vendedorFake = VendedorFixture.VendedorFake();
        vendedorFake.Nome = "Jo@o Conceição";

        // Act
        var result = _vendedorValidator.TestValidate(vendedorFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome);
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_VendedorValidator_Porque_Nome_De_Vendedor_Tem_Espacos_Desnecessarios()
    {
        // Arrange
        var vendedorFake = VendedorFixture.VendedorFake();
        vendedorFake.Nome = "Solange  Braga";

        // Act
        var result = _vendedorValidator.TestValidate(vendedorFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome);
        result.Errors.Count.Should().Be(2);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_VendedorValidator_Porque_Nome_De_Vendedor_Tem_Sobrenome_Invalido()
    {
        // Arrange
        var vendedorFake = VendedorFixture.VendedorFake();
        vendedorFake.Nome = "Maria A Santos";

        // Act
        var result = _vendedorValidator.TestValidate(vendedorFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome);
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_VendedorValidator_Porque_Telefone_Do_Vendedor_Eh_Nulo()
    {
        // Arrange
        var vendedorFake = VendedorFixture.VendedorFake();
        vendedorFake.Telefone = "";

        // Act
        var result = _vendedorValidator.TestValidate(vendedorFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Telefone);
        result.Errors.Count.Should().Be(2);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_VendedorValidator_Porque_Telefone_Do_Vendedor_Tem_Formato_Invalido()
    {
        // Arrange
        var vendedorFake = VendedorFixture.VendedorFake();
        vendedorFake.Telefone = "(11)995479812";

        // Act
        var result = _vendedorValidator.TestValidate(vendedorFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Telefone);
        result.Errors.Count.Should().Be(1);
    }
}