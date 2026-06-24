using AutoMapper;
using DeveloperStore.Sales.Application.Common.Extensions;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Repositories;
using MediatR;

namespace DeveloperStore.Sales.Application.Commands.CreateSale;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleDto>
{
    private readonly ISaleRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;

    public CreateSaleCommandHandler(ISaleRepository repository, IMapper mapper, IPublisher publisher)
    {
        _repository = repository;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<SaleDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = new Sale(
            request.SaleNumber,
            request.SaleDate,
            request.CustomerId,
            request.CustomerName,
            request.BranchId,
            request.BranchName);

        foreach (var item in request.Items)
            sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);

        await _repository.AddAsync(sale, cancellationToken);

        sale.RaiseSaleCreated();
        await _publisher.PublishDomainEventsAsync(sale, cancellationToken);

        return _mapper.Map<SaleDto>(sale);
    }
}
