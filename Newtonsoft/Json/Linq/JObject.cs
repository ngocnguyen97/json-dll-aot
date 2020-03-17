﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JObject
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Represents a JSON object.</summary>
  /// <example>
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\LinqToJsonTests.cs" region="LinqToJsonCreateParse" title="Parsing a JSON Object from Text" />
  /// </example>
  [Preserve]
  public class JObject : JContainer, IDictionary<string, JToken>, ICollection<KeyValuePair<string, JToken>>, IEnumerable<KeyValuePair<string, JToken>>, IEnumerable, INotifyPropertyChanged, ICustomTypeDescriptor, INotifyPropertyChanging
  {
    private readonly JPropertyKeyedCollection _properties = new JPropertyKeyedCollection();

    /// <summary>Gets the container's children tokens.</summary>
    /// <value>The container's children tokens.</value>
    protected override IList<JToken> ChildrenTokens
    {
      get
      {
        return (IList<JToken>) this._properties;
      }
    }

    /// <summary>Occurs when a property value changes.</summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>Occurs when a property value is changing.</summary>
    public event PropertyChangingEventHandler PropertyChanging;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JObject" /> class.
    /// </summary>
    public JObject()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JObject" /> class from another <see cref="T:Newtonsoft.Json.Linq.JObject" /> object.
    /// </summary>
    /// <param name="other">A <see cref="T:Newtonsoft.Json.Linq.JObject" /> object to copy from.</param>
    public JObject(JObject other)
      : base((JContainer) other)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JObject" /> class with the specified content.
    /// </summary>
    /// <param name="content">The contents of the object.</param>
    public JObject(params object[] content)
      : this((object) content)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JObject" /> class with the specified content.
    /// </summary>
    /// <param name="content">The contents of the object.</param>
    public JObject(object content)
    {
      this.Add(content);
    }

    internal override bool DeepEquals(JToken node)
    {
      return node is JObject jobject && this._properties.Compare(jobject._properties);
    }

    internal override int IndexOfItem(JToken item)
    {
      return this._properties.IndexOfReference(item);
    }

    internal override void InsertItem(int index, JToken item, bool skipParentCheck)
    {
      if (item != null && item.Type == JTokenType.Comment)
        return;
      base.InsertItem(index, item, skipParentCheck);
    }

    internal override void ValidateToken(JToken o, JToken existing)
    {
      ValidationUtils.ArgumentNotNull((object) o, nameof (o));
      if (o.Type != JTokenType.Property)
        throw new ArgumentException("Can not add {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) o.GetType(), (object) this.GetType()));
      JProperty jproperty1 = (JProperty) o;
      if (existing != null)
      {
        JProperty jproperty2 = (JProperty) existing;
        if (jproperty1.Name == jproperty2.Name)
          return;
      }
      if (this._properties.TryGetValue(jproperty1.Name, out existing))
        throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jproperty1.Name, (object) this.GetType()));
    }

    internal override void MergeItem(object content, JsonMergeSettings settings)
    {
      if (!(content is JObject jobject))
        return;
      foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
      {
        JProperty jproperty = this.Property(keyValuePair.Key);
        if (jproperty == null)
          this.Add(keyValuePair.Key, keyValuePair.Value);
        else if (keyValuePair.Value != null)
        {
          if (!(jproperty.Value is JContainer jcontainer))
          {
            if (keyValuePair.Value.Type != JTokenType.Null || settings != null && settings.MergeNullValueHandling == MergeNullValueHandling.Merge)
              jproperty.Value = keyValuePair.Value;
          }
          else if (jcontainer.Type != keyValuePair.Value.Type)
            jproperty.Value = keyValuePair.Value;
          else
            jcontainer.Merge((object) keyValuePair.Value, settings);
        }
      }
    }

    internal void InternalPropertyChanged(JProperty childProperty)
    {
      this.OnPropertyChanged(childProperty.Name);
      if (this._listChanged == null)
        return;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.IndexOfItem((JToken) childProperty)));
    }

    internal void InternalPropertyChanging(JProperty childProperty)
    {
      this.OnPropertyChanging(childProperty.Name);
    }

    internal override JToken CloneToken()
    {
      return (JToken) new JObject(this);
    }

    /// <summary>
    /// Gets the node type for this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <value>The type.</value>
    public override JTokenType Type
    {
      get
      {
        return JTokenType.Object;
      }
    }

    /// <summary>
    /// Gets an <see cref="T:System.Collections.Generic.IEnumerable`1" /> of this object's properties.
    /// </summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of this object's properties.</returns>
    public IEnumerable<JProperty> Properties()
    {
      return this._properties.Cast<JProperty>();
    }

    /// <summary>
    /// Gets a <see cref="T:Newtonsoft.Json.Linq.JProperty" /> the specified name.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JProperty" /> with the specified name or null.</returns>
    public JProperty Property(string name)
    {
      if (name == null)
        return (JProperty) null;
      JToken jtoken;
      this._properties.TryGetValue(name, out jtoken);
      return (JProperty) jtoken;
    }

    /// <summary>
    /// Gets an <see cref="T:Newtonsoft.Json.Linq.JEnumerable`1" /> of this object's property values.
    /// </summary>
    /// <returns>An <see cref="T:Newtonsoft.Json.Linq.JEnumerable`1" /> of this object's property values.</returns>
    public JEnumerable<JToken> PropertyValues()
    {
      return new JEnumerable<JToken>(this.Properties().Select<JProperty, JToken>((Func<JProperty, JToken>) (p => p.Value)));
    }

    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified key.
    /// </summary>
    /// <value>The <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified key.</value>
    public override JToken this[object key]
    {
      get
      {
        ValidationUtils.ArgumentNotNull(key, nameof (key));
        if (!(key is string index))
          throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        return this[index];
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, nameof (key));
        if (!(key is string index))
          throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this[index] = value;
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified property name.
    /// </summary>
    /// <value></value>
    public JToken this[string propertyName]
    {
      get
      {
        ValidationUtils.ArgumentNotNull((object) propertyName, nameof (propertyName));
        return this.Property(propertyName)?.Value;
      }
      set
      {
        JProperty jproperty = this.Property(propertyName);
        if (jproperty != null)
        {
          jproperty.Value = value;
        }
        else
        {
          this.OnPropertyChanging(propertyName);
          this.Add((object) new JProperty(propertyName, (object) value));
          this.OnPropertyChanged(propertyName);
        }
      }
    }

    /// <summary>
    /// Loads an <see cref="T:Newtonsoft.Json.Linq.JObject" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JObject" />.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JObject" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static JObject Load(JsonReader reader)
    {
      return JObject.Load(reader, (JsonLoadSettings) null);
    }

    /// <summary>
    /// Loads an <see cref="T:Newtonsoft.Json.Linq.JObject" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JObject" />.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is null, default load settings will be used.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JObject" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static JObject Load(JsonReader reader, JsonLoadSettings settings)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader.");
      reader.MoveToContent();
      if (reader.TokenType != JsonToken.StartObject)
        throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JObject jobject = new JObject();
      jobject.SetLineInfo(reader as IJsonLineInfo, settings);
      jobject.ReadTokenFrom(reader, settings);
      return jobject;
    }

    /// <summary>
    /// Load a <see cref="T:Newtonsoft.Json.Linq.JObject" /> from a string that contains JSON.
    /// </summary>
    /// <param name="json">A <see cref="T:System.String" /> that contains JSON.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JObject" /> populated from the string that contains JSON.</returns>
    /// <example>
    ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\LinqToJsonTests.cs" region="LinqToJsonCreateParse" title="Parsing a JSON Object from Text" />
    /// </example>
    public static JObject Parse(string json)
    {
      return JObject.Parse(json, (JsonLoadSettings) null);
    }

    /// <summary>
    /// Load a <see cref="T:Newtonsoft.Json.Linq.JObject" /> from a string that contains JSON.
    /// </summary>
    /// <param name="json">A <see cref="T:System.String" /> that contains JSON.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is null, default load settings will be used.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JObject" /> populated from the string that contains JSON.</returns>
    /// <example>
    ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\LinqToJsonTests.cs" region="LinqToJsonCreateParse" title="Parsing a JSON Object from Text" />
    /// </example>
    public static JObject Parse(string json, JsonLoadSettings settings)
    {
      using (JsonReader reader = (JsonReader) new JsonTextReader((TextReader) new StringReader(json)))
      {
        JObject jobject = JObject.Load(reader, settings);
        if (reader.Read() && reader.TokenType != JsonToken.Comment)
          throw JsonReaderException.Create(reader, "Additional text found in JSON string after parsing content.");
        return jobject;
      }
    }

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JObject" /> from an object.
    /// </summary>
    /// <param name="o">The object that will be used to create <see cref="T:Newtonsoft.Json.Linq.JObject" />.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JObject" /> with the values of the specified object</returns>
    public static JObject FromObject(object o)
    {
      return JObject.FromObject(o, JsonSerializer.CreateDefault());
    }

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JObject" /> from an object.
    /// </summary>
    /// <param name="o">The object that will be used to create <see cref="T:Newtonsoft.Json.Linq.JObject" />.</param>
    /// <param name="jsonSerializer">The <see cref="T:Newtonsoft.Json.JsonSerializer" /> that will be used to read the object.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JObject" /> with the values of the specified object</returns>
    public static JObject FromObject(object o, JsonSerializer jsonSerializer)
    {
      JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
      if (jtoken != null && jtoken.Type != JTokenType.Object)
        throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jtoken.Type));
      return (JObject) jtoken;
    }

    /// <summary>
    /// Writes this token to a <see cref="T:Newtonsoft.Json.JsonWriter" />.
    /// </summary>
    /// <param name="writer">A <see cref="T:Newtonsoft.Json.JsonWriter" /> into which this method will write.</param>
    /// <param name="converters">A collection of <see cref="T:Newtonsoft.Json.JsonConverter" /> which will be used when writing the token.</param>
    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartObject();
      for (int index = 0; index < this._properties.Count; ++index)
        this._properties[index].WriteTo(writer, converters);
      writer.WriteEndObject();
    }

    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified property name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified property name.</returns>
    public JToken GetValue(string propertyName)
    {
      return this.GetValue(propertyName, StringComparison.Ordinal);
    }

    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified property name.
    /// The exact property name will be searched for first and if no matching property is found then
    /// the <see cref="T:System.StringComparison" /> will be used to match a property.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="comparison">One of the enumeration values that specifies how the strings will be compared.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified property name.</returns>
    public JToken GetValue(string propertyName, StringComparison comparison)
    {
      if (propertyName == null)
        return (JToken) null;
      JProperty jproperty = this.Property(propertyName);
      if (jproperty != null)
        return jproperty.Value;
      if (comparison != StringComparison.Ordinal)
      {
        foreach (JProperty property in (Collection<JToken>) this._properties)
        {
          if (string.Equals(property.Name, propertyName, comparison))
            return property.Value;
        }
      }
      return (JToken) null;
    }

    /// <summary>
    /// Tries to get the <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified property name.
    /// The exact property name will be searched for first and if no matching property is found then
    /// the <see cref="T:System.StringComparison" /> will be used to match a property.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="value">The value.</param>
    /// <param name="comparison">One of the enumeration values that specifies how the strings will be compared.</param>
    /// <returns>true if a value was successfully retrieved; otherwise, false.</returns>
    public bool TryGetValue(string propertyName, StringComparison comparison, out JToken value)
    {
      value = this.GetValue(propertyName, comparison);
      return value != null;
    }

    /// <summary>Adds the specified property name.</summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="value">The value.</param>
    public void Add(string propertyName, JToken value)
    {
      this.Add((object) new JProperty(propertyName, (object) value));
    }

    bool IDictionary<string, JToken>.ContainsKey(string key)
    {
      return this._properties.Contains(key);
    }

    ICollection<string> IDictionary<string, JToken>.Keys
    {
      get
      {
        return this._properties.Keys;
      }
    }

    /// <summary>Removes the property with the specified name.</summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>true if item was successfully removed; otherwise, false.</returns>
    public bool Remove(string propertyName)
    {
      JProperty jproperty = this.Property(propertyName);
      if (jproperty == null)
        return false;
      jproperty.Remove();
      return true;
    }

    /// <summary>Tries the get value.</summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="value">The value.</param>
    /// <returns>true if a value was successfully retrieved; otherwise, false.</returns>
    public bool TryGetValue(string propertyName, out JToken value)
    {
      JProperty jproperty = this.Property(propertyName);
      if (jproperty == null)
      {
        value = (JToken) null;
        return false;
      }
      value = jproperty.Value;
      return true;
    }

    ICollection<JToken> IDictionary<string, JToken>.Values
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    void ICollection<KeyValuePair<string, JToken>>.Add(
      KeyValuePair<string, JToken> item)
    {
      this.Add((object) new JProperty(item.Key, (object) item.Value));
    }

    void ICollection<KeyValuePair<string, JToken>>.Clear()
    {
      this.RemoveAll();
    }

    bool ICollection<KeyValuePair<string, JToken>>.Contains(
      KeyValuePair<string, JToken> item)
    {
      JProperty jproperty = this.Property(item.Key);
      return jproperty != null && jproperty.Value == item.Value;
    }

    void ICollection<KeyValuePair<string, JToken>>.CopyTo(
      KeyValuePair<string, JToken>[] array,
      int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex), "arrayIndex is less than 0.");
      if (arrayIndex >= array.Length && arrayIndex != 0)
        throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
      if (this.Count > array.Length - arrayIndex)
        throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
      int num = 0;
      foreach (JProperty property in (Collection<JToken>) this._properties)
      {
        array[arrayIndex + num] = new KeyValuePair<string, JToken>(property.Name, property.Value);
        ++num;
      }
    }

    bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    bool ICollection<KeyValuePair<string, JToken>>.Remove(
      KeyValuePair<string, JToken> item)
    {
      if (!((ICollection<KeyValuePair<string, JToken>>) this).Contains(item))
        return false;
      this.Remove(item.Key);
      return true;
    }

    internal override int GetDeepHashCode()
    {
      return this.ContentsHashCode();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
    {
      foreach (JProperty property in (Collection<JToken>) this._properties)
        yield return new KeyValuePair<string, JToken>(property.Name, property.Value);
    }

    /// <summary>
    /// Raises the <see cref="E:Newtonsoft.Json.Linq.JObject.PropertyChanged" /> event with the provided arguments.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Raises the <see cref="E:Newtonsoft.Json.Linq.JObject.PropertyChanging" /> event with the provided arguments.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    protected virtual void OnPropertyChanging(string propertyName)
    {
      if (this.PropertyChanging == null)
        return;
      this.PropertyChanging((object) this, new PropertyChangingEventArgs(propertyName));
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
    {
      return ((ICustomTypeDescriptor) this).GetProperties((Attribute[]) null);
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(
      Attribute[] attributes)
    {
      PropertyDescriptorCollection descriptorCollection = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
      foreach (KeyValuePair<string, JToken> keyValuePair in this)
        descriptorCollection.Add((PropertyDescriptor) new JPropertyDescriptor(keyValuePair.Key));
      return descriptorCollection;
    }

    AttributeCollection ICustomTypeDescriptor.GetAttributes()
    {
      return AttributeCollection.Empty;
    }

    string ICustomTypeDescriptor.GetClassName()
    {
      return (string) null;
    }

    string ICustomTypeDescriptor.GetComponentName()
    {
      return (string) null;
    }

    TypeConverter ICustomTypeDescriptor.GetConverter()
    {
      return new TypeConverter();
    }

    EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
    {
      return (EventDescriptor) null;
    }

    PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
    {
      return (PropertyDescriptor) null;
    }

    object ICustomTypeDescriptor.GetEditor(System.Type editorBaseType)
    {
      return (object) null;
    }

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents(
      Attribute[] attributes)
    {
      return EventDescriptorCollection.Empty;
    }

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
    {
      return EventDescriptorCollection.Empty;
    }

    object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
    {
      return (object) null;
    }
  }
}
