namespace BankManagement.Application;

public interface IMapper<TSource, TDest>
    where TSource : class
    where TDest : class
{
  TDest Map(TSource sourceEntity, TDest? destEntity = null);
}
