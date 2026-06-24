using MediatR;

namespace DeveloperStore.Sales.Application.Commands.CancelSaleItem;

public record CancelSaleItemCommand(Guid SaleId, Guid ItemId) : IRequest;
