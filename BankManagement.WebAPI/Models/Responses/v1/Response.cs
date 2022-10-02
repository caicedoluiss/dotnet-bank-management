namespace BankManagement.WebAPI.Responses.v1;

public class Response<TData> : ResponseBase
{
  public TData? Data { get; set; } = default;
}
