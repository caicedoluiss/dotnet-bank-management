using System.Threading.Tasks;
using BankManagement.Application;
using BankManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Infrastructure.Persistency;

public class TransfersRepo : Repository<Transfer>, ITransfersRepo
{
  private readonly BankManagementDbContext context;

  public TransfersRepo(BankManagementDbContext context) : base(context)
  {
    this.context = context;
  }

  public Task<Transfer?> Get(int id, bool getAccountInfo, bool getAccountCustomerInfo, bool getDestinationAccountInfo, bool getDestinationAccountCustomerInfo, bool track = false)
  {
    var query = context.Transfers.AsQueryable();

    if (!track)
      query = query.AsNoTracking();

    if (getAccountCustomerInfo)
      query = query.Include(x => x.Account).ThenInclude(x => x!.Customer);
    else
      query = query.Include(x => x.Account);

    if (getDestinationAccountCustomerInfo)
      query = query.Include(x => x.DestinationAccount).ThenInclude(x => x!.Customer);
    else
      query = query.Include(x => x.DestinationAccount);

    return query.FirstOrDefaultAsync(x => x.Id == id);
  }
}
