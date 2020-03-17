// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Required
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>Indicating whether a property is required.</summary>
  [Preserve]
  public enum Required
  {
    /// <summary>The property is not required. The default state.</summary>
    Default,
    /// <summary>
    /// The property must be defined in JSON but can be a null value.
    /// </summary>
    AllowNull,
    /// <summary>
    /// The property must be defined in JSON and cannot be a null value.
    /// </summary>
    Always,
    /// <summary>
    /// The property is not required but it cannot be a null value.
    /// </summary>
    DisallowNull,
  }
}
