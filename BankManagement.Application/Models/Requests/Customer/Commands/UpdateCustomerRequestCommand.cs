using MediatR;

namespace BankManagement.Application;

public class UpdateCustomerRequestCommand : IRequest<ExistentCustomerDTO>
{
  public NewCustomerDTO? CustomerInfo { get; set; }
}
