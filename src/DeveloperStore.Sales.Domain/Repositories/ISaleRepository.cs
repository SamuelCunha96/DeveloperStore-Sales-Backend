using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Sale> Items, int TotalCount)> GetAllAsync(
        int page,
        int pageSize,
        string? orderBy = null,
        CancellationToken cancellationToken = default);
    Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default);
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
