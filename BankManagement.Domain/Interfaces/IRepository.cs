using System.Threading.Tasks;

namespace BankManagement.Domain;

public interface IRepository<TEntity>
    where TEntity : class, IRepoEntity, new()
{
  Task<bool> Exist(int id);
  TEntity Add(TEntity entity);
  Task<TEntity?> Get(int id, bool track = false);
  void Update(TEntity entity);
  void Delete(TEntity entity);
}
