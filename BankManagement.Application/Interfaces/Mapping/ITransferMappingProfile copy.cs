using BankManagement.Domain;

namespace BankManagement.Application;

public interface ITransferMappingProfile :
    IMapper<Transfer, ExistentTransferDTO>
    , IMapper<UpdatingTransferDTO, Transfer>
{

}
