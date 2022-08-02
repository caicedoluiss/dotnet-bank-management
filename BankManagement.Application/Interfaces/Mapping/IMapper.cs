namespace BankManagement.Application;

public interface IMapper<TSource, TDest>
{
  TDest Map(TSource sourceEntity);
}
