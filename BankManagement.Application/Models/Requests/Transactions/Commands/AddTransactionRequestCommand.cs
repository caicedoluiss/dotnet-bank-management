using MediatR;

namespace BankManagement.Application;

public class AddTransactionRequestCommand : IRequest<int>
{
  public UpdatingTransactionDTO? TransctionInfo { get; set; }
}
