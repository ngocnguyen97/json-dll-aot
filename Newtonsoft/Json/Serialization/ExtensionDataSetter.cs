// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ExtensionDataSetter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Sets extension data for an object during deserialization.
  /// </summary>
  /// <param name="o">The object to set extension data on.</param>
  /// <param name="key">The extension data key.</param>
  /// <param name="value">The extension data value.</param>
  [Preserve]
  public delegate void ExtensionDataSetter(object o, string key, object value);
}
