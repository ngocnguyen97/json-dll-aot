// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.EnumerableVectorConverter`1
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
  /// <summary>
  /// 
  /// </summary>
  public class EnumerableVectorConverter<T> : JsonConverter
  {
    private static readonly VectorConverter VectorConverter = new VectorConverter();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="serializer"></param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
        writer.WriteNull();
      T[] objArray = value is IEnumerable<T> source ? source.ToArray<T>() : (T[]) null;
      if (objArray == null)
      {
        writer.WriteNull();
      }
      else
      {
        writer.WriteStartArray();
        for (int index = 0; index < objArray.Length; ++index)
          EnumerableVectorConverter<T>.VectorConverter.WriteJson(writer, (object) objArray[index], serializer);
        writer.WriteEndArray();
      }
    }

    public override bool CanConvert(Type objectType)
    {
      return typeof (IEnumerable<Vector2>).IsAssignableFrom(objectType) || typeof (IEnumerable<Vector3>).IsAssignableFrom(objectType) || typeof (IEnumerable<Vector4>).IsAssignableFrom(objectType);
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      List<T> objList = new List<T>();
      JObject jobject = JObject.Load(reader);
      for (int index = 0; index < jobject.Count; ++index)
        objList.Add(JsonConvert.DeserializeObject<T>(jobject[(object) index].ToString()));
      return (object) objList;
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
