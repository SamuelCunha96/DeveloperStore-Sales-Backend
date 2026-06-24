using DeveloperStore.Sales.Domain.Common;
using DeveloperStore.Sales.Domain.Events;

namespace DeveloperStore.Sales.Domain.Entities;

public class SaleItem : Entity
{
    public Guid SaleId { get; private set; }

    // External Identity — Product
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;

    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }

    private SaleItem() { } // EF Core

    public SaleItem(Guid saleId, Guid productId, string productName, int quantity, decimal unitPrice)
    {
        SaleId = saleId;
        ProductId = productId;
        ProductName = productName;
        Apply(quantity, unitPrice);
    }

    internal void Update(int quantity, decimal unitPrice) => Apply(quantity, unitPrice);

    public void Cancel()
    {
        if (IsCancelled)
            throw new DomainException("Item is already cancelled.");

        IsCancelled = true;
    }

    private void Apply(int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");
        if (quantity > 20)
            throw new DomainException("Cannot sell more than 20 identical items.");
        if (unitPrice <= 0)
            throw new DomainException("Unit price must be greater than zero.");

        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = ResolveDiscount(quantity);
        TotalAmount = quantity * unitPrice * (1 - Discount);
    }

    private static decimal ResolveDiscount(int quantity) => quantity switch
    {
        >= 10 => 0.20m,
        >= 4  => 0.10m,
        _     => 0m
    };
}
