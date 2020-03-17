// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.BsonObjectIdConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Converts a <see cref="T:Newtonsoft.Json.Bson.BsonObjectId" /> to and from JSON and BSON.
  /// </summary>
  [Preserve]
  public class BsonObjectIdConverter : JsonConverter
  {
    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      BsonObjectId bsonObjectId = (BsonObjectId) value;
      if (writer is BsonWriter bsonWriter)
        bsonWriter.WriteObjectId(bsonObjectId.Value);
      else
        writer.WriteValue(bsonObjectId.Value);
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
      if (reader.TokenType != JsonToken.Bytes)
        throw new JsonSerializationException("Expected Bytes but got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      return (object) new BsonObjectId((byte[]) reader.Value);
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
      return objectType == typeof (BsonObjectId);
    }
  }
}
