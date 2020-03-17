// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.IAttributeProvider
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>Provides methods to get attributes.</summary>
  [Preserve]
  public interface IAttributeProvider
  {
    /// <summary>
    /// Returns a collection of all of the attributes, or an empty collection if there are no attributes.
    /// </summary>
    /// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <returns>A collection of <see cref="T:System.Attribute" />s, or an empty collection.</returns>
    IList<Attribute> GetAttributes(bool inherit);

    /// <summary>
    /// Returns a collection of attributes, identified by type, or an empty collection if there are no attributes.
    /// </summary>
    /// <param name="attributeType">The type of the attributes.</param>
    /// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <returns>A collection of <see cref="T:System.Attribute" />s, or an empty collection.</returns>
    IList<Attribute> GetAttributes(Type attributeType, bool inherit);
  }
}
