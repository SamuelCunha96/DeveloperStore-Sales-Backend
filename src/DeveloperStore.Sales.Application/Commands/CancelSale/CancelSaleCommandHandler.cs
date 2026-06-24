using DeveloperStore.Sales.Application.Common.Exceptions;
using DeveloperStore.Sales.Application.Common.Extensions;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Repositories;
using MediatR;

namespace DeveloperStore.Sales.Application.Commands.CancelSale;

public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand>
{
    private readonly ISaleRepository _repository;
    private readonly IPublisher _publisher;

    public CancelSaleCommandHandler(ISaleRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Sale), request.Id);

        sale.Cancel();

        await _repository.UpdateAsync(sale, cancellationToken);
        await _publisher.PublishDomainEventsAsync(sale, cancellationToken);
    }
}
