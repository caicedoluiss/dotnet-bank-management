using System.Linq;
using System.Threading.Tasks;
using BankManagement.Application;
using BankManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Infrastructure.Persistency;

public class TransactionsRepo : Repository<Transaction>, ITransactionsRepo
{
  private readonly BankManagementDbContext context;

  public TransactionsRepo(BankManagementDbContext context) : base(context)
  {
    this.context = context;
  }

  public Task<Transaction?> Get(int id, bool getAccountInfo, bool getAccountCustomerInfo, bool track = false)
  {
    var query = context.Transactions.AsQueryable();

    if (!track)
      query = query.AsNoTracking();

    if (getAccountCustomerInfo)
      query = query.Include(x => x.Account).ThenInclude(x => x!.Customer);
    else
      query = query.Include(x => x.Account);

    return query.FirstOrDefaultAsync(x => x.Id == id);
  }
}
