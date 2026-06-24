using DeveloperStore.Sales.Application.DTOs;
using MediatR;

namespace DeveloperStore.Sales.Application.Commands.UpdateSale;

public record UpdateSaleCommand(
    Guid Id,
    DateTime SaleDate,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName) : IRequest<SaleDto>;
