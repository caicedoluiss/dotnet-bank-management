using System.Threading.Tasks;
using BankManagement.Domain;

namespace BankManagement.Application;

public interface ITransactionsRepo : IRepository<Transaction>
{
  Task<Transaction?> Get(int id, bool getAccountInfo, bool getAccountCustomerInfo, bool track = false);
}
