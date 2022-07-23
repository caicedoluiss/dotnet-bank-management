using MediatR;

namespace BankManagement.Application;

public class UpdateCustomerRequestCommand : IRequest<ExistentCustomerDTO>
{
  public int CustomerId { get; set; }
  public NewCustomerDTO? CustomerInfo { get; set; }
}
