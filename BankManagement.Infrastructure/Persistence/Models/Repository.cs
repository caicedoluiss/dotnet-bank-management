using System.Threading.Tasks;
using BankManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Infrastructure.Persistency;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IRepoEntity, new()
{
  private readonly DbContext context;

  public Repository(DbContext context)
  {
    this.context = context;
  }

  public TEntity Add(TEntity entity)
  {
    return context.Set<TEntity>().Add(entity).Entity;
  }

  public void Delete(TEntity entity)
  {
    context.Set<TEntity>().Remove(entity);
  }

  public Task<bool> Exist(int id)
  {
    return context.Set<TEntity>().AnyAsync();
  }

  public Task<TEntity?> Get(int id, bool track = false)
  {
    var query = context.Set<TEntity>().AsQueryable();

    if (!track)
      query = query.AsNoTracking();

    return query.FirstOrDefaultAsync(x => x.Id == id);
  }

  public void Update(TEntity entity)
  {
    context.Entry<TEntity>(entity).State = EntityState.Modified;
    context.Entry<TEntity>(entity).Property(x => x.Id).IsModified = false;
    context.Entry<TEntity>(entity).Property(x => x.CreatedAt).IsModified = false;
  }
}
