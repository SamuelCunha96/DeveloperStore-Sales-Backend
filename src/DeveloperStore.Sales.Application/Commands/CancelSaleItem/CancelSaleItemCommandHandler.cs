using DeveloperStore.Sales.Application.Common.Exceptions;
using DeveloperStore.Sales.Application.Common.Extensions;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Repositories;
using MediatR;

namespace DeveloperStore.Sales.Application.Commands.CancelSaleItem;

public class CancelSaleItemCommandHandler : IRequestHandler<CancelSaleItemCommand>
{
    private readonly ISaleRepository _repository;
    private readonly IPublisher _publisher;

    public CancelSaleItemCommandHandler(ISaleRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.SaleId, cancellationToken)
            ?? throw new NotFoundException(nameof(Sale), request.SaleId);

        sale.CancelItem(request.ItemId);

        await _repository.UpdateAsync(sale, cancellationToken);
        await _publisher.PublishDomainEventsAsync(sale, cancellationToken);
    }
}
