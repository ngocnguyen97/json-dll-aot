// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonRegex
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Bson
{
  [Preserve]
  internal class BsonRegex : BsonToken
  {
    public BsonString Pattern { get; set; }

    public BsonString Options { get; set; }

    public BsonRegex(string pattern, string options)
    {
      this.Pattern = new BsonString((object) pattern, false);
      this.Options = new BsonString((object) options, false);
    }

    public override BsonType Type
    {
      get
      {
        return BsonType.Regex;
      }
    }
  }
}
