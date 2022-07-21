using MediatR;

namespace BankManagement.Application;

public class DeleteCustomerRequestCommand : IRequest<ExistentCustomerDTO>
{
  public int CustomerId { get; set; }
}
