using BankManagement.Application.Utils;
using BankManagement.Domain;

namespace BankManagement.Application;

public class CustomerMappingProfile : ICustomerMappingProfile
{
  public ExistentCustomerDTO Map(Customer sourceEntity)
  {
    ExistentCustomerDTO existentCustomer = new();

    existentCustomer.IdNumber = sourceEntity.IdNumber;
    existentCustomer.Name = sourceEntity.Name;
    existentCustomer.Gender = sourceEntity.Gender.ToString();
    existentCustomer.Age = sourceEntity.Age;
    existentCustomer.PhoneNumber = sourceEntity.PhoneNumber;
    existentCustomer.Email = sourceEntity.Email;
    existentCustomer.State = sourceEntity.State;

    existentCustomer.Id = sourceEntity.Id;
    existentCustomer.CreatedAt = sourceEntity.CreatedAt.ToString(Constants.DateFormat);
    existentCustomer.UpdatedAt = sourceEntity.UpdatedAt.ToString(Constants.DateFormat);

    return existentCustomer;
  }
}
