using DeveloperStore.Sales.API.Models.Requests;
using DeveloperStore.Sales.API.Models.Responses;
using DeveloperStore.Sales.Application.Commands.CancelSale;
using DeveloperStore.Sales.Application.Commands.CancelSaleItem;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Commands.UpdateSale;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Queries.GetSale;
using DeveloperStore.Sales.Application.Queries.GetSales;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Sales.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Returns a paginated list of sales.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetSalesQuery(page, pageSize, orderBy), cancellationToken);
        return Ok(ApiResponse<object>.Ok(result));
    }

    /// <summary>Returns a sale by its ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SaleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSaleQuery(id), cancellationToken);
        return Ok(ApiResponse<SaleDto>.Ok(result));
    }

    /// <summary>Creates a new sale.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SaleDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateSaleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<SaleDto>.Ok(result, "Sale created successfully."));
    }

    /// <summary>Updates the header of an existing sale.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SaleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSaleCommand(
            id,
            request.SaleDate,
            request.CustomerId,
            request.CustomerName,
            request.BranchId,
            request.BranchName);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(ApiResponse<SaleDto>.Ok(result, "Sale updated successfully."));
    }

    /// <summary>Cancels a sale.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelSaleCommand(id), cancellationToken);
        return Ok(ApiResponse.Ok("Sale cancelled successfully."));
    }

    /// <summary>Cancels a specific item within a sale.</summary>
    [HttpDelete("{id:guid}/items/{itemId:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CancelItem(
        Guid id,
        Guid itemId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelSaleItemCommand(id, itemId), cancellationToken);
        return Ok(ApiResponse.Ok("Item cancelled successfully."));
    }
}
