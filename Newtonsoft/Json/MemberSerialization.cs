// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.MemberSerialization
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies the member serialization options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public enum MemberSerialization
  {
    /// <summary>
    /// All public members are serialized by default. Members can be excluded using <see cref="T:Newtonsoft.Json.JsonIgnoreAttribute" /> or <see cref="T:System.NonSerializedAttribute" />.
    /// This is the default member serialization mode.
    /// </summary>
    OptOut,
    /// <summary>
    /// Only members marked with <see cref="T:Newtonsoft.Json.JsonPropertyAttribute" /> or <see cref="T:System.Runtime.Serialization.DataMemberAttribute" /> are serialized.
    /// This member serialization mode can also be set by marking the class with <see cref="T:System.Runtime.Serialization.DataContractAttribute" />.
    /// </summary>
    OptIn,
    /// <summary>
    /// All public and private fields are serialized. Members can be excluded using <see cref="T:Newtonsoft.Json.JsonIgnoreAttribute" /> or <see cref="T:System.NonSerializedAttribute" />.
    /// This member serialization mode can also be set by marking the class with <see cref="T:System.SerializableAttribute" />
    /// and setting IgnoreSerializableAttribute on <see cref="T:Newtonsoft.Json.Serialization.DefaultContractResolver" /> to false.
    /// </summary>
    Fields,
  }
}
