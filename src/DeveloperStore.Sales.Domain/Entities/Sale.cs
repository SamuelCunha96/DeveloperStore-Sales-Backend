using DeveloperStore.Sales.Domain.Common;
using DeveloperStore.Sales.Domain.Events;

namespace DeveloperStore.Sales.Domain.Entities;

public class Sale : Entity
{
    public string SaleNumber { get; private set; } = string.Empty;
    public DateTime SaleDate { get; private set; }

    // External Identity — Customer
    public Guid CustomerId { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;

    // External Identity — Branch
    public Guid BranchId { get; private set; }
    public string BranchName { get; private set; } = string.Empty;

    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }

    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    private Sale() { } // EF Core

    public Sale(
        string saleNumber,
        DateTime saleDate,
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName)
    {
        if (string.IsNullOrWhiteSpace(saleNumber))
            throw new DomainException("Sale number is required.");
        if (string.IsNullOrWhiteSpace(customerName))
            throw new DomainException("Customer name is required.");
        if (string.IsNullOrWhiteSpace(branchName))
            throw new DomainException("Branch name is required.");

        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
    }

    public SaleItem AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (IsCancelled)
            throw new DomainException("Cannot add items to a cancelled sale.");

        var existing = _items.FirstOrDefault(i => i.ProductId == productId && !i.IsCancelled);
        if (existing is not null)
        {
            var merged = existing.Quantity + quantity;
            if (merged > 20)
                throw new DomainException("Cannot sell more than 20 identical items.");

            existing.Update(merged, unitPrice);
            RecalculateTotal();
            return existing;
        }

        var item = new SaleItem(Id, productId, productName, quantity, unitPrice);
        _items.Add(item);
        RecalculateTotal();
        return item;
    }

    public void Update(
        DateTime saleDate,
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName)
    {
        if (IsCancelled)
            throw new DomainException("Cannot update a cancelled sale.");
        if (string.IsNullOrWhiteSpace(customerName))
            throw new DomainException("Customer name is required.");
        if (string.IsNullOrWhiteSpace(branchName))
            throw new DomainException("Branch name is required.");

        SaleDate = saleDate;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;

        AddDomainEvent(new SaleModifiedEvent(Id, SaleNumber, TotalAmount));
    }

    public void CancelItem(Guid itemId)
    {
        if (IsCancelled)
            throw new DomainException("Cannot cancel an item of an already cancelled sale.");

        var item = _items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new DomainException($"Item '{itemId}' not found in this sale.");

        item.Cancel();
        RecalculateTotal();

        AddDomainEvent(new ItemCancelledEvent(Id, item.Id, item.ProductId, item.ProductName));
        AddDomainEvent(new SaleModifiedEvent(Id, SaleNumber, TotalAmount));
    }

    public void Cancel()
    {
        if (IsCancelled)
            throw new DomainException("Sale is already cancelled.");

        IsCancelled = true;

        foreach (var item in _items.Where(i => !i.IsCancelled))
            item.Cancel();

        RecalculateTotal();
        AddDomainEvent(new SaleCancelledEvent(Id, SaleNumber));
    }

    // Called by the Application layer after persisting, to raise SaleCreated with final total.
    public void RaiseSaleCreated() =>
        AddDomainEvent(new SaleCreatedEvent(
            Id, SaleNumber, CustomerId, CustomerName,
            BranchId, BranchName, SaleDate, TotalAmount));

    private void RecalculateTotal() =>
        TotalAmount = _items.Where(i => !i.IsCancelled).Sum(i => i.TotalAmount);
}
