using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infrastructure.Persistence.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly SalesDbContext _context;

    public SaleRepository(SalesDbContext context) => _context = context;

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
        => await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);

    public async Task<(IEnumerable<Sale> Items, int TotalCount)> GetAllAsync(
        int page,
        int pageSize,
        string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.Include(s => s.Items).AsQueryable();

        query = orderBy?.ToLowerInvariant() switch
        {
            "salenumber"       => query.OrderBy(s => s.SaleNumber),
            "salenumber desc"  => query.OrderByDescending(s => s.SaleNumber),
            "saledate"         => query.OrderBy(s => s.SaleDate),
            "saledate desc"    => query.OrderByDescending(s => s.SaleDate),
            "totalamount"      => query.OrderBy(s => s.TotalAmount),
            "totalamount desc" => query.OrderByDescending(s => s.TotalAmount),
            _                  => query.OrderByDescending(s => s.SaleDate)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await _context.Sales.FindAsync([id], cancellationToken);
        if (sale is not null)
        {
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
