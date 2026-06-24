using DeveloperStore.Sales.Application.DTOs;
using MediatR;

namespace DeveloperStore.Sales.Application.Queries.GetSale;

public record GetSaleQuery(Guid Id) : IRequest<SaleDto>;
