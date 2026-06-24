using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Application.DTOs;
using MediatR;

namespace DeveloperStore.Sales.Application.Queries.GetSales;

public record GetSalesQuery(
    int Page = 1,
    int PageSize = 10,
    string? OrderBy = null) : IRequest<PaginatedList<SaleDto>>;
