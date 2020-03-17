// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ResolverContractKey
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Serialization
{
  [Preserve]
  internal struct ResolverContractKey
  {
    private readonly Type _resolverType;
    private readonly Type _contractType;

    public ResolverContractKey(Type resolverType, Type contractType)
    {
      this._resolverType = resolverType;
      this._contractType = contractType;
    }

    public override int GetHashCode()
    {
      return this._resolverType.GetHashCode() ^ this._contractType.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return obj is ResolverContractKey other && this.Equals(other);
    }

    public bool Equals(ResolverContractKey other)
    {
      return this._resolverType == other._resolverType && this._contractType == other._contractType;
    }
  }
}
