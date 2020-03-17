// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonArray
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
  [Preserve]
  internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
  {
    private readonly List<BsonToken> _children = new List<BsonToken>();

    public void Add(BsonToken token)
    {
      this._children.Add(token);
      token.Parent = (BsonToken) this;
    }

    public override BsonType Type
    {
      get
      {
        return BsonType.Array;
      }
    }

    public IEnumerator<BsonToken> GetEnumerator()
    {
      return (IEnumerator<BsonToken>) this._children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
