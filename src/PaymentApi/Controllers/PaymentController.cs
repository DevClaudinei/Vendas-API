using Application.Models;
using AppServices.Services.Interfaces;
using DomainModels.Entities;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PaymentApi.Controllers;

/// <summary>
/// Declaração de controlador implementado ControllerBase
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IVendaAppService _vendaAppService;

    /// <summary>
    /// Construtor recebe como parâmetro a implementação da camada de aplicação
    /// </summary>
    /// <param name="vendaAppService"></param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public PaymentsController(IVendaAppService vendaAppService)
    {
        _vendaAppService = vendaAppService ?? throw new System.ArgumentNullException(nameof(vendaAppService));
    }

    /// <summary>
    /// Salva dados de uma venda.
    /// </summary>
    /// <param name="vendaRequest"></param>
    /// <returns>Retorna o Id da venda salva no banco de dados</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Payments
    ///      {
    ///          "dataVenda": "2023-01-30T21:33:47.494Z",
    ///          "vendedor": {
    ///              "cpf": "11111111111",
    ///              "nome": "João de Lima",
    ///              "telefone": "(71) 98705-5669"
    ///          },
    ///          "itens": [
    ///              {
    ///                  "name": "MacBook Air"
    ///              }
    ///          ],
    ///          "status": 1
    ///      }
    ///
    /// </remarks>
    /// <response code="201">Retorna o Id da venda salva no banco de dados</response>
    /// <response code="400">Se o status da venda é diferente de aguardando pagamento</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult Post(CreateVendaRequest vendaRequest)
    {
        try
        {
            var vendaId = _vendaAppService.CadastrarVenda(vendaRequest);
            return Created("~https://localhost:7145/api/Payments", vendaId);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Mostra informações de uma venda.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Mostra informações de uma venda</returns>
    /// <response code="200">Retorna as informações de uma venda</response>
    /// <response code="400">Se o Id informado não corresponde a uma venda no banco de dados</response>
    [HttpGet("{id}", Name = "GetById")]
    [ProducesResponseType(typeof(Venda), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        try
        {
            var vendaEncontrada = _vendaAppService.BuscarVendaPorId(id);
            return Ok(vendaEncontrada);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Realiza atualização do status do pagamento de uma venda.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateVendaRequest"></param>
    /// <returns>Realiza atualização do status de uma venda</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /Payments/{id}
    ///     {
    ///         "status": 2
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Atualiza o status da venda e salva no banco de dados</response>
    /// <response code="404">Se o Id informado não corresponde a uma venda no banco de dados</response>
    /// <response code="400">Se não for possível executar a transição de status por regra de negócio</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult Put(long id, UpdateVendaRequest updateVendaRequest)
    {
        try
        {
            _vendaAppService.AtualizarVenda(id, updateVendaRequest);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}