// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ReflectionValueProvider
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Get and set values for a <see cref="T:System.Reflection.MemberInfo" /> using reflection.
  /// </summary>
  [Preserve]
  public class ReflectionValueProvider : IValueProvider
  {
    private readonly MemberInfo _memberInfo;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.ReflectionValueProvider" /> class.
    /// </summary>
    /// <param name="memberInfo">The member info.</param>
    public ReflectionValueProvider(MemberInfo memberInfo)
    {
      ValidationUtils.ArgumentNotNull((object) memberInfo, nameof (memberInfo));
      this._memberInfo = memberInfo;
    }

    /// <summary>Sets the value.</summary>
    /// <param name="target">The target to set the value on.</param>
    /// <param name="value">The value to set on the target.</param>
    public void SetValue(object target, object value)
    {
      try
      {
        ReflectionUtils.SetMemberValue(this._memberInfo, target, value);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
      }
    }

    /// <summary>Gets the value.</summary>
    /// <param name="target">The target to get the value from.</param>
    /// <returns>The value.</returns>
    public object GetValue(object target)
    {
      try
      {
        return ReflectionUtils.GetMemberValue(this._memberInfo, target);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
      }
    }
  }
}
