using BankManagement.Application;
using BankManagement.Domain;

namespace BankManagement.Infrastructure.Persistency;

public class CustomersRepo : Repository<Customer>, ICustomersRepo
{
  public CustomersRepo(BankManagementDbContext context) : base(context)
  {

  }
}
