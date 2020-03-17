// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.IValueProvider
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>Provides methods to get and set values.</summary>
  [Preserve]
  public interface IValueProvider
  {
    /// <summary>Sets the value.</summary>
    /// <param name="target">The target to set the value on.</param>
    /// <param name="value">The value to set on the target.</param>
    void SetValue(object target, object value);

    /// <summary>Gets the value.</summary>
    /// <param name="target">The target to get the value from.</param>
    /// <returns>The value.</returns>
    object GetValue(object target);
  }
}
