// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonToken
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Bson
{
  [Preserve]
  internal abstract class BsonToken
  {
    public abstract BsonType Type { get; }

    public BsonToken Parent { get; set; }

    public int CalculatedSize { get; set; }
  }
}
