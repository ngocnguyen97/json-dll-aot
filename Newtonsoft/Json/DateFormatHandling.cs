// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.DateFormatHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how dates are formatted when writing JSON text.
  /// </summary>
  [Preserve]
  public enum DateFormatHandling
  {
    /// <summary>
    /// Dates are written in the ISO 8601 format, e.g. "2012-03-21T05:40Z".
    /// </summary>
    IsoDateFormat,
    /// <summary>
    /// Dates are written in the Microsoft JSON format, e.g. "\/Date(1198908717056)\/".
    /// </summary>
    MicrosoftDateFormat,
  }
}
