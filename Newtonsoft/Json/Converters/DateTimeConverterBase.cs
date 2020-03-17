// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.DateTimeConverterBase
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Provides a base class for converting a <see cref="T:System.DateTime" /> to and from JSON.
  /// </summary>
  [Preserve]
  public abstract class DateTimeConverterBase : JsonConverter
  {
    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (DateTime) || objectType == typeof (DateTime?) || (objectType == typeof (DateTimeOffset) || objectType == typeof (DateTimeOffset?));
    }
  }
}
