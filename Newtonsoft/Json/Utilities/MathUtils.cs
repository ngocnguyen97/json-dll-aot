// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.MathUtils
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal static class MathUtils
  {
    public static int IntLength(ulong i)
    {
      if (i < 10000000000UL)
      {
        if (i < 10UL)
          return 1;
        if (i < 100UL)
          return 2;
        if (i < 1000UL)
          return 3;
        if (i < 10000UL)
          return 4;
        if (i < 100000UL)
          return 5;
        if (i < 1000000UL)
          return 6;
        if (i < 10000000UL)
          return 7;
        if (i < 100000000UL)
          return 8;
        return i < 1000000000UL ? 9 : 10;
      }
      if (i < 100000000000UL)
        return 11;
      if (i < 1000000000000UL)
        return 12;
      if (i < 10000000000000UL)
        return 13;
      if (i < 100000000000000UL)
        return 14;
      if (i < 1000000000000000UL)
        return 15;
      if (i < 10000000000000000UL)
        return 16;
      if (i < 100000000000000000UL)
        return 17;
      if (i < 1000000000000000000UL)
        return 18;
      return i < 10000000000000000000UL ? 19 : 20;
    }

    public static char IntToHex(int n)
    {
      return n <= 9 ? (char) (n + 48) : (char) (n - 10 + 97);
    }

    public static int? Min(int? val1, int? val2)
    {
      if (!val1.HasValue)
        return val2;
      return !val2.HasValue ? val1 : new int?(Math.Min(val1.GetValueOrDefault(), val2.GetValueOrDefault()));
    }

    public static int? Max(int? val1, int? val2)
    {
      if (!val1.HasValue)
        return val2;
      return !val2.HasValue ? val1 : new int?(Math.Max(val1.GetValueOrDefault(), val2.GetValueOrDefault()));
    }

    public static double? Max(double? val1, double? val2)
    {
      if (!val1.HasValue)
        return val2;
      return !val2.HasValue ? val1 : new double?(Math.Max(val1.GetValueOrDefault(), val2.GetValueOrDefault()));
    }

    public static bool ApproxEquals(double d1, double d2)
    {
      if (d1 == d2)
        return true;
      double num1 = (Math.Abs(d1) + Math.Abs(d2) + 10.0) * 2.22044604925031E-16;
      double num2 = d1 - d2;
      return -num1 < num2 && num1 > num2;
    }
  }
}
