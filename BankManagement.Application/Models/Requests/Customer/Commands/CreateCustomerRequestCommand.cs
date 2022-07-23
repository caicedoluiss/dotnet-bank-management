using MediatR;

namespace BankManagement.Application;

public class CreateCustomerRequestCommand : IRequest<int>
{
  public NewCustomerDTO? CustomerInfo { get; set; }
}
