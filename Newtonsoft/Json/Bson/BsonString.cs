﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonString
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Bson
{
  [Preserve]
  internal class BsonString : BsonValue
  {
    public int ByteCount { get; set; }

    public bool IncludeLength { get; set; }

    public BsonString(object value, bool includeLength)
      : base(value, BsonType.String)
    {
      this.IncludeLength = includeLength;
    }
  }
}
