using AutoMapper;
using DeveloperStore.Sales.Application.Common.Exceptions;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Repositories;
using MediatR;

namespace DeveloperStore.Sales.Application.Queries.GetSale;

public class GetSaleQueryHandler : IRequestHandler<GetSaleQuery, SaleDto>
{
    private readonly ISaleRepository _repository;
    private readonly IMapper _mapper;

    public GetSaleQueryHandler(ISaleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SaleDto> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Sale), request.Id);

        return _mapper.Map<SaleDto>(sale);
    }
}
