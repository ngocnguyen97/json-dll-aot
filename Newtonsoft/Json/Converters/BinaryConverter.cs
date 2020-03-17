// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.BinaryConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Converts a binary value to and from a base 64 string value.
  /// </summary>
  [Preserve]
  public class BinaryConverter : JsonConverter
  {
    private const string BinaryTypeName = "System.Data.Linq.Binary";
    private const string BinaryToArrayName = "ToArray";
    private ReflectionObject _reflectionObject;

    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        byte[] byteArray = this.GetByteArray(value);
        writer.WriteValue(byteArray);
      }
    }

    private byte[] GetByteArray(object value)
    {
      if (!value.GetType().AssignableToTypeName("System.Data.Linq.Binary"))
        throw new JsonSerializationException("Unexpected value type when writing binary: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
      this.EnsureReflectionObject(value.GetType());
      return (byte[]) this._reflectionObject.GetValue(value, "ToArray");
    }

    private void EnsureReflectionObject(Type t)
    {
      if (this._reflectionObject != null)
        return;
      this._reflectionObject = ReflectionObject.Create(t, (MethodBase) t.GetConstructor(new Type[1]
      {
        typeof (byte[])
      }), "ToArray");
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
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullable(objectType))
          throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        return (object) null;
      }
      byte[] numArray;
      if (reader.TokenType == JsonToken.StartArray)
      {
        numArray = this.ReadByteArray(reader);
      }
      else
      {
        if (reader.TokenType != JsonToken.String)
          throw JsonSerializationException.Create(reader, "Unexpected token parsing binary. Expected String or StartArray, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
        numArray = Convert.FromBase64String(reader.Value.ToString());
      }
      Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      if (!type.AssignableToTypeName("System.Data.Linq.Binary"))
        throw JsonSerializationException.Create(reader, "Unexpected object type when writing binary: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
      this.EnsureReflectionObject(type);
      return this._reflectionObject.Creator(new object[1]
      {
        (object) numArray
      });
    }

    private byte[] ReadByteArray(JsonReader reader)
    {
      List<byte> byteList = new List<byte>();
      while (reader.Read())
      {
        switch (reader.TokenType)
        {
          case JsonToken.Comment:
            continue;
          case JsonToken.Integer:
            byteList.Add(Convert.ToByte(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            continue;
          case JsonToken.EndArray:
            return byteList.ToArray();
          default:
            throw JsonSerializationException.Create(reader, "Unexpected token when reading bytes: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
        }
      }
      throw JsonSerializationException.Create(reader, "Unexpected end when reading bytes.");
    }

    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type objectType)
    {
      return objectType.AssignableToTypeName("System.Data.Linq.Binary");
    }
  }
}
