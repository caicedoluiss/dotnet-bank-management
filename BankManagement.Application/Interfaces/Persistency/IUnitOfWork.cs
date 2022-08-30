using System.Threading.Tasks;

namespace BankManagement.Application;

public interface IUnitOfWork
{
  ICustomersRepo CustomersRepo { get; }
  IAccountsRepo AccountsRepo { get; }
  ITransactionsRepo TransactionsRepo { get; }
  ITransfersRepo TransfersRepo { get; }
  Task<int> Complete();
}
