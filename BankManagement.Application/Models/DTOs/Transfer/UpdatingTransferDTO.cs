namespace BankManagement.Application;

public class UpdatingTransferDTO : UpdatingTransactionDTO
{
  public int DestinationAccountId { get; set; }
}
