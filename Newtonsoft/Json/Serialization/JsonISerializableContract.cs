// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonISerializableContract
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
  public class JsonISerializableContract : JsonContainerContract
  {
    /// <summary>Gets or sets the ISerializable object constructor.</summary>
    /// <value>The ISerializable object constructor.</value>
    public ObjectConstructor<object> ISerializableCreator { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonISerializableContract" /> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    public JsonISerializableContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Serializable;
    }
  }
}
