// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.ConstructorHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how constructors are used when initializing objects during deserialization by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public enum ConstructorHandling
  {
    /// <summary>
    /// First attempt to use the public default constructor, then fall back to single paramatized constructor, then the non-public default constructor.
    /// </summary>
    Default,
    /// <summary>
    /// Json.NET will use a non-public default constructor before falling back to a paramatized constructor.
    /// </summary>
    AllowNonPublicDefaultConstructor,
  }
}
