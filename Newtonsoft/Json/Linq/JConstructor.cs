// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JConstructor
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Represents a JSON constructor.</summary>
  [Preserve]
  public class JConstructor : JContainer
  {
    private readonly List<JToken> _values = new List<JToken>();
    private string _name;

    /// <summary>Gets the container's children tokens.</summary>
    /// <value>The container's children tokens.</value>
    protected override IList<JToken> ChildrenTokens
    {
      get
      {
        return (IList<JToken>) this._values;
      }
    }

    internal override int IndexOfItem(JToken item)
    {
      return this._values.IndexOfReference<JToken>(item);
    }

    internal override void MergeItem(object content, JsonMergeSettings settings)
    {
      if (!(content is JConstructor jconstructor))
        return;
      if (jconstructor.Name != null)
        this.Name = jconstructor.Name;
      JContainer.MergeEnumerableContent((JContainer) this, (IEnumerable) jconstructor, settings);
    }

    /// <summary>Gets or sets the name of this constructor.</summary>
    /// <value>The constructor name.</value>
    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }

    /// <summary>
    /// Gets the node type for this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <value>The type.</value>
    public override JTokenType Type
    {
      get
      {
        return JTokenType.Constructor;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> class.
    /// </summary>
    public JConstructor()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> class from another <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> object.
    /// </summary>
    /// <param name="other">A <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> object to copy from.</param>
    public JConstructor(JConstructor other)
      : base((JContainer) other)
    {
      this._name = other.Name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> class with the specified name and content.
    /// </summary>
    /// <param name="name">The constructor name.</param>
    /// <param name="content">The contents of the constructor.</param>
    public JConstructor(string name, params object[] content)
      : this(name, (object) content)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> class with the specified name and content.
    /// </summary>
    /// <param name="name">The constructor name.</param>
    /// <param name="content">The contents of the constructor.</param>
    public JConstructor(string name, object content)
      : this(name)
    {
      this.Add(content);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> class with the specified name.
    /// </summary>
    /// <param name="name">The constructor name.</param>
    public JConstructor(string name)
    {
      switch (name)
      {
        case "":
          throw new ArgumentException("Constructor name cannot be empty.", nameof (name));
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          this._name = name;
          break;
      }
    }

    internal override bool DeepEquals(JToken node)
    {
      return node is JConstructor jconstructor && this._name == jconstructor.Name && this.ContentsEqual((JContainer) jconstructor);
    }

    internal override JToken CloneToken()
    {
      return (JToken) new JConstructor(this);
    }

    /// <summary>
    /// Writes this token to a <see cref="T:Newtonsoft.Json.JsonWriter" />.
    /// </summary>
    /// <param name="writer">A <see cref="T:Newtonsoft.Json.JsonWriter" /> into which this method will write.</param>
    /// <param name="converters">A collection of <see cref="T:Newtonsoft.Json.JsonConverter" /> which will be used when writing the token.</param>
    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartConstructor(this._name);
      foreach (JToken child in this.Children())
        child.WriteTo(writer, converters);
      writer.WriteEndConstructor();
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
        if (!(key is int index))
          throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        return this.GetItem(index);
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, nameof (key));
        if (!(key is int index))
          throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this.SetItem(index, value);
      }
    }

    internal override int GetDeepHashCode()
    {
      return this._name.GetHashCode() ^ this.ContentsHashCode();
    }

    /// <summary>
    /// Loads an <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" />.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static JConstructor Load(JsonReader reader)
    {
      return JConstructor.Load(reader, (JsonLoadSettings) null);
    }

    /// <summary>
    /// Loads an <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" />.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is null, default load settings will be used.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static JConstructor Load(JsonReader reader, JsonLoadSettings settings)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
      reader.MoveToContent();
      if (reader.TokenType != JsonToken.StartConstructor)
        throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JConstructor jconstructor = new JConstructor((string) reader.Value);
      jconstructor.SetLineInfo(reader as IJsonLineInfo, settings);
      jconstructor.ReadTokenFrom(reader, settings);
      return jconstructor;
    }
  }
}
