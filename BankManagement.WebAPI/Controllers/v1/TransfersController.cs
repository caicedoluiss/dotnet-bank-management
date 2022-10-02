using System.Threading.Tasks;
using BankManagement.Application;
using BankManagement.WebAPI.Responses.v1;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankManagement.WebAPI.Controllers.v1;

[Route("api/v1/[controller]")]
[Produces("application/json")]
[ApiController]
public class TransfersController : ControllerBase
{
  private readonly ILogger<TransfersController> logger;
  private readonly IMediator mediator;

  public TransfersController(ILogger<TransfersController> logger, IMediator mediator)
  {
    this.logger = logger;
    this.mediator = mediator;
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Response<ExistentTransferDTO?>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Response<ExistentTransferDTO?>), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(Response<ExistentTransferDTO?>), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnGetAsync([FromRoute] int id, [FromQuery] bool getAccount = false, [FromQuery] bool getCustomer = false
                                              , [FromQuery] bool getDestAccount = false, [FromQuery] bool getDestCustomer = false)
  {
    var request = new GetTransferByIdRequest
    {
      TransferId = id,
      RetrieveAccountInfo = getAccount,
      RetrieveCustomerInfo = getCustomer,
      RetrieveDestinationAccountInfo = getDestAccount,
      RetrieveDestinationAccountCustomerInfo = getDestCustomer
    };

    var result = await mediator.Send(request);

    var response = new Response<ExistentTransferDTO> { Data = result };

    return Ok(response);
  }

  [HttpPost()]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnPostAsync([FromBody] NewTransferDTO transfer)
  {
    var request = new CreateTransferRequestCommand { TransferInfo = transfer };

    var result = await mediator.Send(request);

    var response = new Response<int> { Data = result };

    return Ok(response);
  }

  [HttpPost("add")]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnPostAsync([FromBody] UpdatingTransferDTO transfer)
  {
    var request = new AddTransferRequestCommand { TransferInfo = transfer };

    var result = await mediator.Send(request);

    var response = new Response<int> { Data = result };

    return Ok(response);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnPutAsync([FromRoute] int id, [FromBody] UpdatingTransferDTO transaction)
  {
    var command = new UpdateTransferRequestCommand { TransferId = id, TransferInfo = transaction };

    _ = await mediator.Send(command);

    return NoContent();
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnDeleteAsync([FromRoute] int id)
  {
    var command = new DeleteTransferRequestCommand { TransferId = id };

    _ = await mediator.Send(command);

    return NoContent();
  }
}
