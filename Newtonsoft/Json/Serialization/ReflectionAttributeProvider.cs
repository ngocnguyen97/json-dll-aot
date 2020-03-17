// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ReflectionAttributeProvider
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Provides methods to get attributes from a <see cref="T:System.Type" />, <see cref="T:System.Reflection.MemberInfo" />, <see cref="T:System.Reflection.ParameterInfo" /> or <see cref="T:System.Reflection.Assembly" />.
  /// </summary>
  [Preserve]
  public class ReflectionAttributeProvider : IAttributeProvider
  {
    private readonly object _attributeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.ReflectionAttributeProvider" /> class.
    /// </summary>
    /// <param name="attributeProvider">The instance to get attributes for. This parameter should be a <see cref="T:System.Type" />, <see cref="T:System.Reflection.MemberInfo" />, <see cref="T:System.Reflection.ParameterInfo" /> or <see cref="T:System.Reflection.Assembly" />.</param>
    public ReflectionAttributeProvider(object attributeProvider)
    {
      ValidationUtils.ArgumentNotNull(attributeProvider, nameof (attributeProvider));
      this._attributeProvider = attributeProvider;
    }

    /// <summary>
    /// Returns a collection of all of the attributes, or an empty collection if there are no attributes.
    /// </summary>
    /// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <returns>A collection of <see cref="T:System.Attribute" />s, or an empty collection.</returns>
    public IList<Attribute> GetAttributes(bool inherit)
    {
      return (IList<Attribute>) ReflectionUtils.GetAttributes(this._attributeProvider, (Type) null, inherit);
    }

    /// <summary>
    /// Returns a collection of attributes, identified by type, or an empty collection if there are no attributes.
    /// </summary>
    /// <param name="attributeType">The type of the attributes.</param>
    /// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <returns>A collection of <see cref="T:System.Attribute" />s, or an empty collection.</returns>
    public IList<Attribute> GetAttributes(Type attributeType, bool inherit)
    {
      return (IList<Attribute>) ReflectionUtils.GetAttributes(this._attributeProvider, attributeType, inherit);
    }
  }
}
