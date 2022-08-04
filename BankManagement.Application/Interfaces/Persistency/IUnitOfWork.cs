using System.Threading.Tasks;

namespace BankManagement.Application;

public interface IUnitOfWork
{
  ICustomersRepo CustomersRepo { get; }
  IAccountsRepo AccountsRepo { get; }
  Task<int> Complete();
}
