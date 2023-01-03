using Application.Models;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Interfaces;
using System;

namespace AppServices;

public class VendaAppService : IVendaAppService
{
    private readonly IVendaService _vendaService;
    private readonly IMapper _mapper;

    public VendaAppService(IVendaService vendaService, IMapper mapper)
    {
        _vendaService = vendaService ?? throw new ArgumentNullException(nameof(vendaService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public long Create(CreateVendaRequest createVendaRequest)
    {
        var vendaMap = _mapper.Map<Venda>(createVendaRequest);

        return _vendaService.Create(vendaMap);
    }

    public VendaResult GetById(long id)
    {
        var vendaFound = _vendaService.GetById(id) ??
            throw new NotFoundException($"Venda para o Id: {id} não encontrada.");
        
        return _mapper.Map<VendaResult>(vendaFound);
    }

    public void Update(long id, UpdateVendaRequest updateVendaRequest)
    {
        var vendaMap = _mapper.Map<Venda>(updateVendaRequest);

        _vendaService.Update(id, vendaMap);
    }
}