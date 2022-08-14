using BankManagement.Domain;

namespace BankManagement.Application;

public interface ITransactionMappingProfile :
    IMapper<Transaction, ExistentTransactionDTO>
    , IMapper<UpdatingTransactionDTO, Transaction>
{

}
