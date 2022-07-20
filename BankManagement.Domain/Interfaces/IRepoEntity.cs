using System;

namespace BankManagement.Domain;

public interface IRepoEntity
{
  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
