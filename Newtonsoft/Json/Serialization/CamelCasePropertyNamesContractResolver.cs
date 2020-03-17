// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Resolves member mappings for a type, camel casing property names.
  /// </summary>
  [Preserve]
  public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver" /> class.
    /// </summary>
    public CamelCasePropertyNamesContractResolver()
      : base(true)
    {
    }

    /// <summary>Resolves the name of the property.</summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>The property name camel cased.</returns>
    protected override string ResolvePropertyName(string propertyName)
    {
      return StringUtils.ToCamelCase(propertyName);
    }
  }
}
