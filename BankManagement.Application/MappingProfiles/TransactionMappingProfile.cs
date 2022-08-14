using System;
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

  public Transaction Map(UpdatingTransactionDTO sourceEntity, Transaction? destEntity = null)
  {
    if (!DateTime.TryParseExact(sourceEntity.Date, Constants.DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime date))
    {
      throw new ArgumentException(nameof(sourceEntity.Date));
    }

    Transaction transaction = destEntity is null ? new() : destEntity;

    transaction.AccountId = sourceEntity.AccountId;
    transaction.Balance = sourceEntity.Balance;
    transaction.Date = date;
    transaction.Value = sourceEntity.Value;

    return transaction;
  }
}
