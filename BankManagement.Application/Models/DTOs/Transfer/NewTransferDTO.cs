namespace BankManagement.Application;

public class NewTransferDTO : NewTransactionDTO
{
  public int DestinationAccountId { get; set; }
}
