using System.Threading.Tasks;

namespace BankManagement.Application;

public interface IUnitOfWork
{
  ICustomersRepo CustomersRepo { get; }
  Task<int> Complete();
}
