// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.StringEscapeHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how strings are escaped when writing JSON text.
  /// </summary>
  [Preserve]
  public enum StringEscapeHandling
  {
    /// <summary>Only control characters (e.g. newline) are escaped.</summary>
    Default,
    /// <summary>
    /// All non-ASCII and control characters (e.g. newline) are escaped.
    /// </summary>
    EscapeNonAscii,
    /// <summary>
    /// HTML (&lt;, &gt;, &amp;, ', ") and control characters (e.g. newline) are escaped.
    /// </summary>
    EscapeHtml,
  }
}
