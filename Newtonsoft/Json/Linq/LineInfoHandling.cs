// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.LineInfoHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Linq
{
  /// <summary>
  /// Specifies how line information is handled when loading JSON.
  /// </summary>
  [Preserve]
  public enum LineInfoHandling
  {
    /// <summary>Ignore line information.</summary>
    Ignore,
    /// <summary>Load line information.</summary>
    Load,
  }
}
