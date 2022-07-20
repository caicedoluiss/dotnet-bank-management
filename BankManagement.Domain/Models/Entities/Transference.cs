namespace BankManagement.Domain;

public class Transference : Transaction
{
  new TransactionType Type => TransactionType.Transference;
  public int DestinationAccountId { get; set; }
  public Account? DestinationAccount { get; set; }
}
