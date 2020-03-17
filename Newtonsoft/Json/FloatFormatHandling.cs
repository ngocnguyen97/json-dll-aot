// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.FloatFormatHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies float format handling options when writing special floating point numbers, e.g. <see cref="F:System.Double.NaN" />,
  /// <see cref="F:System.Double.PositiveInfinity" /> and <see cref="F:System.Double.NegativeInfinity" /> with <see cref="T:Newtonsoft.Json.JsonWriter" />.
  /// </summary>
  [Preserve]
  public enum FloatFormatHandling
  {
    /// <summary>
    /// Write special floating point values as strings in JSON, e.g. "NaN", "Infinity", "-Infinity".
    /// </summary>
    String,
    /// <summary>
    /// Write special floating point values as symbols in JSON, e.g. NaN, Infinity, -Infinity.
    /// Note that this will produce non-valid JSON.
    /// </summary>
    Symbol,
    /// <summary>
    /// Write special floating point values as the property's default value in JSON, e.g. 0.0 for a <see cref="T:System.Double" /> property, null for a <see cref="T:System.Nullable`1" /> property.
    /// </summary>
    DefaultValue,
  }
}
