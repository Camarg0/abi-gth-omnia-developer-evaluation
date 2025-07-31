
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.UpdateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.CreateSaleItem;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.UpdateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.DeleteSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SaleItemsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("{saleId}/items")]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleItemResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSaleItem([FromRoute] Guid saleId, [FromBody] CreateSaleItemRequest request, CancellationToken cancellationToken)
        {
            request.SaleId = saleId;

            var validator = new CreateSaleItemRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateSaleItemCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateSaleItemResponse>
            {
                Success = true,
                Message = "Sale item added successfully",
                Data = _mapper.Map<CreateSaleItemResponse>(response)
            });
        }

        [HttpPut("{saleId}/items/{itemId}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSaleItem([FromRoute] Guid saleId, [FromRoute] Guid itemId, [FromBody] UpdateSaleItemRequest request, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleItemRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<UpdateSaleItemCommand>(request);
            command.SaleId = saleId;
            command.Id = itemId;

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<UpdateSaleItemResponse>
            {
                Success = true,
                Message = $"Item '{itemId}' updated successfully in sale '{saleId}'",
                Data = _mapper.Map<UpdateSaleItemResponse>(result)
            });
        }

        [HttpDelete("{saleId}/items/{itemId}")]
        [ProducesResponseType(typeof(ApiResponseWithData<DeleteSaleItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSaleItem([FromRoute] Guid saleId, [FromRoute] Guid itemId, CancellationToken cancellationToken)
        {
            try
            {
                var request = new DeleteSaleItemRequest
                {
                    SaleId = saleId,
                    ItemId = itemId
                };

                var validator = new DeleteSaleItemRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var command = _mapper.Map<DeleteSaleItemCommand>(request);
                var result = await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponseWithData<DeleteSaleItemResponse>
                {
                    Success = true,
                    Message = $"Item with ID '{itemId}' from Sale '{saleId}' deleted successfully",
                    Data = _mapper.Map<DeleteSaleItemResponse>(result)
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Could not find a sale with ID '{saleId}' or item '{itemId}'"
                });
            }
        }
    }
}
