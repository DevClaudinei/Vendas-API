using Application.Models;
using AutoMapper;
using DomainModels.Entities;

public class VendaProfile : Profile
{
    public VendaProfile()
    {
        CreateMap<Venda, VendaResult>()
            .ForMember(x => x.IdVenda, opts => opts.MapFrom(source => source.Id));
        CreateMap<UpdateVendaRequest, Venda>();
        CreateMap<CreateVendaRequest, Venda>();
    }
}