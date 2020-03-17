// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.Matrix4x4Converter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Newtonsoft.Json.Converters
{
  public class Matrix4x4Converter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        Matrix4x4 matrix4x4 = (Matrix4x4) value;
        writer.WriteStartObject();
        writer.WritePropertyName("m00");
        writer.WriteValue((float) matrix4x4.m00);
        writer.WritePropertyName("m01");
        writer.WriteValue((float) matrix4x4.m01);
        writer.WritePropertyName("m02");
        writer.WriteValue((float) matrix4x4.m02);
        writer.WritePropertyName("m03");
        writer.WriteValue((float) matrix4x4.m03);
        writer.WritePropertyName("m10");
        writer.WriteValue((float) matrix4x4.m10);
        writer.WritePropertyName("m11");
        writer.WriteValue((float) matrix4x4.m11);
        writer.WritePropertyName("m12");
        writer.WriteValue((float) matrix4x4.m12);
        writer.WritePropertyName("m13");
        writer.WriteValue((float) matrix4x4.m13);
        writer.WritePropertyName("m20");
        writer.WriteValue((float) matrix4x4.m20);
        writer.WritePropertyName("m21");
        writer.WriteValue((float) matrix4x4.m21);
        writer.WritePropertyName("m22");
        writer.WriteValue((float) matrix4x4.m22);
        writer.WritePropertyName("m23");
        writer.WriteValue((float) matrix4x4.m23);
        writer.WritePropertyName("m30");
        writer.WriteValue((float) matrix4x4.m30);
        writer.WritePropertyName("m31");
        writer.WriteValue((float) matrix4x4.m31);
        writer.WritePropertyName("m32");
        writer.WriteValue((float) matrix4x4.m32);
        writer.WritePropertyName("m33");
        writer.WriteValue((float) matrix4x4.m33);
        writer.WriteEnd();
      }
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) (Matrix4x4) null;
      JObject jobject = JObject.Load(reader);
      Matrix4x4 matrix4x4 = (Matrix4x4) null;
      matrix4x4.m00 = (__Null) (double) (float) jobject["m00"];
      matrix4x4.m01 = (__Null) (double) (float) jobject["m01"];
      matrix4x4.m02 = (__Null) (double) (float) jobject["m02"];
      matrix4x4.m03 = (__Null) (double) (float) jobject["m03"];
      matrix4x4.m20 = (__Null) (double) (float) jobject["m20"];
      matrix4x4.m21 = (__Null) (double) (float) jobject["m21"];
      matrix4x4.m22 = (__Null) (double) (float) jobject["m22"];
      matrix4x4.m23 = (__Null) (double) (float) jobject["m23"];
      matrix4x4.m30 = (__Null) (double) (float) jobject["m30"];
      matrix4x4.m31 = (__Null) (double) (float) jobject["m31"];
      matrix4x4.m32 = (__Null) (double) (float) jobject["m32"];
      matrix4x4.m33 = (__Null) (double) (float) jobject["m33"];
      return (object) matrix4x4;
    }

    public override bool CanRead
    {
      get
      {
        return true;
      }
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (Matrix4x4);
    }
  }
}
