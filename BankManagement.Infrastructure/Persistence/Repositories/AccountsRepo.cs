using System.Threading.Tasks;
using BankManagement.Application;
using BankManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Infrastructure.Persistency;

public class AccountsRepo : Repository<Account>, IAccountsRepo
{
  private readonly BankManagementDbContext context;

  public AccountsRepo(BankManagementDbContext context) : base(context)
  {
    this.context = context;
  }

  public Task<Account?> Get(int id, bool getCustomer, bool track = false)
  {
    var query = context.Accounts.AsQueryable();

    if (!track)
      query = query.AsNoTracking();

    if (getCustomer)
      query = query.Include(x => x.Customer);

    return query.FirstOrDefaultAsync(x => x.Id == id);
  }
}
