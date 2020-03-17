// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonValue
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Bson
{
  [Preserve]
  internal class BsonValue : BsonToken
  {
    private readonly object _value;
    private readonly BsonType _type;

    public BsonValue(object value, BsonType type)
    {
      this._value = value;
      this._type = type;
    }

    public object Value
    {
      get
      {
        return this._value;
      }
    }

    public override BsonType Type
    {
      get
      {
        return this._type;
      }
    }
  }
}
