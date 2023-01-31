using AppServices.Services;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Interfaces;
using FluentAssertions;
using Moq;
using System;
using UnitTests.Fixtures;

namespace UnitTests.AppServices.Services;

public class VendaAppServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IVendaService> _vendaServiceMock;
    private readonly VendaAppService _vendaAppSevice;

    public VendaAppServiceTests()
    {
        var config = new MapperConfiguration(opt =>
        {
            opt.AddProfile(new VendaProfile());
        });
        _mapper = config.CreateMapper();
        _vendaServiceMock = new ();
        _vendaAppSevice = new(_vendaServiceMock.Object, _mapper);
    }

    [Fact]
    public void Deveria_Executar_CadastrarVenda_Com_Sucesso()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaRequestFake();

        _vendaServiceMock.Setup(x => x.CadastrarVenda(It.IsAny<Venda>()))
            .Returns(It.IsAny<long>);

        // Act
        var vendaId = _vendaAppSevice.CadastrarVenda(vendaFake);

        // Assert
        vendaId.Should().Be(0);
        _vendaServiceMock.Verify(x => x.CadastrarVenda(It.IsAny<Venda>()), Times.Once);
    }

    [Fact]
    public void Deveria_Retornar_Venda_Quando_Executar_BuscarVendaPorId()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();

        _vendaServiceMock.Setup(x => x.BuscarVendaPorId(It.IsAny<long>()))
            .Returns(vendaFake);

        // Act
        var venda = _vendaAppSevice.BuscarVendaPorId(vendaFake.Id);

        // Assert
        venda.IdVenda.Should().Be(vendaFake.Id);

        _vendaServiceMock.Verify(x => x.BuscarVendaPorId(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_BuscarVendaPorId() 
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();

        _vendaServiceMock.Setup(x => x.BuscarVendaPorId(It.IsAny<long>()));

        // Act
        Action act = () => _vendaAppSevice.BuscarVendaPorId(vendaFake.Id);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Venda para o Id: {vendaFake.Id} não encontrada.");
        _vendaServiceMock.Verify(x => x.BuscarVendaPorId(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public void Deveria_AtualizarVenda_Com_Sucesso()
    {
        // Arrange
        var vendaFake = VendaFixture.UpdateVendaRequest();
        var id = 1L;

        _vendaServiceMock.Setup(x => x.AtualizarVenda(It.IsAny<long>(), It.IsAny<Venda>()));

        // Act
        _vendaAppSevice.AtualizarVenda(id, vendaFake);

        // Assert
        _vendaServiceMock.Verify(x => x.AtualizarVenda(It.IsAny<long>(), It.IsAny<Venda>()), Times.Once);
    }
}