namespace BankManagement.Domain;

public abstract class Person
{
  public string IdNumber { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public PersonGender Gender { get; set; }
  public int Age { get; set; }
}
