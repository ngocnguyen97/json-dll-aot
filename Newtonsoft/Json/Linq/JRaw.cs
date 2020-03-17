// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JRaw
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Represents a raw JSON string.</summary>
  [Preserve]
  public class JRaw : JValue
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JRaw" /> class from another <see cref="T:Newtonsoft.Json.Linq.JRaw" /> object.
    /// </summary>
    /// <param name="other">A <see cref="T:Newtonsoft.Json.Linq.JRaw" /> object to copy from.</param>
    public JRaw(JRaw other)
      : base((JValue) other)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JRaw" /> class.
    /// </summary>
    /// <param name="rawJson">The raw json.</param>
    public JRaw(object rawJson)
      : base(rawJson, JTokenType.Raw)
    {
    }

    /// <summary>
    /// Creates an instance of <see cref="T:Newtonsoft.Json.Linq.JRaw" /> with the content of the reader's current token.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>An instance of <see cref="T:Newtonsoft.Json.Linq.JRaw" /> with the content of the reader's current token.</returns>
    public static JRaw Create(JsonReader reader)
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter))
        {
          jsonTextWriter.WriteToken(reader);
          return new JRaw((object) stringWriter.ToString());
        }
      }
    }

    internal override JToken CloneToken()
    {
      return (JToken) new JRaw(this);
    }
  }
}
