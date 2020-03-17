// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IsoDateTimeConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Converts a <see cref="T:System.DateTime" /> to and from the ISO 8601 date format (e.g. 2008-04-12T12:53Z).
  /// </summary>
  [Preserve]
  public class IsoDateTimeConverter : DateTimeConverterBase
  {
    private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
    private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
    private string _dateTimeFormat;
    private CultureInfo _culture;

    /// <summary>
    /// Gets or sets the date time styles used when converting a date to and from JSON.
    /// </summary>
    /// <value>The date time styles used when converting a date to and from JSON.</value>
    public DateTimeStyles DateTimeStyles
    {
      get
      {
        return this._dateTimeStyles;
      }
      set
      {
        this._dateTimeStyles = value;
      }
    }

    /// <summary>
    /// Gets or sets the date time format used when converting a date to and from JSON.
    /// </summary>
    /// <value>The date time format used when converting a date to and from JSON.</value>
    public string DateTimeFormat
    {
      get
      {
        return this._dateTimeFormat ?? string.Empty;
      }
      set
      {
        this._dateTimeFormat = StringUtils.NullEmptyString(value);
      }
    }

    /// <summary>
    /// Gets or sets the culture used when converting a date to and from JSON.
    /// </summary>
    /// <value>The culture used when converting a date to and from JSON.</value>
    public CultureInfo Culture
    {
      get
      {
        return this._culture ?? CultureInfo.CurrentCulture;
      }
      set
      {
        this._culture = value;
      }
    }

    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      string str;
      switch (value)
      {
        case DateTime universalTime:
          if ((this._dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal || (this._dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
            universalTime = universalTime.ToUniversalTime();
          str = universalTime.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", (IFormatProvider) this.Culture);
          break;
        case DateTimeOffset universalTime:
          if ((this._dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal || (this._dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
            universalTime = universalTime.ToUniversalTime();
          str = universalTime.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", (IFormatProvider) this.Culture);
          break;
        default:
          throw new JsonSerializationException("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) ReflectionUtils.GetObjectType(value)));
      }
      writer.WriteValue(str);
    }

    /// <summary>Reads the JSON representation of the object.</summary>
    /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      bool flag = ReflectionUtils.IsNullableType(objectType);
      Type type = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullableType(objectType))
          throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        return (object) null;
      }
      if (reader.TokenType == JsonToken.Date)
        return type == typeof (DateTimeOffset) ? (!(reader.Value is DateTimeOffset) ? (object) new DateTimeOffset((DateTime) reader.Value) : reader.Value) : (reader.Value is DateTimeOffset ? (object) ((DateTimeOffset) reader.Value).DateTime : reader.Value);
      if (reader.TokenType != JsonToken.String)
        throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected String, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      string str = reader.Value.ToString();
      if (string.IsNullOrEmpty(str) & flag)
        return (object) null;
      return type == typeof (DateTimeOffset) ? (!string.IsNullOrEmpty(this._dateTimeFormat) ? (object) DateTimeOffset.ParseExact(str, this._dateTimeFormat, (IFormatProvider) this.Culture, this._dateTimeStyles) : (object) DateTimeOffset.Parse(str, (IFormatProvider) this.Culture, this._dateTimeStyles)) : (!string.IsNullOrEmpty(this._dateTimeFormat) ? (object) DateTime.ParseExact(str, this._dateTimeFormat, (IFormatProvider) this.Culture, this._dateTimeStyles) : (object) DateTime.Parse(str, (IFormatProvider) this.Culture, this._dateTimeStyles));
    }
  }
}
