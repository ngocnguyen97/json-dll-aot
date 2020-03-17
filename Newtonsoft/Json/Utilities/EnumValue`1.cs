// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumValue`1
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal class EnumValue<T> where T : struct
  {
    private readonly string _name;
    private readonly T _value;

    public string Name
    {
      get
      {
        return this._name;
      }
    }

    public T Value
    {
      get
      {
        return this._value;
      }
    }

    public EnumValue(string name, T value)
    {
      this._name = name;
      this._value = value;
    }
  }
}
