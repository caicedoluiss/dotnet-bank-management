namespace BankManagement.Domain;

public class Transfer : Transaction
{
  new TransactionType Type => TransactionType.Transfer;
  public int DestinationAccountId { get; set; }
  public Account? DestinationAccount { get; set; }
}
