// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.MissingMemberHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies missing member handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public enum MissingMemberHandling
  {
    /// <summary>
    /// Ignore a missing member and do not attempt to deserialize it.
    /// </summary>
    Ignore,
    /// <summary>
    /// Throw a <see cref="T:Newtonsoft.Json.JsonSerializationException" /> when a missing member is encountered during deserialization.
    /// </summary>
    Error,
  }
}
