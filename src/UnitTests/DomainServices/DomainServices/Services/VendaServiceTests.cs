using DomainModels.Entities;
using DomainModels.Enums;
using DomainServices.Exceptions;
using DomainServices.Services;
using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infraestructure.Data.Context;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnitTests.Fixtures;

namespace UnitTests.DomainServices.Services;

public class VendaServiceTests
{
    private readonly VendaService _vendaService;
    private readonly Mock<IUnitOfWork<ApplicationDbContext>> _mockUnitOfWork;
    private readonly Mock<IRepositoryFactory<ApplicationDbContext>> _mockRepositoryFactory;

    public VendaServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<ApplicationDbContext>>();
        _mockRepositoryFactory = new Mock<IRepositoryFactory<ApplicationDbContext>>();
        _vendaService = new(_mockUnitOfWork.Object, _mockRepositoryFactory.Object);
    }

    [Fact]
    public void Deveria_Executar_CadastrarVenda_Com_Sucesso()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake(); 

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Add(vendaFake))
            .Returns(vendaFake);

        // Act
        var vendaId = _vendaService.CadastrarVenda(vendaFake);

        // Assert
        vendaId.Should().Be(vendaFake.Id);
    }

    [Fact]
    public void Deveria_Falhar_Quando_Executar_CadastrarVenda_Porque_Status_Eh_Incorreto()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();
        vendaFake.Status = Status.PagamentoAprovado;

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Add(vendaFake))
            .Returns(vendaFake);

        // Act
        Action act = () => _vendaService.CadastrarVenda(vendaFake);

        // Assert
        act.Should().ThrowExactly<BadRequestException>($"Não é possível realizar uma compra com status: {vendaFake.Status}.");
    }

    [Fact]
    public void Deveria_Retornar_Venda_Quando_Executar_BuscaVendaPorId()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()))
            .Returns(It.IsAny<IQuery<Venda>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()))
            .Returns(vendaFake);

        // Act
        var vendaEncontrada = _vendaService.BuscarVendaPorId(vendaFake.Id);

        // Assert
        vendaEncontrada.Id.Should().Be(vendaFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()), Times.Once());
    }

    [Fact]
    public void Deveria_Retornar_Nulo_Quando_Executar_BuscaVendaPorId()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()));

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()));

        // Act
        var vendaEncontrada = _vendaService.BuscarVendaPorId(vendaFake.Id);

        // Assert
        vendaEncontrada.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()), Times.Once());
    }

    [Fact]
    public void Deveria_Passar_Quando_Executar_AtualizarVenda()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();
        var vendaEncontradaFake = VendaFixture.VendaFake();

        vendaFake.Status = Status.PagamentoAprovado;

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()))
            .Returns(It.IsAny<IQuery<Venda>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()))
            .Returns(vendaEncontradaFake);

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Update(It.IsAny<Venda>()));

        // Act
        _vendaService.AtualizarVenda(vendaFake.Id, vendaFake);

        // Assert
        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()))
            .Returns(It.IsAny<IQuery<Venda>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()))
            .Returns(vendaFake);

        _mockUnitOfWork.Verify(x => x.Repository<Venda>().Update(It.IsAny<Venda>()), Times.Once());
    }

    [Fact]
    public void Deveria_Gerar_Um_NotFoundException_Quando_Executar_AtualizarVenda()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();

        vendaFake.Status = Status.PagamentoAprovado;

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()));

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()));

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Update(It.IsAny<Venda>()));

        // Act
        Action act = () => _vendaService.AtualizarVenda(vendaFake.Id, vendaFake);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Venda para o Id: {vendaFake.Id} não encontrada.");

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()), Times.Once());
    }

    [Fact]
    public void Deveria_Gerar_Um_BadRequestException_Quando_Tentar_AtualizarVenda_Com_Status_de_AguardandoPagamento_Para_Status_Invalido()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();
        var vendaEncontradaFake = VendaFixture.VendaFake();

        vendaFake.Status = Status.EnviadoParaTransportadora;

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()))
            .Returns(It.IsAny<IQuery<Venda>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>())).Returns(vendaEncontradaFake);

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Update(It.IsAny<Venda>()));

        // Act
        Action act = () => _vendaService.AtualizarVenda(vendaFake.Id, vendaFake);

        // Assert
        act.Should()
            .ThrowExactly<BadRequestException>($"O status: {vendaEncontradaFake.Status} não pode ser atualizado para o status: {vendaFake.Status}.");

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()), Times.Once());
    }

    [Fact]
    public void Deveria_Gerar_Um_BadRequestException_Quando_Tentar_AtualizarVenda_Com_Status_de_PagamentoAprovado_Para_Status_Invalido()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();
        var vendaEncontradaFake = VendaFixture.VendaFake();

        vendaFake.Status = Status.Entregue;
        vendaEncontradaFake.Status = Status.PagamentoAprovado;

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()))
            .Returns(It.IsAny<IQuery<Venda>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>())).Returns(vendaEncontradaFake);

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Update(It.IsAny<Venda>()));

        // Act
        Action act = () => _vendaService.AtualizarVenda(vendaFake.Id, vendaFake);

        // Assert
        act.Should().ThrowExactly<BadRequestException>($"O status: {vendaEncontradaFake.Status} não pode ser atualizado para o status: {vendaFake.Status}.");

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()), Times.Once());
    }

    [Fact]
    public void Deveria_Passar_Quando_Tentar_AtualizarVenda__Com_Status_de_PagamentoAprovado_Para_Status_Valido()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();
        var vendaEncontradaFake = VendaFixture.VendaFake();

        vendaFake.Status = Status.EnviadoParaTransportadora;
        vendaEncontradaFake.Status = Status.PagamentoAprovado;

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()))
            .Returns(It.IsAny<IQuery<Venda>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>())).Returns(vendaEncontradaFake);

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Update(It.IsAny<Venda>()));

        // Act
        _vendaService.AtualizarVenda(vendaFake.Id, vendaFake);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.Repository<Venda>().Update(It.IsAny<Venda>()), Times.Once());
    }

   [Fact]
    public void Deveria_Gerar_Um_BadRequestException_Quando_Tentar_AtualizarVenda_Com_Status_de_EnviadoParaTransportadora_Para_Status_Invalido()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();
        var vendaEncontradaFake = VendaFixture.VendaFake();

        vendaFake.Status = Status.Cancelada;
        vendaEncontradaFake.Status = Status.EnviadoParaTransportadora;

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()))
            .Returns(It.IsAny<IQuery<Venda>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>())).Returns(vendaEncontradaFake);

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Update(It.IsAny<Venda>()));

        // Act
        Action act = () => _vendaService.AtualizarVenda(vendaFake.Id, vendaFake);

        // Assert
        act.Should()
            .ThrowExactly<BadRequestException>($"O status: {vendaEncontradaFake.Status} não pode ser atualizado para o status: {vendaFake.Status}.");

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()), Times.Once());
    }

    [Fact]
    public void Deveria_Passar_Quando_Tentar_AtualizarVenda_Com_Status_de_EnviadoParaTransportadora_Para_Status_Valido()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();
        var vendaEncontradaFake = VendaFixture.VendaFake();

        vendaFake.Status = Status.Entregue;
        vendaEncontradaFake.Status = Status.EnviadoParaTransportadora;

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()))
            .Returns(It.IsAny<IQuery<Venda>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>())).Returns(vendaEncontradaFake);

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Update(It.IsAny<Venda>()));

        // Act
        _vendaService.AtualizarVenda(vendaFake.Id, vendaFake);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.Repository<Venda>().Update(It.IsAny<Venda>()), Times.Once());
    }

    [Fact]
    public void Deveria_Gerar_Um_BadRequestException_Quando_Tentar_AtualizarVenda_Com_Status_de_Entregue_Para_Status_Invalido()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();
        var vendaEncontradaFake = VendaFixture.VendaFake();

        vendaFake.Status = Status.Cancelada;
        vendaEncontradaFake.Status = Status.Entregue;

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()))
            .Returns(It.IsAny<IQuery<Venda>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>())).Returns(vendaEncontradaFake);

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Update(It.IsAny<Venda>()));

        // Act
        Action act = () => _vendaService.AtualizarVenda(vendaFake.Id, vendaFake);

        // Assert
        act.Should()
            .ThrowExactly<BadRequestException>($"O status: {vendaEncontradaFake.Status} não pode ser atualizado para o status: {vendaFake.Status}.");

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()), Times.Once());
    }

    [Fact]
    public void Deveria_Gerar_Um_BadRequestException_Quando_Tentar_AtualizarVenda_Com_Status_Inexistente()
    {
        // Arrange
        var vendaFake = VendaFixture.VendaFake();
        var vendaEncontradaFake = VendaFixture.VendaFake();

        vendaEncontradaFake.Status = (Status)6;

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, Vendedor>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, ICollection<Item>>>>()))
            .Returns(It.IsAny<IQuery<Venda>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>())).Returns(vendaEncontradaFake);

        _mockUnitOfWork.Setup(x => x.Repository<Venda>().Update(It.IsAny<Venda>()));

        // Act
        Action act = () => _vendaService.AtualizarVenda(vendaFake.Id, vendaFake);

        // Assert
        act.Should().ThrowExactly<BadRequestException>($"Status: {vendaEncontradaFake.Status} não existe.");

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Venda, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>(),
                It.IsAny<Func<IQueryable<Venda>,
                IIncludableQueryable<Venda, object>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Venda>()
            .FirstOrDefault(It.IsAny<IQuery<Venda>>()), Times.Once());
    }
}