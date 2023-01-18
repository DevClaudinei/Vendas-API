using AppServices.Validations;
using FluentValidation.TestHelper;
using UnitTests.Fixtures;
using FluentAssertions;

namespace UnitTests.AppServices.Validations;

public class CreateVendaValidatorTests
{
    private readonly CreateVendaValidator _createVendaValidator = new ();

    [Fact]
    public void Should_Pass_When_Executing_CreateVendaValidator()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CreateVendaValidator_Porque_Cpf_Eh_Nulo()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Vendedor.Cpf = "";

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Vendedor.Cpf);
        result.Errors.Count.Should().Be(2);
    }

    [Fact]
    public void Should_Fail_When_Executing_CreateVendaValidator_Because_Cpf_Is_Invalid()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Vendedor.Cpf = "008.216.795-89";

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Vendedor.Cpf);
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CreateVendaValidator_Porque_Nao_Tem_Nome_Do_Vendedor()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Vendedor.Nome = "";

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Vendedor.Nome);
        result.Errors.Count.Should().Be(4);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CreateVendaValidator_Porque_Vendedor_Nao_Tem_Sobrenome()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Vendedor.Nome = "José";

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Vendedor.Nome);
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CreateVendaValidator_Porque_Nome_De_Vendedor_Tem_Caracteres_Invalidos()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Vendedor.Nome = "Jo@o Conceição";

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Vendedor.Nome);
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CreateVendaValidator_Porque_Nome_De_Vendedor_Tem_Espacos_Desnecessarios()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Vendedor.Nome = "Solange  Braga";

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Vendedor.Nome);
        result.Errors.Count.Should().Be(2);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CreateVendaValidator_Porque_Nome_De_Vendedor_Tem_Sobrenome_Invalido()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Vendedor.Nome = "Maria A Santos";

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Vendedor.Nome);
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CreateVendaValidator_Porque_Telefone_Do_Vendedor_Eh_Nulo()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Vendedor.Telefone = "";

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Vendedor.Telefone);
        result.Errors.Count.Should().Be(2);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CreateVendaValidator_Porque_Telefone_Do_Vendedor_Tem_Formato_Invalido()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Vendedor.Telefone = "(11)995479812";

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Vendedor.Telefone);
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CreateVendaValidator_Porque_Status_Da_Venda_Eh_Incorreto()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Status = 0;

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status);
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CreateVendaValidator_Porque_Nao_Possui_Itens()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();
        vendaFake.Itens.Clear();

        // Act
        var result = _createVendaValidator.TestValidate(vendaFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Itens);
        result.Errors.Count.Should().Be(1);
    }
}