using BankManagement.Domain;

namespace BankManagement.Application;

public class TransferMappingProfile : ITransferMappingProfile
{
  private readonly ITransactionMappingProfile transactionMappingProfile;
  private readonly IAccountMappingProfile accountMappingProfile;

  public TransferMappingProfile(ITransactionMappingProfile transactionMappingProfile, IAccountMappingProfile accountMappingProfile)
  {
    this.transactionMappingProfile = transactionMappingProfile;
    this.accountMappingProfile = accountMappingProfile;
  }

  public ExistentTransferDTO Map(Transfer sourceEntity, ExistentTransferDTO? destEntity = null)
  {
    ExistentTransferDTO existentTransfer = destEntity is null ? new() : destEntity;

    _ = transactionMappingProfile.Map(sourceEntity, existentTransfer);

    existentTransfer.DestinationAccountId = sourceEntity.DestinationAccountId;
    existentTransfer.DestinationAccountInfo = sourceEntity.DestinationAccount is null ? null : accountMappingProfile.Map(sourceEntity.DestinationAccount);

    return existentTransfer;
  }
}
