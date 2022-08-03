using MediatR;

namespace BankManagement.Application;

public class GetCustomerByIdRequest : IRequest<ExistentCustomerDTO?>
{
  public int CustomerId { get; set; }
}
