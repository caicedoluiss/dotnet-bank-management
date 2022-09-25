using System;
using System.Threading;
using System.Threading.Tasks;
using BankManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Infrastructure.Persistency;

public class BankManagementDbContext : DbContext
{
  public DbSet<Customer> Customers { get; set; } = null!;
  public DbSet<Account> Accounts { get; set; } = null!;
  public DbSet<Transaction> Transactions { get; set; } = null!;
  public DbSet<Transfer> Transfers { get; set; } = null!;


  public BankManagementDbContext(DbContextOptions<BankManagementDbContext> options) : base(options)
  {

  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Customer>().ToTable(nameof(Customer));
    modelBuilder.Entity<Account>().ToTable(nameof(Account));
    modelBuilder.Entity<Transaction>().ToTable(nameof(Transaction));
    modelBuilder.Entity<Transfer>().ToTable(nameof(Transfer));

    base.OnModelCreating(modelBuilder);
  }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    foreach (var entry in ChangeTracker.Entries<IRepoEntity>())
    {
      DateTime currentDate = DateTime.UtcNow;

      entry.Entity.UpdatedAt = currentDate;

      if (entry.State == EntityState.Added)
        entry.Entity.CreatedAt = currentDate;
    }

    return base.SaveChangesAsync(cancellationToken);
  }
}
