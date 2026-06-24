namespace DeveloperStore.Sales.API.Models.Requests;

public record UpdateSaleRequest(
    DateTime SaleDate,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName);
