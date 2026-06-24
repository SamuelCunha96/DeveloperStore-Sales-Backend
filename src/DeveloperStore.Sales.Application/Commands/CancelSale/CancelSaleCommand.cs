using MediatR;

namespace DeveloperStore.Sales.Application.Commands.CancelSale;

public record CancelSaleCommand(Guid Id) : IRequest;
