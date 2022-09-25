using System.Threading.Tasks;
using BankManagement.Application;

namespace BankManagement.Infrastructure.Persistency;

public class UnitOfWork : IUnitOfWork
{
  private readonly BankManagementDbContext context;

  public ICustomersRepo CustomersRepo { get; private set; }
  public IAccountsRepo AccountsRepo { get; private set; }
  public ITransactionsRepo TransactionsRepo { get; private set; }
  public ITransfersRepo TransfersRepo { get; private set; }

  public UnitOfWork(BankManagementDbContext context, ICustomersRepo customersRepo, IAccountsRepo accountsRepo, ITransactionsRepo transactionsRepo, ITransfersRepo transfersRepo)
  {
    this.context = context;
    CustomersRepo = customersRepo;
    AccountsRepo = accountsRepo;
    TransactionsRepo = transactionsRepo;
    TransfersRepo = transfersRepo;
  }

  public Task<int> Complete()
  {
    return context.SaveChangesAsync();
  }
}
