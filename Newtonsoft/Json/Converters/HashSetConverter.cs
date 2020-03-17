// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.HashSetConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Converters
{
  public class HashSetConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      bool flag = serializer.ObjectCreationHandling == ObjectCreationHandling.Replace;
      if (reader.TokenType == JsonToken.Null)
        return !flag ? existingValue : (object) null;
      object obj1 = flag || existingValue == null ? Activator.CreateInstance(objectType) : existingValue;
      Type genericArgument = objectType.GetGenericArguments()[0];
      MethodInfo method = objectType.GetMethod("Add");
      JArray jarray = JArray.Load(reader);
      for (int index = 0; index < jarray.Count; ++index)
      {
        object obj2 = serializer.Deserialize(jarray[index].CreateReader(), genericArgument);
        method.Invoke(obj1, new object[1]{ obj2 });
      }
      return obj1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objectType"></param>
    /// <returns></returns>
    public override bool CanConvert(Type objectType)
    {
      return objectType.IsGenericType() && objectType.GetGenericTypeDefinition() == typeof (HashSet<>);
    }

    public override bool CanWrite
    {
      get
      {
        return false;
      }
    }
  }
}
