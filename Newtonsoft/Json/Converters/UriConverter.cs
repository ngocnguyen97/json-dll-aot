// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.UriConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Converters
{
  public class UriConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (Uri);
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      switch (reader.TokenType)
      {
        case JsonToken.String:
          return (object) new Uri((string) reader.Value);
        case JsonToken.Null:
          return (object) null;
        default:
          throw new InvalidOperationException("Unhandled case for UriConverter. Check to see if this converter has been applied to the wrong serialization type.");
      }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        Uri uri = value as Uri;
        if (uri == (Uri) null)
          throw new InvalidOperationException("Unhandled case for UriConverter. Check to see if this converter has been applied to the wrong serialization type.");
        writer.WriteValue(uri.OriginalString);
      }
    }
  }
}
