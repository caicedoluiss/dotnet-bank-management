using System.Threading.Tasks;
using BankManagement.Domain;

namespace BankManagement.Application;

public interface IAccountsRepo : IRepository<Account>
{
  Task<Account?> Get(int id, bool getCustomer, bool track = false);
}
