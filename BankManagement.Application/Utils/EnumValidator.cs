using System;

namespace BankManagement.Application.Utils;

public static class EnumValidator
{
  public static bool Validate<TEnum>(string? stringValue, out TEnum result)
      where TEnum : struct, Enum
  {
    if (stringValue is null)
    {
      result = default;
      return true;
    }

    return Enum.TryParse<TEnum>(stringValue, out result) && Enum.IsDefined<TEnum>(result);
  }
}
