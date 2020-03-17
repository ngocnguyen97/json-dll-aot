// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.KeyValuePairConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Converts a <see cref="T:System.Collections.Generic.KeyValuePair`2" /> to and from JSON.
  /// </summary>
  [Preserve]
  public class KeyValuePairConverter : JsonConverter
  {
    private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(new Func<Type, ReflectionObject>(KeyValuePairConverter.InitializeReflectionObject));
    private const string KeyName = "Key";
    private const string ValueName = "Value";

    private static ReflectionObject InitializeReflectionObject(Type t)
    {
      Type[] genericArguments = t.GetGenericArguments();
      Type type1 = ((IList<Type>) genericArguments)[0];
      Type type2 = ((IList<Type>) genericArguments)[1];
      return ReflectionObject.Create(t, (MethodBase) t.GetConstructor(new Type[2]
      {
        type1,
        type2
      }), "Key", "Value");
    }

    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      ReflectionObject reflectionObject = KeyValuePairConverter.ReflectionObjectPerType.Get(value.GetType());
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      writer.WriteStartObject();
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Key") : "Key");
      serializer.Serialize(writer, reflectionObject.GetValue(value, "Key"), reflectionObject.GetType("Key"));
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Value") : "Value");
      serializer.Serialize(writer, reflectionObject.GetValue(value, "Value"), reflectionObject.GetType("Value"));
      writer.WriteEndObject();
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
        if (!ReflectionUtils.IsNullableType(objectType))
          throw JsonSerializationException.Create(reader, "Cannot convert null value to KeyValuePair.");
        return (object) null;
      }
      object obj1 = (object) null;
      object obj2 = (object) null;
      reader.ReadAndAssert();
      Type key = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      ReflectionObject reflectionObject = KeyValuePairConverter.ReflectionObjectPerType.Get(key);
      while (reader.TokenType == JsonToken.PropertyName)
      {
        string a = reader.Value.ToString();
        if (string.Equals(a, "Key", StringComparison.OrdinalIgnoreCase))
        {
          reader.ReadAndAssert();
          obj1 = serializer.Deserialize(reader, reflectionObject.GetType("Key"));
        }
        else if (string.Equals(a, "Value", StringComparison.OrdinalIgnoreCase))
        {
          reader.ReadAndAssert();
          obj2 = serializer.Deserialize(reader, reflectionObject.GetType("Value"));
        }
        else
          reader.Skip();
        reader.ReadAndAssert();
      }
      return reflectionObject.Creator(new object[2]
      {
        obj1,
        obj2
      });
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
      Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      return type.IsValueType() && type.IsGenericType() && type.GetGenericTypeDefinition() == typeof (KeyValuePair<,>);
    }
  }
}
