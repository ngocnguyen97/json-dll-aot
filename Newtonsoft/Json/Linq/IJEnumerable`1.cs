// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.IJEnumerable`1
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
  /// <summary>
  /// Represents a collection of <see cref="T:Newtonsoft.Json.Linq.JToken" /> objects.
  /// </summary>
  /// <typeparam name="T">The type of token</typeparam>
  [Preserve]
  public interface IJEnumerable<T> : IEnumerable<T>, IEnumerable where T : JToken
  {
    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Linq.IJEnumerable`1" /> with the specified key.
    /// </summary>
    /// <value></value>
    IJEnumerable<JToken> this[object key] { get; }
  }
}
