using BankManagement.Application.Utils;
using BankManagement.Domain;

namespace BankManagement.Application;

public class TransactionMappingProfile : ITransactionMappingProfile
{
  private readonly IAccountMappingProfile accountMapperProfile;

  public TransactionMappingProfile(IAccountMappingProfile accountMapperProfile)
  {
    this.accountMapperProfile = accountMapperProfile;
  }

  public ExistentTransactionDTO Map(Transaction sourceEntity, ExistentTransactionDTO? destEntity = null)
  {
    ExistentTransactionDTO existentTransaction = destEntity is null ? new() : destEntity;

    existentTransaction.AccountId = sourceEntity.AccountId;
    existentTransaction.Account = sourceEntity.Account is null ? null : accountMapperProfile.Map(sourceEntity.Account);
    existentTransaction.Value = sourceEntity.Value;
    existentTransaction.Balance = sourceEntity.Balance;
    existentTransaction.Date = sourceEntity.Date.ToString(Constants.DateFormat);

    existentTransaction.Id = sourceEntity.Id;
    existentTransaction.CreatedAt = sourceEntity.CreatedAt.ToString(Constants.DateFormat);
    existentTransaction.UpdatedAt = sourceEntity.UpdatedAt.ToString(Constants.DateFormat);

    return existentTransaction;
  }
}
