using BankManagement.Domain;

namespace BankManagement.Application;

public interface IAccountMappingProfile :
    IMapper<Account, ExistentAccountDTO>
    , IMapper<NewAccountDTO, Account>
{

}
