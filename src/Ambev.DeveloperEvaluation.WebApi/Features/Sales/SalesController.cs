using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.PutSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.PutSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sale operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<SalesController> _logger;

    /// <summary>
    /// Initializes a new instance of SalesController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public SalesController(IMediator mediator, IMapper mapper, ILogger<SalesController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="request">The sale creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Sale validation error: {Errors}", validationResult.Errors);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Sale validation error",
                    Errors = validationResult.Errors.Select(e => (ValidationErrorDetail)e)
                });
            }

            var command = _mapper.Map<CreateSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = _mapper.Map<CreateSaleResponse>(response)
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HttpRequestException");
            var apiResponse = new ApiResponse
            {
                Success = false,
                Message = ex.Message,
                Errors = [new ValidationErrorDetail { Error = "HttpRequestException", Detail = ex.Message }]
            };
            return new ObjectResult(apiResponse) { StatusCode = (int?)ex.StatusCode };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sale creation error");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Sale creation error",
                Errors = [new ValidationErrorDetail { Error = "Exception", Detail = ex.Message }]
            });
        }
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="request">The sale update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<PutSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutSale([FromRoute] Guid id, [FromBody] PutSaleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty)
                request.Id = id;
            else if (id != request.Id)
            {
                _logger.LogError("Sale validation error: The ID in the URL does not match the ID in the request body");
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Sale validation error",
                    Errors = [new ValidationErrorDetail { Error = "Id", Detail = "The ID in the URL does not match the ID in the request body" }]
                });
            }

            var validator = new PutSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Sale validation error: {Errors}", validationResult.Errors);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Sale validation error",
                    Errors = validationResult.Errors.Select(e => (ValidationErrorDetail)e)
                });
            }

            var command = _mapper.Map<PutSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<PutSaleResponse>(response), "Sale updated successfully");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HttpRequestException");
            var apiResponse = new ApiResponse
            {
                Success = false,
                Message = ex.Message,
                Errors = [new ValidationErrorDetail { Error = "HttpRequestException", Detail = ex.Message }]
            };
            return new ObjectResult(apiResponse) { StatusCode = (int?)ex.StatusCode };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sale update error");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Sale update error",
                Errors = [new ValidationErrorDetail { Error = "Exception", Detail = ex.Message }]
            });
        }
    }

    /// <summary>
    /// Retrieves a sale by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetSaleRequest { Id = id };
            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Sale validation error: {Errors}", validationResult.Errors);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Sale validation error",
                    Errors = validationResult.Errors.Select(e => (ValidationErrorDetail)e)
                });
            }

            var command = _mapper.Map<GetSaleCommand>(request.Id);            
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<GetSaleResponse>(response), "Sale retrieved successfully");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HttpRequestException");
            var apiResponse = new ApiResponse
            {
                Success = false,
                Message = ex.Message,
                Errors = [new ValidationErrorDetail { Error = "HttpRequestException", Detail = ex.Message }]
            };

            return new ObjectResult(apiResponse) { StatusCode = (int?)ex.StatusCode };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sale get error");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Sale get error",
                Errors = [new ValidationErrorDetail { Error = "Exception", Detail = ex.Message }]
            });
        }
    }

    /// <summary>
    /// Retrieves a list of sales by page and size order by a specific fields, and filters by specific fields
    /// </summary>
    /// <param name="_page">The page number</param>
    /// <param name="_size">The page size</param>
    /// <param name="_order">The order fields (Field1 asc, Field2 desc)</param>
    /// <param name="filters">The filters (Key: Field, Value: Value)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of sales and products list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GetSaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSaleList([FromQuery] int _page = 1, [FromQuery] int _size = 10, [FromQuery] string _order = "", [FromQuery] Dictionary<string, string>? filters = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (filters != null)
            {
                filters.Remove("_page");
                filters.Remove("_size");
                filters.Remove("_order");
            }

            var request = new GetSaleRequest { Page = _page, PageSize = _size, OrderFields = _order, Filters = filters };
            
            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Sale validation error: {Errors}", validationResult.Errors);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Sale validation error",
                    Errors = validationResult.Errors.Select(e => (ValidationErrorDetail)e)
                });
            }

            var command = _mapper.Map<GetSaleCommandList>(request);
            var response = await _mediator.Send(command, cancellationToken);

            var pageList = new PaginatedList<GetSaleResult>(response.SaleList, response.TotalCount, _page, _size);
            return OkPaginated(pageList);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HttpRequestException");
            var apiResponse = new ApiResponse
            {
                Success = false,
                Message = ex.Message,
                Errors = [new ValidationErrorDetail { Error = "HttpRequestException", Detail = ex.Message }]
            };
            return new ObjectResult(apiResponse) { StatusCode = (int?)ex.StatusCode };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sale get error");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Sale get error",
                Errors = [new ValidationErrorDetail { Error = "Exception", Detail = ex.Message }]
            });
        }
    }

    /// <summary>
    /// Deletes a sale by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the sale was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteSaleRequest { Id = id };
            var validator = new DeleteSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Sale delete validation error: {Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            var command = _mapper.Map<DeleteSaleCommand>(request.Id);
            await _mediator.Send(command, cancellationToken);

            return Ok("Sale deleted successfully");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HttpRequestException");
            var apiResponse = new ApiResponse
            {
                Success = false,
                Message = ex.Message,
                Errors = [new ValidationErrorDetail { Error = "HttpRequestException", Detail = ex.Message }]
            };

            return new ObjectResult(apiResponse) { StatusCode = (int?)ex.StatusCode };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sale delete error");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Sale delete error",
                Errors = [new ValidationErrorDetail { Error = "Exception", Detail = ex.Message }]
            });
        }
    }
}
