// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.QuaternionConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Newtonsoft.Json.Converters
{
  public class QuaternionConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      Quaternion quaternion = (Quaternion) value;
      writer.WriteStartObject();
      writer.WritePropertyName("w");
      writer.WriteValue((float) quaternion.w);
      writer.WritePropertyName("x");
      writer.WriteValue((float) quaternion.x);
      writer.WritePropertyName("y");
      writer.WriteValue((float) quaternion.y);
      writer.WritePropertyName("z");
      writer.WriteValue((float) quaternion.z);
      writer.WritePropertyName("eulerAngles");
      writer.WriteStartObject();
      writer.WritePropertyName("x");
      writer.WriteValue((float) ((Quaternion) ref quaternion).get_eulerAngles().x);
      writer.WritePropertyName("y");
      writer.WriteValue((float) ((Quaternion) ref quaternion).get_eulerAngles().y);
      writer.WritePropertyName("z");
      writer.WriteValue((float) ((Quaternion) ref quaternion).get_eulerAngles().z);
      writer.WriteEndObject();
      writer.WriteEndObject();
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (Quaternion);
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      JObject jobject = JObject.Load(reader);
      List<JProperty> list = jobject.Properties().ToList<JProperty>();
      Quaternion quaternion = (Quaternion) null;
      if (list.Any<JProperty>((Func<JProperty, bool>) (p => p.Name == "w")))
        quaternion.w = (__Null) (double) (float) jobject["w"];
      if (list.Any<JProperty>((Func<JProperty, bool>) (p => p.Name == "x")))
        quaternion.x = (__Null) (double) (float) jobject["x"];
      if (list.Any<JProperty>((Func<JProperty, bool>) (p => p.Name == "y")))
        quaternion.y = (__Null) (double) (float) jobject["y"];
      if (list.Any<JProperty>((Func<JProperty, bool>) (p => p.Name == "z")))
        quaternion.z = (__Null) (double) (float) jobject["z"];
      if (list.Any<JProperty>((Func<JProperty, bool>) (p => p.Name == "eulerAngles")))
      {
        JToken jtoken = jobject["eulerAngles"];
        Vector3 vector3 = (Vector3) null;
        vector3.x = (__Null) (double) (float) jtoken[(object) "x"];
        vector3.y = (__Null) (double) (float) jtoken[(object) "y"];
        vector3.z = (__Null) (double) (float) jtoken[(object) "z"];
        ((Quaternion) ref quaternion).set_eulerAngles(vector3);
      }
      return (object) quaternion;
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
