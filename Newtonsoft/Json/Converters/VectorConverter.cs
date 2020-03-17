// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.VectorConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Shims;
using System;
using UnityEngine;

namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Json Converter for Vector2, Vector3 and Vector4.  Only serializes x, y, (z) and (w) properties.
  /// </summary>
  [Preserve]
  public class VectorConverter : JsonConverter
  {
    private static readonly Type V2 = typeof (Vector2);
    private static readonly Type V3 = typeof (Vector3);
    private static readonly Type V4 = typeof (Vector4);

    public bool EnableVector2 { get; set; }

    public bool EnableVector3 { get; set; }

    public bool EnableVector4 { get; set; }

    /// <summary>
    /// Default Constructor - All Vector types enabled by default
    /// </summary>
    public VectorConverter()
    {
      this.EnableVector2 = true;
      this.EnableVector3 = true;
      this.EnableVector4 = true;
    }

    /// <summary>Selectively enable Vector types</summary>
    /// <param name="enableVector2">Use for Vector2 objects</param>
    /// <param name="enableVector3">Use for Vector3 objects</param>
    /// <param name="enableVector4">Use for Vector4 objects</param>
    public VectorConverter(bool enableVector2, bool enableVector3, bool enableVector4)
      : this()
    {
      this.EnableVector2 = enableVector2;
      this.EnableVector3 = enableVector3;
      this.EnableVector4 = enableVector4;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="serializer"></param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        Type type = value.GetType();
        if (type == VectorConverter.V2)
        {
          Vector2 vector2 = (Vector2) value;
          VectorConverter.WriteVector(writer, (float) vector2.x, (float) vector2.y, new float?(), new float?());
        }
        else if (type == VectorConverter.V3)
        {
          Vector3 vector3 = (Vector3) value;
          VectorConverter.WriteVector(writer, (float) vector3.x, (float) vector3.y, new float?((float) vector3.z), new float?());
        }
        else if (type == VectorConverter.V4)
        {
          Vector4 vector4 = (Vector4) value;
          VectorConverter.WriteVector(writer, (float) vector4.x, (float) vector4.y, new float?((float) vector4.z), new float?((float) vector4.w));
        }
        else
          writer.WriteNull();
      }
    }

    private static void WriteVector(JsonWriter writer, float x, float y, float? z, float? w)
    {
      writer.WriteStartObject();
      writer.WritePropertyName(nameof (x));
      writer.WriteValue(x);
      writer.WritePropertyName(nameof (y));
      writer.WriteValue(y);
      if (z.HasValue)
      {
        writer.WritePropertyName(nameof (z));
        writer.WriteValue(z.Value);
        if (w.HasValue)
        {
          writer.WritePropertyName(nameof (w));
          writer.WriteValue(w.Value);
        }
      }
      writer.WriteEndObject();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="objectType"></param>
    /// <param name="existingValue"></param>
    /// <param name="serializer"></param>
    /// <returns></returns>
    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (objectType == VectorConverter.V2)
        return (object) VectorConverter.PopulateVector2(reader);
      return objectType == VectorConverter.V3 ? (object) VectorConverter.PopulateVector3(reader) : (object) VectorConverter.PopulateVector4(reader);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objectType"></param>
    /// <returns></returns>
    public override bool CanConvert(Type objectType)
    {
      if (this.EnableVector2 && objectType == VectorConverter.V2 || this.EnableVector3 && objectType == VectorConverter.V3)
        return true;
      return this.EnableVector4 && objectType == VectorConverter.V4;
    }

    private static Vector2 PopulateVector2(JsonReader reader)
    {
      Vector2 vector2 = (Vector2) null;
      if (reader.TokenType != JsonToken.Null)
      {
        JObject jobject = JObject.Load(reader);
        vector2.x = (__Null) (double) jobject["x"].Value<float>();
        vector2.y = (__Null) (double) jobject["y"].Value<float>();
      }
      return vector2;
    }

    private static Vector3 PopulateVector3(JsonReader reader)
    {
      Vector3 vector3 = (Vector3) null;
      if (reader.TokenType != JsonToken.Null)
      {
        JObject jobject = JObject.Load(reader);
        vector3.x = (__Null) (double) jobject["x"].Value<float>();
        vector3.y = (__Null) (double) jobject["y"].Value<float>();
        vector3.z = (__Null) (double) jobject["z"].Value<float>();
      }
      return vector3;
    }

    private static Vector4 PopulateVector4(JsonReader reader)
    {
      Vector4 vector4 = (Vector4) null;
      if (reader.TokenType != JsonToken.Null)
      {
        JObject jobject = JObject.Load(reader);
        vector4.x = (__Null) (double) jobject["x"].Value<float>();
        vector4.y = (__Null) (double) jobject["y"].Value<float>();
        vector4.z = (__Null) (double) jobject["z"].Value<float>();
        vector4.w = (__Null) (double) jobject["w"].Value<float>();
      }
      return vector4;
    }
  }
}
