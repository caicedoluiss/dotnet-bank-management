using BankManagement.Domain;

namespace BankManagement.Application;

public class AccountMappingProfile : IAccountMappingProfile
{
  private readonly ICustomerMappingProfile customerMappingProfile;

  public AccountMappingProfile(ICustomerMappingProfile customerMappingProfile)
  {
    this.customerMappingProfile = customerMappingProfile;
  }

  public ExistentAccountDTO Map(Account sourceEntity, ExistentAccountDTO? destEntity = null)
  {
    ExistentAccountDTO existentAccount = destEntity is null ? new() : destEntity;

    existentAccount.Number = sourceEntity.Number;
    existentAccount.Type = sourceEntity.Type.ToString();
    existentAccount.Balance = sourceEntity.Balance;
    existentAccount.CustomerId = sourceEntity.CustomerId;
    existentAccount.State = sourceEntity.State;
    existentAccount.Customer = sourceEntity.Customer is null ? null : customerMappingProfile.Map(sourceEntity.Customer);

    existentAccount.Id = sourceEntity.Id;
    existentAccount.CreatedAt = sourceEntity.CreatedAt;
    existentAccount.UpdatedAt = sourceEntity.UpdatedAt;

    return existentAccount;
  }
}
