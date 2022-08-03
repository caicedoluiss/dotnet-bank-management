using BankManagement.Domain;

namespace BankManagement.Application;

public interface ICustomerMappingProfile :
    IMapper<Customer, ExistentCustomerDTO>
    , IMapper<NewCustomerDTO, Customer>
{

}
