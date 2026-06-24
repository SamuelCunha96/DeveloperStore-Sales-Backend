using AutoMapper;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Application.Mappings;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<SaleItem, SaleItemDto>();

        CreateMap<Sale, SaleDto>()
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));
    }
}
