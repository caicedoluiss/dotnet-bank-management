namespace BankManagement.WebAPI.Responses.v1;

public class ResponseBase
{
  public bool Error { get; set; }
  public string Message { get; set; } = string.Empty;
}
