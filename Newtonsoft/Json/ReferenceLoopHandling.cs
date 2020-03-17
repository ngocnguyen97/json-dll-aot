// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.ReferenceLoopHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies reference loop handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public enum ReferenceLoopHandling
  {
    /// <summary>
    /// Throw a <see cref="T:Newtonsoft.Json.JsonSerializationException" /> when a loop is encountered.
    /// </summary>
    Error,
    /// <summary>Ignore loop references and do not serialize.</summary>
    Ignore,
    /// <summary>Serialize loop references.</summary>
    Serialize,
  }
}
