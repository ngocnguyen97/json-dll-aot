// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonLinqContract
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="T:System.Type" /> used by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public class JsonLinqContract : JsonContract
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonLinqContract" /> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    public JsonLinqContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Linq;
    }
  }
}
