using System;
using BankManagement.Application.Utils;
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

  public Account Map(NewAccountDTO sourceEntity, Account? destEntity = null)
  {
    if (!EnumValidator.Validate(sourceEntity.Type, out AccountType type)) throw new ArgumentException(nameof(sourceEntity.Type));

    Account account = destEntity is null ? new() : destEntity;

    account.Number = sourceEntity.Number;
    account.Type = type;
    account.Balance = sourceEntity.Balance;
    account.CustomerId = sourceEntity.CustomerId;
    account.State = sourceEntity.State;

    return account;
  }
}
