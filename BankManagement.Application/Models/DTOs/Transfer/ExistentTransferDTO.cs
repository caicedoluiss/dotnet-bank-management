

using BankManagement.Domain;

namespace BankManagement.Application;

public class ExistentTransferDTO : ExistentTransactionDTO
{
  new public string Type => TransactionType.Transfer.ToString();
  public int DestinationAccountId { get; set; }
  public ExistentAccountDTO? DestinationAccountInfo { get; set; }
}
