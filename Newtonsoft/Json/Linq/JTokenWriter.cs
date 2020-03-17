// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JTokenWriter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
  /// <summary>
  /// Represents a writer that provides a fast, non-cached, forward-only way of generating JSON data.
  /// </summary>
  [Preserve]
  public class JTokenWriter : JsonWriter
  {
    private JContainer _token;
    private JContainer _parent;
    private JValue _value;
    private JToken _current;

    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Linq.JToken" /> at the writer's current position.
    /// </summary>
    public JToken CurrentToken
    {
      get
      {
        return this._current;
      }
    }

    /// <summary>Gets the token being writen.</summary>
    /// <value>The token being writen.</value>
    public JToken Token
    {
      get
      {
        return this._token != null ? (JToken) this._token : (JToken) this._value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JTokenWriter" /> class writing to the given <see cref="T:Newtonsoft.Json.Linq.JContainer" />.
    /// </summary>
    /// <param name="container">The container being written to.</param>
    public JTokenWriter(JContainer container)
    {
      ValidationUtils.ArgumentNotNull((object) container, nameof (container));
      this._token = container;
      this._parent = container;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JTokenWriter" /> class.
    /// </summary>
    public JTokenWriter()
    {
    }

    /// <summary>
    /// Flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
    /// </summary>
    public override void Flush()
    {
    }

    /// <summary>Closes this stream and the underlying stream.</summary>
    public override void Close()
    {
      base.Close();
    }

    /// <summary>Writes the beginning of a JSON object.</summary>
    public override void WriteStartObject()
    {
      base.WriteStartObject();
      this.AddParent((JContainer) new JObject());
    }

    private void AddParent(JContainer container)
    {
      if (this._parent == null)
        this._token = container;
      else
        this._parent.AddAndSkipParentCheck((JToken) container);
      this._parent = container;
      this._current = (JToken) container;
    }

    private void RemoveParent()
    {
      this._current = (JToken) this._parent;
      this._parent = this._parent.Parent;
      if (this._parent == null || this._parent.Type != JTokenType.Property)
        return;
      this._parent = this._parent.Parent;
    }

    /// <summary>Writes the beginning of a JSON array.</summary>
    public override void WriteStartArray()
    {
      base.WriteStartArray();
      this.AddParent((JContainer) new JArray());
    }

    /// <summary>
    /// Writes the start of a constructor with the given name.
    /// </summary>
    /// <param name="name">The name of the constructor.</param>
    public override void WriteStartConstructor(string name)
    {
      base.WriteStartConstructor(name);
      this.AddParent((JContainer) new JConstructor(name));
    }

    /// <summary>Writes the end.</summary>
    /// <param name="token">The token.</param>
    protected override void WriteEnd(JsonToken token)
    {
      this.RemoveParent();
    }

    /// <summary>
    /// Writes the property name of a name/value pair on a JSON object.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    public override void WritePropertyName(string name)
    {
      if (this._parent is JObject parent)
        parent.Remove(name);
      this.AddParent((JContainer) new JProperty(name));
      base.WritePropertyName(name);
    }

    private void AddValue(object value, JsonToken token)
    {
      this.AddValue(new JValue(value), token);
    }

    internal void AddValue(JValue value, JsonToken token)
    {
      if (this._parent != null)
      {
        this._parent.Add((object) value);
        this._current = this._parent.Last;
        if (this._parent.Type != JTokenType.Property)
          return;
        this._parent = this._parent.Parent;
      }
      else
      {
        this._value = value ?? JValue.CreateNull();
        this._current = (JToken) this._value;
      }
    }

    /// <summary>
    /// Writes a <see cref="T:System.Object" /> value.
    /// An error will raised if the value cannot be written as a single JSON token.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Object" /> value to write.</param>
    public override void WriteValue(object value)
    {
      base.WriteValue(value);
    }

    /// <summary>Writes a null value.</summary>
    public override void WriteNull()
    {
      base.WriteNull();
      this.AddValue((JValue) null, JsonToken.Null);
    }

    /// <summary>Writes an undefined value.</summary>
    public override void WriteUndefined()
    {
      base.WriteUndefined();
      this.AddValue((JValue) null, JsonToken.Undefined);
    }

    /// <summary>Writes raw JSON.</summary>
    /// <param name="json">The raw JSON to write.</param>
    public override void WriteRaw(string json)
    {
      base.WriteRaw(json);
      this.AddValue((JValue) new JRaw((object) json), JsonToken.Raw);
    }

    /// <summary>
    /// Writes out a comment <code>/*...*/</code> containing the specified text.
    /// </summary>
    /// <param name="text">Text to place inside the comment.</param>
    public override void WriteComment(string text)
    {
      base.WriteComment(text);
      this.AddValue(JValue.CreateComment(text), JsonToken.Comment);
    }

    /// <summary>
    /// Writes a <see cref="T:System.String" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.String" /> value to write.</param>
    public override void WriteValue(string value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.String);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Int32" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Int32" /> value to write.</param>
    public override void WriteValue(int value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    /// <summary>
    /// Writes a <see cref="T:System.UInt32" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.UInt32" /> value to write.</param>
    [CLSCompliant(false)]
    public override void WriteValue(uint value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Int64" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Int64" /> value to write.</param>
    public override void WriteValue(long value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    /// <summary>
    /// Writes a <see cref="T:System.UInt64" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.UInt64" /> value to write.</param>
    [CLSCompliant(false)]
    public override void WriteValue(ulong value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Single" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Single" /> value to write.</param>
    public override void WriteValue(float value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Float);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Double" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Double" /> value to write.</param>
    public override void WriteValue(double value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Float);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Boolean" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Boolean" /> value to write.</param>
    public override void WriteValue(bool value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Boolean);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Int16" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Int16" /> value to write.</param>
    public override void WriteValue(short value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    /// <summary>
    /// Writes a <see cref="T:System.UInt16" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.UInt16" /> value to write.</param>
    [CLSCompliant(false)]
    public override void WriteValue(ushort value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Char" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Char" /> value to write.</param>
    public override void WriteValue(char value)
    {
      base.WriteValue(value);
      this.AddValue((object) value.ToString((IFormatProvider) CultureInfo.InvariantCulture), JsonToken.String);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Byte" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Byte" /> value to write.</param>
    public override void WriteValue(byte value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    /// <summary>
    /// Writes a <see cref="T:System.SByte" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.SByte" /> value to write.</param>
    [CLSCompliant(false)]
    public override void WriteValue(sbyte value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Decimal" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Decimal" /> value to write.</param>
    public override void WriteValue(Decimal value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Float);
    }

    /// <summary>
    /// Writes a <see cref="T:System.DateTime" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.DateTime" /> value to write.</param>
    public override void WriteValue(DateTime value)
    {
      base.WriteValue(value);
      value = DateTimeUtils.EnsureDateTime(value, this.DateTimeZoneHandling);
      this.AddValue((object) value, JsonToken.Date);
    }

    /// <summary>
    /// Writes a <see cref="T:System.DateTimeOffset" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.DateTimeOffset" /> value to write.</param>
    public override void WriteValue(DateTimeOffset value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Date);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Byte" />[] value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Byte" />[] value to write.</param>
    public override void WriteValue(byte[] value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Bytes);
    }

    /// <summary>
    /// Writes a <see cref="T:System.TimeSpan" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.TimeSpan" /> value to write.</param>
    public override void WriteValue(TimeSpan value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.String);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Guid" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Guid" /> value to write.</param>
    public override void WriteValue(Guid value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.String);
    }

    /// <summary>
    /// Writes a <see cref="T:System.Uri" /> value.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Uri" /> value to write.</param>
    public override void WriteValue(Uri value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.String);
    }

    internal override void WriteToken(
      JsonReader reader,
      bool writeChildren,
      bool writeDateConstructorAsDate,
      bool writeComments)
    {
      JTokenReader jtokenReader = reader as JTokenReader;
      if (jtokenReader != null & writeChildren & writeDateConstructorAsDate & writeComments)
      {
        if (jtokenReader.TokenType == JsonToken.None && !jtokenReader.Read())
          return;
        JToken jtoken = jtokenReader.CurrentToken.CloneToken();
        if (this._parent != null)
        {
          this._parent.Add((object) jtoken);
          this._current = this._parent.Last;
          if (this._parent.Type == JTokenType.Property)
          {
            this._parent = this._parent.Parent;
            this.InternalWriteValue(JsonToken.Null);
          }
        }
        else
        {
          this._current = jtoken;
          if (this._token == null && this._value == null)
          {
            this._token = jtoken as JContainer;
            this._value = jtoken as JValue;
          }
        }
        jtokenReader.Skip();
      }
      else
        base.WriteToken(reader, writeChildren, writeDateConstructorAsDate, writeComments);
    }
  }
}
