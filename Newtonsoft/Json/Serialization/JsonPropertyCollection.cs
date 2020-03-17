// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonPropertyCollection
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// A collection of <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> objects.
  /// </summary>
  [Preserve]
  public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
  {
    private readonly Type _type;
    private readonly List<JsonProperty> _list;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonPropertyCollection" /> class.
    /// </summary>
    /// <param name="type">The type.</param>
    public JsonPropertyCollection(Type type)
      : base((IEqualityComparer<string>) StringComparer.Ordinal)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      this._type = type;
      this._list = (List<JsonProperty>) this.Items;
    }

    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <param name="item">The element from which to extract the key.</param>
    /// <returns>The key for the specified element.</returns>
    protected override string GetKeyForItem(JsonProperty item)
    {
      return item.PropertyName;
    }

    /// <summary>
    /// Adds a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> object.
    /// </summary>
    /// <param name="property">The property to add to the collection.</param>
    public void AddProperty(JsonProperty property)
    {
      if (this.Contains(property.PropertyName))
      {
        if (property.Ignored)
          return;
        JsonProperty jsonProperty = this[property.PropertyName];
        bool flag = true;
        if (jsonProperty.Ignored)
        {
          this.Remove(jsonProperty);
          flag = false;
        }
        else if (property.DeclaringType != null && jsonProperty.DeclaringType != null)
        {
          if (property.DeclaringType.IsSubclassOf(jsonProperty.DeclaringType) || jsonProperty.DeclaringType.IsInterface() && property.DeclaringType.ImplementInterface(jsonProperty.DeclaringType))
          {
            this.Remove(jsonProperty);
            flag = false;
          }
          if (jsonProperty.DeclaringType.IsSubclassOf(property.DeclaringType) || property.DeclaringType.IsInterface() && jsonProperty.DeclaringType.ImplementInterface(property.DeclaringType))
            return;
        }
        if (flag)
          throw new JsonSerializationException("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName, (object) this._type));
      }
      this.Add(property);
    }

    /// <summary>
    /// Gets the closest matching <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> object.
    /// First attempts to get an exact case match of propertyName and then
    /// a case insensitive match.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>A matching property if found.</returns>
    public JsonProperty GetClosestMatchProperty(string propertyName)
    {
      return this.GetProperty(propertyName, StringComparison.Ordinal) ?? this.GetProperty(propertyName, StringComparison.OrdinalIgnoreCase);
    }

    private bool TryGetValue(string key, out JsonProperty item)
    {
      if (this.Dictionary != null)
        return this.Dictionary.TryGetValue(key, out item);
      item = (JsonProperty) null;
      return false;
    }

    /// <summary>Gets a property by property name.</summary>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <param name="comparisonType">Type property name string comparison.</param>
    /// <returns>A matching property if found.</returns>
    public JsonProperty GetProperty(
      string propertyName,
      StringComparison comparisonType)
    {
      if (comparisonType == StringComparison.Ordinal)
      {
        JsonProperty jsonProperty;
        return this.TryGetValue(propertyName, out jsonProperty) ? jsonProperty : (JsonProperty) null;
      }
      for (int index = 0; index < this._list.Count; ++index)
      {
        JsonProperty jsonProperty = this._list[index];
        if (string.Equals(propertyName, jsonProperty.PropertyName, comparisonType))
          return jsonProperty;
      }
      return (JsonProperty) null;
    }
  }
}
