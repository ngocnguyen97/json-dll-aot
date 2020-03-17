// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.ColorConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Newtonsoft.Json.Converters
{
  public class ColorConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        Color color = (Color) value;
        writer.WriteStartObject();
        writer.WritePropertyName("a");
        writer.WriteValue((float) color.a);
        writer.WritePropertyName("r");
        writer.WriteValue((float) color.r);
        writer.WritePropertyName("g");
        writer.WriteValue((float) color.g);
        writer.WritePropertyName("b");
        writer.WriteValue((float) color.b);
        writer.WriteEndObject();
      }
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (Color) || objectType == typeof (Color32);
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) (Color) null;
      JObject jobject = JObject.Load(reader);
      return objectType == typeof (Color32) ? (object) new Color32((byte) jobject["r"], (byte) jobject["g"], (byte) jobject["b"], (byte) jobject["a"]) : (object) new Color((float) jobject["r"], (float) jobject["g"], (float) jobject["b"], (float) jobject["a"]);
    }

    public override bool CanRead
    {
      get
      {
        return true;
      }
    }
  }
}
