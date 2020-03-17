// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.ResolutionConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Newtonsoft.Json.Converters
{
  public class ResolutionConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      Resolution resolution = (Resolution) value;
      writer.WriteStartObject();
      writer.WritePropertyName("height");
      writer.WriteValue(((Resolution) ref resolution).get_height());
      writer.WritePropertyName("width");
      writer.WriteValue(((Resolution) ref resolution).get_width());
      writer.WritePropertyName("refreshRate");
      writer.WriteValue(((Resolution) ref resolution).get_refreshRate());
      writer.WriteEndObject();
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (Resolution);
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      JObject jobject = JObject.Load(reader);
      Resolution resolution = (Resolution) null;
      ((Resolution) ref resolution).set_height((int) jobject["height"]);
      ((Resolution) ref resolution).set_width((int) jobject["width"]);
      ((Resolution) ref resolution).set_refreshRate((int) jobject["refreshRate"]);
      return (object) resolution;
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
