using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSaleByNumber;
using Ambev.DeveloperEvaluation.Application.Sales.GetSalesByCustomerId;
using Ambev.DeveloperEvaluation.Application.Sales.GetSalesByBranchId;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleByNumber;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByCustomerId;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByBranchId;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            // http validations
            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // mapping to the command (application layer)
            var command = _mapper.Map<CreateSaleCommand>(request);

            // sending to the application layer
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = _mapper.Map<CreateSaleResponse>(response)
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new GetSaleRequest { Id = id };

                // http validations
                var validator = new GetSaleRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // mapping to the query (application layer)
                var query = _mapper.Map<GetSaleQuery>(request);

                // sending to the application layer
                var response = await _mediator.Send(query, cancellationToken);

                return Ok(new ApiResponseWithData<GetSaleResponse>
                {
                    Success = true,
                    Message = "Sale retrieved successfully",
                    Data = _mapper.Map<GetSaleResponse>(response)
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Sale with ID {id} not found"
                });
            }
        }

        [HttpGet("number/{saleNumber}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleByNumberResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSaleByNumber([FromRoute] string saleNumber, CancellationToken cancellationToken)
        {
            try
            {
                var request = new GetSaleByNumberRequest { SaleNumber = saleNumber };

                // http validations
                var validator = new GetSaleByNumberRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // mapping to the query (application layer)
                var query = _mapper.Map<GetSaleByNumberQuery>(request);

                // sending to the application layer
                var response = await _mediator.Send(query, cancellationToken);

                return Ok(new ApiResponseWithData<GetSaleByNumberResponse>
                {
                    Success = true,
                    Message = "Sale retrieved successfully",
                    Data = _mapper.Map<GetSaleByNumberResponse>(response)
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Sale with number '{saleNumber}' not found"
                });
            }
        }

        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSalesByCustomerIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSalesByCustomerId([FromRoute] Guid customerId, CancellationToken cancellationToken)
        {
            try
            {
                var request = new GetSalesByCustomerIdRequest { CustomerId = customerId };

                // http validations
                var validator = new GetSalesByCustomerIdRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // mapping to the query (application layer)
                var query = _mapper.Map<GetSalesByCustomerIdQuery>(request);

                // sending to the application layer
                var response = await _mediator.Send(query, cancellationToken);

                return Ok(new ApiResponseWithData<GetSalesByCustomerIdResponse>
                {
                    Success = true,
                    Message = $"Found {response.TotalSales} sales for customer",
                    Data = _mapper.Map<GetSalesByCustomerIdResponse>(response)
                });
            }

            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Could not find any sales for the customer '{customerId}'"
                });
            }
        }

        [HttpGet("branch/{branchId}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSalesByBranchIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSalesByBranchId([FromRoute] Guid branchId, CancellationToken cancellationToken)
        {
            try
            {
                var request = new GetSalesByBranchIdRequest { BranchId = branchId };

                // http validations
                var validator = new GetSalesByBranchIdRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // mapping to the query (application layer)
                var query = _mapper.Map<GetSalesByBranchIdQuery>(request);

                // sending to the application layer
                var response = await _mediator.Send(query, cancellationToken);

                return Ok(new ApiResponseWithData<GetSalesByBranchIdResponse>
                {
                    Success = true,
                    Message = $"Found {response.TotalSales} sales for branch",
                    Data = _mapper.Map<GetSalesByBranchIdResponse>(response)
                });
            }

            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Could not find any sales in the branch '{branchId}'"
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<DeleteSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new DeleteSaleRequest { Id = id };

                // http validations
                var validator = new DeleteSaleRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // mapping to the command (application layer)
                var command = _mapper.Map<DeleteSaleCommand>(request);

                // sending to the application layer
                var response = await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponseWithData<DeleteSaleResponse>
                {
                    Success = true,
                    Message = $"Sale with ID '{id}' deleted succesfully",
                    Data = _mapper.Map<DeleteSaleResponse>(response)
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Could not find any sale with ID '{id}'"
                });
            }
        }

        [HttpPatch("{id}/cancel")]
        [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new CancelSaleRequest { Id = id };

                // http validations
                var validator = new CancelSaleRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // mapping to the command (application layer)
                var command = _mapper.Map<CancelSaleCommand>(request);

                // sending to the application layer
                var response = await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponseWithData<CancelSaleResponse>
                {
                    Success = true,
                    Message = $"Sale with ID '{id}' cancelled succesfully",
                    Data = _mapper.Map<CancelSaleResponse>(response)
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Could not find any sale with ID '{id}'"
                });
            }
        }
    }
}
