using AutoMapper;
using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Domain.Repositories;
using MediatR;

namespace DeveloperStore.Sales.Application.Queries.GetSales;

public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, PaginatedList<SaleDto>>
{
    private readonly ISaleRepository _repository;
    private readonly IMapper _mapper;

    public GetSalesQueryHandler(ISaleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SaleDto>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.GetAllAsync(
            request.Page,
            request.PageSize,
            request.OrderBy,
            cancellationToken);

        var dtos = _mapper.Map<IEnumerable<SaleDto>>(items);

        return new PaginatedList<SaleDto>(dtos, totalCount, request.Page, request.PageSize);
    }
}
