// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.DateParseHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how date formatted strings, e.g. "\/Date(1198908717056)\/" and "2012-03-21T05:40Z", are parsed when reading JSON text.
  /// </summary>
  [Preserve]
  public enum DateParseHandling
  {
    /// <summary>
    /// Date formatted strings are not parsed to a date type and are read as strings.
    /// </summary>
    None,
    /// <summary>
    /// Date formatted strings, e.g. "\/Date(1198908717056)\/" and "2012-03-21T05:40Z", are parsed to <see cref="F:Newtonsoft.Json.DateParseHandling.DateTime" />.
    /// </summary>
    DateTime,
    /// <summary>
    /// Date formatted strings, e.g. "\/Date(1198908717056)\/" and "2012-03-21T05:40Z", are parsed to <see cref="F:Newtonsoft.Json.DateParseHandling.DateTimeOffset" />.
    /// </summary>
    DateTimeOffset,
  }
}
