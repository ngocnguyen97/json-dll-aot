// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.CommentHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Linq
{
  /// <summary>
  /// Specifies how JSON comments are handled when loading JSON.
  /// </summary>
  [Preserve]
  public enum CommentHandling
  {
    /// <summary>Ignore comments.</summary>
    Ignore,
    /// <summary>
    /// Load comments as a <see cref="T:Newtonsoft.Json.Linq.JValue" /> with type <see cref="F:Newtonsoft.Json.Linq.JTokenType.Comment" />.
    /// </summary>
    Load,
  }
}
