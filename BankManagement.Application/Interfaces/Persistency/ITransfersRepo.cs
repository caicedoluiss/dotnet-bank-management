using System.Threading.Tasks;
using BankManagement.Domain;

namespace BankManagement.Application;

public interface ITransfersRepo : IRepository<Transfer>
{
  Task<Transfer?> Get(int id, bool getAccountInfo, bool getAccountCustomerInfo, bool getDestinationAccountInfo, bool getDestinationAccountCustomerInfo, bool track = false);
}
