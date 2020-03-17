// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ExtensionDataGetter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Gets extension data for an object during serialization.
  /// </summary>
  /// <param name="o">The object to set extension data on.</param>
  [Preserve]
  public delegate IEnumerable<KeyValuePair<object, object>> ExtensionDataGetter(
    object o);
}
