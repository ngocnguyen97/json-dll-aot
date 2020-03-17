// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JTokenEqualityComparer
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Compares tokens to determine whether they are equal.</summary>
  [Preserve]
  public class JTokenEqualityComparer : IEqualityComparer<JToken>
  {
    /// <summary>Determines whether the specified objects are equal.</summary>
    /// <param name="x">The first object of type <see cref="T:Newtonsoft.Json.Linq.JToken" /> to compare.</param>
    /// <param name="y">The second object of type <see cref="T:Newtonsoft.Json.Linq.JToken" /> to compare.</param>
    /// <returns>
    /// true if the specified objects are equal; otherwise, false.
    /// </returns>
    public bool Equals(JToken x, JToken y)
    {
      return JToken.DeepEquals(x, y);
    }

    /// <summary>Returns a hash code for the specified object.</summary>
    /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
    /// <returns>A hash code for the specified object.</returns>
    /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
    public int GetHashCode(JToken obj)
    {
      return obj == null ? 0 : obj.GetDeepHashCode();
    }
  }
}
