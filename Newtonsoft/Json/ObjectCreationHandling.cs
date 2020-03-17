// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.ObjectCreationHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how object creation is handled by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public enum ObjectCreationHandling
  {
    /// <summary>
    /// Reuse existing objects, create new objects when needed.
    /// </summary>
    Auto,
    /// <summary>Only reuse existing objects.</summary>
    Reuse,
    /// <summary>Always create new objects.</summary>
    Replace,
  }
}
