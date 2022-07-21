using MediatR;

namespace BankManagement.Application;

public class CreateTransactionRequestCommand : IRequest<int>
{
  public NewTransactionDTO? TransactionInfo { get; set; }
}
