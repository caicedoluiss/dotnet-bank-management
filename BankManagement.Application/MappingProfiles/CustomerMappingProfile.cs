using System;
using BankManagement.Application.Utils;
using BankManagement.Domain;

namespace BankManagement.Application;

public class CustomerMappingProfile : ICustomerMappingProfile
{
  public ExistentCustomerDTO Map(Customer sourceEntity, ExistentCustomerDTO? destEntity = null)
  {
    ExistentCustomerDTO existentCustomer = destEntity is null ? new() : destEntity;

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

  public Customer Map(NewCustomerDTO sourceEntity, Customer? destEntity = null)
  {
    if (!EnumValidator.Validate(sourceEntity.Gender, out PersonGender gender)) throw new ArgumentException(nameof(sourceEntity.Gender));

    Customer customer = destEntity is null ? new() : destEntity;

    customer.IdNumber = sourceEntity.IdNumber;
    customer.Name = sourceEntity.Name;
    customer.Gender = gender;
    customer.Age = sourceEntity.Age;
    customer.PhoneNumber = sourceEntity.PhoneNumber;
    customer.Email = sourceEntity.Email;
    customer.State = sourceEntity.State;

    return customer;
  }
}
