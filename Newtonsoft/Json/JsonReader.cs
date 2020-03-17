// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonReader
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Represents a reader that provides fast, non-cached, forward-only access to serialized JSON data.
  /// </summary>
  [Preserve]
  public abstract class JsonReader : IDisposable
  {
    private JsonToken _tokenType;
    private object _value;
    internal char _quoteChar;
    internal JsonReader.State _currentState;
    private JsonPosition _currentPosition;
    private CultureInfo _culture;
    private DateTimeZoneHandling _dateTimeZoneHandling;
    private int? _maxDepth;
    private bool _hasExceededMaxDepth;
    internal DateParseHandling _dateParseHandling;
    internal FloatParseHandling _floatParseHandling;
    private string _dateFormatString;
    private List<JsonPosition> _stack;

    /// <summary>Gets the current reader state.</summary>
    /// <value>The current reader state.</value>
    protected JsonReader.State CurrentState
    {
      get
      {
        return this._currentState;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the underlying stream or
    /// <see cref="T:System.IO.TextReader" /> should be closed when the reader is closed.
    /// </summary>
    /// <value>
    /// true to close the underlying stream or <see cref="T:System.IO.TextReader" /> when
    /// the reader is closed; otherwise false. The default is true.
    /// </value>
    public bool CloseInput { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether multiple pieces of JSON content can
    /// be read from a continuous stream without erroring.
    /// </summary>
    /// <value>
    /// true to support reading multiple pieces of JSON content; otherwise false. The default is false.
    /// </value>
    public bool SupportMultipleContent { get; set; }

    /// <summary>
    /// Gets the quotation mark character used to enclose the value of a string.
    /// </summary>
    public virtual char QuoteChar
    {
      get
      {
        return this._quoteChar;
      }
      protected internal set
      {
        this._quoteChar = value;
      }
    }

    /// <summary>
    /// Get or set how <see cref="T:System.DateTime" /> time zones are handling when reading JSON.
    /// </summary>
    public DateTimeZoneHandling DateTimeZoneHandling
    {
      get
      {
        return this._dateTimeZoneHandling;
      }
      set
      {
        if (value < DateTimeZoneHandling.Local || value > DateTimeZoneHandling.RoundtripKind)
          throw new ArgumentOutOfRangeException(nameof (value));
        this._dateTimeZoneHandling = value;
      }
    }

    /// <summary>
    /// Get or set how date formatted strings, e.g. "\/Date(1198908717056)\/" and "2012-03-21T05:40Z", are parsed when reading JSON.
    /// </summary>
    public DateParseHandling DateParseHandling
    {
      get
      {
        return this._dateParseHandling;
      }
      set
      {
        if (value < DateParseHandling.None || value > DateParseHandling.DateTimeOffset)
          throw new ArgumentOutOfRangeException(nameof (value));
        this._dateParseHandling = value;
      }
    }

    /// <summary>
    /// Get or set how floating point numbers, e.g. 1.0 and 9.9, are parsed when reading JSON text.
    /// </summary>
    public FloatParseHandling FloatParseHandling
    {
      get
      {
        return this._floatParseHandling;
      }
      set
      {
        if (value < FloatParseHandling.Double || value > FloatParseHandling.Decimal)
          throw new ArgumentOutOfRangeException(nameof (value));
        this._floatParseHandling = value;
      }
    }

    /// <summary>
    /// Get or set how custom date formatted strings are parsed when reading JSON.
    /// </summary>
    public string DateFormatString
    {
      get
      {
        return this._dateFormatString;
      }
      set
      {
        this._dateFormatString = value;
      }
    }

    /// <summary>
    /// Gets or sets the maximum depth allowed when reading JSON. Reading past this depth will throw a <see cref="T:Newtonsoft.Json.JsonReaderException" />.
    /// </summary>
    public int? MaxDepth
    {
      get
      {
        return this._maxDepth;
      }
      set
      {
        int? nullable = value;
        int num = 0;
        if ((nullable.GetValueOrDefault() <= num ? (nullable.HasValue ? 1 : 0) : 0) != 0)
          throw new ArgumentException("Value must be positive.", nameof (value));
        this._maxDepth = value;
      }
    }

    /// <summary>Gets the type of the current JSON token.</summary>
    public virtual JsonToken TokenType
    {
      get
      {
        return this._tokenType;
      }
    }

    /// <summary>Gets the text value of the current JSON token.</summary>
    public virtual object Value
    {
      get
      {
        return this._value;
      }
    }

    /// <summary>
    /// Gets The Common Language Runtime (CLR) type for the current JSON token.
    /// </summary>
    public virtual Type ValueType
    {
      get
      {
        return this._value == null ? (Type) null : this._value.GetType();
      }
    }

    /// <summary>
    /// Gets the depth of the current token in the JSON document.
    /// </summary>
    /// <value>The depth of the current token in the JSON document.</value>
    public virtual int Depth
    {
      get
      {
        int num = this._stack != null ? this._stack.Count : 0;
        return JsonTokenUtils.IsStartToken(this.TokenType) || this._currentPosition.Type == JsonContainerType.None ? num : num + 1;
      }
    }

    /// <summary>Gets the path of the current JSON token.</summary>
    public virtual string Path
    {
      get
      {
        return this._currentPosition.Type == JsonContainerType.None ? string.Empty : JsonPosition.BuildPath(this._stack, (this._currentState == JsonReader.State.ArrayStart || this._currentState == JsonReader.State.ConstructorStart ? 0 : (this._currentState != JsonReader.State.ObjectStart ? 1 : 0)) != 0 ? new JsonPosition?(this._currentPosition) : new JsonPosition?());
      }
    }

    /// <summary>
    /// Gets or sets the culture used when reading JSON. Defaults to <see cref="P:System.Globalization.CultureInfo.InvariantCulture" />.
    /// </summary>
    public CultureInfo Culture
    {
      get
      {
        return this._culture ?? CultureInfo.InvariantCulture;
      }
      set
      {
        this._culture = value;
      }
    }

    internal JsonPosition GetPosition(int depth)
    {
      return this._stack != null && depth < this._stack.Count ? this._stack[depth] : this._currentPosition;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonReader" /> class with the specified <see cref="T:System.IO.TextReader" />.
    /// </summary>
    protected JsonReader()
    {
      this._currentState = JsonReader.State.Start;
      this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
      this._dateParseHandling = DateParseHandling.DateTime;
      this._floatParseHandling = FloatParseHandling.Double;
      this.CloseInput = true;
    }

    private void Push(JsonContainerType value)
    {
      this.UpdateScopeWithFinishedValue();
      if (this._currentPosition.Type == JsonContainerType.None)
      {
        this._currentPosition = new JsonPosition(value);
      }
      else
      {
        if (this._stack == null)
          this._stack = new List<JsonPosition>();
        this._stack.Add(this._currentPosition);
        this._currentPosition = new JsonPosition(value);
        if (!this._maxDepth.HasValue)
          return;
        int num = this.Depth + 1;
        int? maxDepth = this._maxDepth;
        int valueOrDefault = maxDepth.GetValueOrDefault();
        if ((num > valueOrDefault ? (maxDepth.HasValue ? 1 : 0) : 0) != 0 && !this._hasExceededMaxDepth)
        {
          this._hasExceededMaxDepth = true;
          throw JsonReaderException.Create(this, "The reader's MaxDepth of {0} has been exceeded.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._maxDepth));
        }
      }
    }

    private JsonContainerType Pop()
    {
      JsonPosition currentPosition;
      if (this._stack != null && this._stack.Count > 0)
      {
        currentPosition = this._currentPosition;
        this._currentPosition = this._stack[this._stack.Count - 1];
        this._stack.RemoveAt(this._stack.Count - 1);
      }
      else
      {
        currentPosition = this._currentPosition;
        this._currentPosition = new JsonPosition();
      }
      if (this._maxDepth.HasValue)
      {
        int depth = this.Depth;
        int? maxDepth = this._maxDepth;
        int valueOrDefault = maxDepth.GetValueOrDefault();
        if ((depth <= valueOrDefault ? (maxDepth.HasValue ? 1 : 0) : 0) != 0)
          this._hasExceededMaxDepth = false;
      }
      return currentPosition.Type;
    }

    private JsonContainerType Peek()
    {
      return this._currentPosition.Type;
    }

    /// <summary>Reads the next JSON token from the stream.</summary>
    /// <returns>true if the next token was read successfully; false if there are no more tokens to read.</returns>
    public abstract bool Read();

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public virtual int? ReadAsInt32()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new int?();
        case JsonToken.Integer:
        case JsonToken.Float:
          if (!(this.Value is int))
            this.SetToken(JsonToken.Integer, (object) Convert.ToInt32(this.Value, (IFormatProvider) CultureInfo.InvariantCulture), false);
          return new int?((int) this.Value);
        case JsonToken.String:
          return this.ReadInt32String((string) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal int? ReadInt32String(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new int?();
      }
      int result;
      if (int.TryParse(s, NumberStyles.Integer, (IFormatProvider) this.Culture, out result))
      {
        this.SetToken(JsonToken.Integer, (object) result, false);
        return new int?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to integer: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.String" />.
    /// </summary>
    /// <returns>A <see cref="T:System.String" />. This method will return <c>null</c> at the end of an array.</returns>
    public virtual string ReadAsString()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return (string) null;
        case JsonToken.String:
          return (string) this.Value;
        default:
          if (!JsonTokenUtils.IsPrimitiveToken(contentToken) || this.Value == null)
            throw JsonReaderException.Create(this, "Error reading string. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
          string str = !(this.Value is IFormattable) ? ((object) (this.Value as Uri) == null ? this.Value.ToString() : ((Uri) this.Value).OriginalString) : ((IFormattable) this.Value).ToString((string) null, (IFormatProvider) this.Culture);
          this.SetToken(JsonToken.String, (object) str, false);
          return str;
      }
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Byte" />[].
    /// </summary>
    /// <returns>A <see cref="T:System.Byte" />[] or a null reference if the next JSON token is null. This method will return <c>null</c> at the end of an array.</returns>
    public virtual byte[] ReadAsBytes()
    {
      JsonToken contentToken = this.GetContentToken();
      if (contentToken == JsonToken.None)
        return (byte[]) null;
      if (this.TokenType == JsonToken.StartObject)
      {
        this.ReadIntoWrappedTypeObject();
        byte[] numArray = this.ReadAsBytes();
        this.ReaderReadAndAssert();
        if (this.TokenType != JsonToken.EndObject)
          throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
        this.SetToken(JsonToken.Bytes, (object) numArray, false);
        return numArray;
      }
      switch (contentToken)
      {
        case JsonToken.StartArray:
          return this.ReadArrayIntoByteArray();
        case JsonToken.String:
          string s = (string) this.Value;
          Guid g;
          byte[] numArray1 = s.Length != 0 ? (!ConvertUtils.TryConvertGuid(s, out g) ? Convert.FromBase64String(s) : g.ToByteArray()) : new byte[0];
          this.SetToken(JsonToken.Bytes, (object) numArray1, false);
          return numArray1;
        case JsonToken.Null:
        case JsonToken.EndArray:
          return (byte[]) null;
        case JsonToken.Bytes:
          if (this.ValueType != typeof (Guid))
            return (byte[]) this.Value;
          byte[] byteArray = ((Guid) this.Value).ToByteArray();
          this.SetToken(JsonToken.Bytes, (object) byteArray, false);
          return byteArray;
        default:
          throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal byte[] ReadArrayIntoByteArray()
    {
      List<byte> byteList = new List<byte>();
      JsonToken contentToken;
      while (true)
      {
        contentToken = this.GetContentToken();
        switch (contentToken)
        {
          case JsonToken.None:
            goto label_2;
          case JsonToken.Integer:
            byteList.Add(Convert.ToByte(this.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            continue;
          case JsonToken.EndArray:
            goto label_4;
          default:
            goto label_5;
        }
      }
label_2:
      throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
label_4:
      byte[] array = byteList.ToArray();
      this.SetToken(JsonToken.Bytes, (object) array, false);
      return array;
label_5:
      throw JsonReaderException.Create(this, "Unexpected token when reading bytes: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public virtual double? ReadAsDouble()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new double?();
        case JsonToken.Integer:
        case JsonToken.Float:
          if (!(this.Value is double))
            this.SetToken(JsonToken.Float, (object) Convert.ToDouble(this.Value, (IFormatProvider) CultureInfo.InvariantCulture), false);
          return new double?((double) this.Value);
        case JsonToken.String:
          return this.ReadDoubleString((string) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading double. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal double? ReadDoubleString(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new double?();
      }
      double result;
      if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) this.Culture, out result))
      {
        this.SetToken(JsonToken.Float, (object) result, false);
        return new double?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to double: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public virtual bool? ReadAsBoolean()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new bool?();
        case JsonToken.Integer:
        case JsonToken.Float:
          bool boolean = Convert.ToBoolean(this.Value, (IFormatProvider) CultureInfo.InvariantCulture);
          this.SetToken(JsonToken.Boolean, (object) boolean, false);
          return new bool?(boolean);
        case JsonToken.String:
          return this.ReadBooleanString((string) this.Value);
        case JsonToken.Boolean:
          return new bool?((bool) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading boolean. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal bool? ReadBooleanString(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new bool?();
      }
      bool result;
      if (bool.TryParse(s, out result))
      {
        this.SetToken(JsonToken.Boolean, (object) result, false);
        return new bool?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to boolean: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public virtual Decimal? ReadAsDecimal()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new Decimal?();
        case JsonToken.Integer:
        case JsonToken.Float:
          if (!(this.Value is Decimal))
            this.SetToken(JsonToken.Float, (object) Convert.ToDecimal(this.Value, (IFormatProvider) CultureInfo.InvariantCulture), false);
          return new Decimal?((Decimal) this.Value);
        case JsonToken.String:
          return this.ReadDecimalString((string) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal Decimal? ReadDecimalString(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new Decimal?();
      }
      Decimal result;
      if (Decimal.TryParse(s, NumberStyles.Number, (IFormatProvider) this.Culture, out result))
      {
        this.SetToken(JsonToken.Float, (object) result, false);
        return new Decimal?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to decimal: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public virtual DateTime? ReadAsDateTime()
    {
      switch (this.GetContentToken())
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new DateTime?();
        case JsonToken.String:
          return this.ReadDateTimeString((string) this.Value);
        case JsonToken.Date:
          if (this.Value is DateTimeOffset)
            this.SetToken(JsonToken.Date, (object) ((DateTimeOffset) this.Value).DateTime, false);
          return new DateTime?((DateTime) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
      }
    }

    internal DateTime? ReadDateTimeString(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new DateTime?();
      }
      DateTime dateTime;
      if (DateTimeUtils.TryParseDateTime(s, this.DateTimeZoneHandling, this._dateFormatString, this.Culture, out dateTime))
      {
        dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
        this.SetToken(JsonToken.Date, (object) dateTime, false);
        return new DateTime?(dateTime);
      }
      if (!DateTime.TryParse(s, (IFormatProvider) this.Culture, DateTimeStyles.RoundtripKind, out dateTime))
        throw JsonReaderException.Create(this, "Could not convert string to DateTime: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
      dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
      this.SetToken(JsonToken.Date, (object) dateTime, false);
      return new DateTime?(dateTime);
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public virtual DateTimeOffset? ReadAsDateTimeOffset()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new DateTimeOffset?();
        case JsonToken.String:
          return this.ReadDateTimeOffsetString((string) this.Value);
        case JsonToken.Date:
          if (this.Value is DateTime)
            this.SetToken(JsonToken.Date, (object) new DateTimeOffset((DateTime) this.Value), false);
          return new DateTimeOffset?((DateTimeOffset) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal DateTimeOffset? ReadDateTimeOffsetString(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new DateTimeOffset?();
      }
      DateTimeOffset dateTimeOffset;
      if (DateTimeUtils.TryParseDateTimeOffset(s, this._dateFormatString, this.Culture, out dateTimeOffset))
      {
        this.SetToken(JsonToken.Date, (object) dateTimeOffset, false);
        return new DateTimeOffset?(dateTimeOffset);
      }
      if (DateTimeOffset.TryParse(s, (IFormatProvider) this.Culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
      {
        this.SetToken(JsonToken.Date, (object) dateTimeOffset, false);
        return new DateTimeOffset?(dateTimeOffset);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    internal void ReaderReadAndAssert()
    {
      if (!this.Read())
        throw this.CreateUnexpectedEndException();
    }

    internal JsonReaderException CreateUnexpectedEndException()
    {
      return JsonReaderException.Create(this, "Unexpected end when reading JSON.");
    }

    internal void ReadIntoWrappedTypeObject()
    {
      this.ReaderReadAndAssert();
      if (this.Value.ToString() == "$type")
      {
        this.ReaderReadAndAssert();
        if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
        {
          this.ReaderReadAndAssert();
          if (this.Value.ToString() == "$value")
            return;
        }
      }
      throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JsonToken.StartObject));
    }

    /// <summary>Skips the children of the current token.</summary>
    public void Skip()
    {
      if (this.TokenType == JsonToken.PropertyName)
        this.Read();
      if (!JsonTokenUtils.IsStartToken(this.TokenType))
        return;
      int depth = this.Depth;
      do
        ;
      while (this.Read() && depth < this.Depth);
    }

    /// <summary>Sets the current token.</summary>
    /// <param name="newToken">The new token.</param>
    protected void SetToken(JsonToken newToken)
    {
      this.SetToken(newToken, (object) null, true);
    }

    /// <summary>Sets the current token and value.</summary>
    /// <param name="newToken">The new token.</param>
    /// <param name="value">The value.</param>
    protected void SetToken(JsonToken newToken, object value)
    {
      this.SetToken(newToken, value, true);
    }

    internal void SetToken(JsonToken newToken, object value, bool updateIndex)
    {
      this._tokenType = newToken;
      this._value = value;
      switch (newToken)
      {
        case JsonToken.StartObject:
          this._currentState = JsonReader.State.ObjectStart;
          this.Push(JsonContainerType.Object);
          break;
        case JsonToken.StartArray:
          this._currentState = JsonReader.State.ArrayStart;
          this.Push(JsonContainerType.Array);
          break;
        case JsonToken.StartConstructor:
          this._currentState = JsonReader.State.ConstructorStart;
          this.Push(JsonContainerType.Constructor);
          break;
        case JsonToken.PropertyName:
          this._currentState = JsonReader.State.Property;
          this._currentPosition.PropertyName = (string) value;
          break;
        case JsonToken.Raw:
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Null:
        case JsonToken.Undefined:
        case JsonToken.Date:
        case JsonToken.Bytes:
          this.SetPostValueState(updateIndex);
          break;
        case JsonToken.EndObject:
          this.ValidateEnd(JsonToken.EndObject);
          break;
        case JsonToken.EndArray:
          this.ValidateEnd(JsonToken.EndArray);
          break;
        case JsonToken.EndConstructor:
          this.ValidateEnd(JsonToken.EndConstructor);
          break;
      }
    }

    internal void SetPostValueState(bool updateIndex)
    {
      if (this.Peek() != JsonContainerType.None)
        this._currentState = JsonReader.State.PostValue;
      else
        this.SetFinished();
      if (!updateIndex)
        return;
      this.UpdateScopeWithFinishedValue();
    }

    private void UpdateScopeWithFinishedValue()
    {
      if (!this._currentPosition.HasIndex)
        return;
      ++this._currentPosition.Position;
    }

    private void ValidateEnd(JsonToken endToken)
    {
      JsonContainerType jsonContainerType = this.Pop();
      if (this.GetTypeForCloseToken(endToken) != jsonContainerType)
        throw JsonReaderException.Create(this, "JsonToken {0} is not valid for closing JsonType {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) endToken, (object) jsonContainerType));
      if (this.Peek() != JsonContainerType.None)
        this._currentState = JsonReader.State.PostValue;
      else
        this.SetFinished();
    }

    /// <summary>Sets the state based on current token type.</summary>
    protected void SetStateBasedOnCurrent()
    {
      JsonContainerType jsonContainerType = this.Peek();
      switch (jsonContainerType)
      {
        case JsonContainerType.None:
          this.SetFinished();
          break;
        case JsonContainerType.Object:
          this._currentState = JsonReader.State.Object;
          break;
        case JsonContainerType.Array:
          this._currentState = JsonReader.State.Array;
          break;
        case JsonContainerType.Constructor:
          this._currentState = JsonReader.State.Constructor;
          break;
        default:
          throw JsonReaderException.Create(this, "While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jsonContainerType));
      }
    }

    private void SetFinished()
    {
      if (this.SupportMultipleContent)
        this._currentState = JsonReader.State.Start;
      else
        this._currentState = JsonReader.State.Finished;
    }

    private JsonContainerType GetTypeForCloseToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.EndObject:
          return JsonContainerType.Object;
        case JsonToken.EndArray:
          return JsonContainerType.Array;
        case JsonToken.EndConstructor:
          return JsonContainerType.Constructor;
        default:
          throw JsonReaderException.Create(this, "Not a valid close JsonToken: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) token));
      }
    }

    void IDisposable.Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!(this._currentState != JsonReader.State.Closed & disposing))
        return;
      this.Close();
    }

    /// <summary>
    /// Changes the <see cref="T:Newtonsoft.Json.JsonReader.State" /> to Closed.
    /// </summary>
    public virtual void Close()
    {
      this._currentState = JsonReader.State.Closed;
      this._tokenType = JsonToken.None;
      this._value = (object) null;
    }

    internal void ReadAndAssert()
    {
      if (!this.Read())
        throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
    }

    internal bool ReadAndMoveToContent()
    {
      return this.Read() && this.MoveToContent();
    }

    internal bool MoveToContent()
    {
      for (JsonToken tokenType = this.TokenType; tokenType == JsonToken.None || tokenType == JsonToken.Comment; tokenType = this.TokenType)
      {
        if (!this.Read())
          return false;
      }
      return true;
    }

    private JsonToken GetContentToken()
    {
      while (this.Read())
      {
        JsonToken tokenType = this.TokenType;
        if (tokenType != JsonToken.Comment)
          return tokenType;
      }
      this.SetToken(JsonToken.None);
      return JsonToken.None;
    }

    /// <summary>Specifies the state of the reader.</summary>
    protected internal enum State
    {
      /// <summary>The Read method has not been called.</summary>
      Start,
      /// <summary>The end of the file has been reached successfully.</summary>
      Complete,
      /// <summary>Reader is at a property.</summary>
      Property,
      /// <summary>Reader is at the start of an object.</summary>
      ObjectStart,
      /// <summary>Reader is in an object.</summary>
      Object,
      /// <summary>Reader is at the start of an array.</summary>
      ArrayStart,
      /// <summary>Reader is in an array.</summary>
      Array,
      /// <summary>The Close method has been called.</summary>
      Closed,
      /// <summary>Reader has just read a value.</summary>
      PostValue,
      /// <summary>Reader is at the start of a constructor.</summary>
      ConstructorStart,
      /// <summary>Reader in a constructor.</summary>
      Constructor,
      /// <summary>
      /// An error occurred that prevents the read operation from continuing.
      /// </summary>
      Error,
      /// <summary>The end of the file has been reached successfully.</summary>
      Finished,
    }
  }
}
