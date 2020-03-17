// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.MetadataPropertyHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies metadata property handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public enum MetadataPropertyHandling
  {
    /// <summary>
    /// Read metadata properties located at the start of a JSON object.
    /// </summary>
    Default,
    /// <summary>
    /// Read metadata properties located anywhere in a JSON object. Note that this setting will impact performance.
    /// </summary>
    ReadAhead,
    /// <summary>Do not try to read metadata properties.</summary>
    Ignore,
  }
}
