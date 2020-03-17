// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Formatting
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies formatting options for the <see cref="T:Newtonsoft.Json.JsonTextWriter" />.
  /// </summary>
  [Preserve]
  public enum Formatting
  {
    /// <summary>
    /// No special formatting is applied. This is the default.
    /// </summary>
    None,
    /// <summary>
    /// Causes child objects to be indented according to the <see cref="P:Newtonsoft.Json.JsonTextWriter.Indentation" /> and <see cref="P:Newtonsoft.Json.JsonTextWriter.IndentChar" /> settings.
    /// </summary>
    Indented,
  }
}
