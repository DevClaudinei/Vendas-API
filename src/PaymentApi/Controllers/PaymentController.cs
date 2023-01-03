using Application.Models;
using AppServices;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace PaymentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IVendaAppService _vendaAppService;

    public PaymentsController(IVendaAppService vendaAppService)
    {
        _vendaAppService = vendaAppService ?? throw new System.ArgumentNullException(nameof(vendaAppService));
    }

    [HttpPost]
    public IActionResult Post(CreateVendaRequest vendaRequest)
    {
        try
        {
            var createdCustomer = _vendaAppService.Create(vendaRequest);
            return Created("~https://localhost:7145/api/Payments", createdCustomer);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(long id)
    {
        try
        {
            var customerFoundId = _vendaAppService.GetById(id);
            return Ok(customerFoundId);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(long id, UpdateVendaRequest updateVendaRequest)
    {
        try
        {
            _vendaAppService.Update(id, updateVendaRequest);
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