// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JToken
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq.JsonPath;
using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Represents an abstract JSON token.</summary>
  [Preserve]
  public abstract class JToken : IJEnumerable<JToken>, IEnumerable<JToken>, IEnumerable, IJsonLineInfo, ICloneable
  {
    private static readonly JTokenType[] BooleanTypes = new JTokenType[6]
    {
      JTokenType.Integer,
      JTokenType.Float,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Boolean
    };
    private static readonly JTokenType[] NumberTypes = new JTokenType[6]
    {
      JTokenType.Integer,
      JTokenType.Float,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Boolean
    };
    private static readonly JTokenType[] StringTypes = new JTokenType[11]
    {
      JTokenType.Date,
      JTokenType.Integer,
      JTokenType.Float,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Boolean,
      JTokenType.Bytes,
      JTokenType.Guid,
      JTokenType.TimeSpan,
      JTokenType.Uri
    };
    private static readonly JTokenType[] GuidTypes = new JTokenType[5]
    {
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Guid,
      JTokenType.Bytes
    };
    private static readonly JTokenType[] TimeSpanTypes = new JTokenType[4]
    {
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.TimeSpan
    };
    private static readonly JTokenType[] UriTypes = new JTokenType[4]
    {
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Uri
    };
    private static readonly JTokenType[] CharTypes = new JTokenType[5]
    {
      JTokenType.Integer,
      JTokenType.Float,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw
    };
    private static readonly JTokenType[] DateTimeTypes = new JTokenType[4]
    {
      JTokenType.Date,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw
    };
    private static readonly JTokenType[] BytesTypes = new JTokenType[5]
    {
      JTokenType.Bytes,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Integer
    };
    private static JTokenEqualityComparer _equalityComparer;
    private JContainer _parent;
    private JToken _previous;
    private JToken _next;
    private object _annotations;

    /// <summary>
    /// Gets a comparer that can compare two tokens for value equality.
    /// </summary>
    /// <value>A <see cref="T:Newtonsoft.Json.Linq.JTokenEqualityComparer" /> that can compare two nodes for value equality.</value>
    public static JTokenEqualityComparer EqualityComparer
    {
      get
      {
        if (JToken._equalityComparer == null)
          JToken._equalityComparer = new JTokenEqualityComparer();
        return JToken._equalityComparer;
      }
    }

    /// <summary>Gets or sets the parent.</summary>
    /// <value>The parent.</value>
    public JContainer Parent
    {
      [DebuggerStepThrough] get
      {
        return this._parent;
      }
      internal set
      {
        this._parent = value;
      }
    }

    /// <summary>
    /// Gets the root <see cref="T:Newtonsoft.Json.Linq.JToken" /> of this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <value>The root <see cref="T:Newtonsoft.Json.Linq.JToken" /> of this <see cref="T:Newtonsoft.Json.Linq.JToken" />.</value>
    public JToken Root
    {
      get
      {
        JContainer parent = this.Parent;
        if (parent == null)
          return this;
        while (parent.Parent != null)
          parent = parent.Parent;
        return (JToken) parent;
      }
    }

    internal abstract JToken CloneToken();

    internal abstract bool DeepEquals(JToken node);

    /// <summary>
    /// Gets the node type for this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <value>The type.</value>
    public abstract JTokenType Type { get; }

    /// <summary>
    /// Gets a value indicating whether this token has child tokens.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this token has child values; otherwise, <c>false</c>.
    /// </value>
    public abstract bool HasValues { get; }

    /// <summary>
    /// Compares the values of two tokens, including the values of all descendant tokens.
    /// </summary>
    /// <param name="t1">The first <see cref="T:Newtonsoft.Json.Linq.JToken" /> to compare.</param>
    /// <param name="t2">The second <see cref="T:Newtonsoft.Json.Linq.JToken" /> to compare.</param>
    /// <returns>true if the tokens are equal; otherwise false.</returns>
    public static bool DeepEquals(JToken t1, JToken t2)
    {
      if (t1 == t2)
        return true;
      return t1 != null && t2 != null && t1.DeepEquals(t2);
    }

    /// <summary>Gets the next sibling token of this node.</summary>
    /// <value>The <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the next sibling token.</value>
    public JToken Next
    {
      get
      {
        return this._next;
      }
      internal set
      {
        this._next = value;
      }
    }

    /// <summary>Gets the previous sibling token of this node.</summary>
    /// <value>The <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the previous sibling token.</value>
    public JToken Previous
    {
      get
      {
        return this._previous;
      }
      internal set
      {
        this._previous = value;
      }
    }

    /// <summary>Gets the path of the JSON token.</summary>
    public string Path
    {
      get
      {
        if (this.Parent == null)
          return string.Empty;
        List<JsonPosition> positions = new List<JsonPosition>();
        JToken jtoken1 = (JToken) null;
        for (JToken jtoken2 = this; jtoken2 != null; jtoken2 = (JToken) jtoken2.Parent)
        {
          JsonPosition jsonPosition1;
          switch (jtoken2.Type)
          {
            case JTokenType.Array:
            case JTokenType.Constructor:
              if (jtoken1 != null)
              {
                int num = ((IList<JToken>) jtoken2).IndexOf(jtoken1);
                List<JsonPosition> jsonPositionList = positions;
                jsonPosition1 = new JsonPosition(JsonContainerType.Array);
                jsonPosition1.Position = num;
                JsonPosition jsonPosition2 = jsonPosition1;
                jsonPositionList.Add(jsonPosition2);
                break;
              }
              break;
            case JTokenType.Property:
              JProperty jproperty = (JProperty) jtoken2;
              List<JsonPosition> jsonPositionList1 = positions;
              jsonPosition1 = new JsonPosition(JsonContainerType.Object);
              jsonPosition1.PropertyName = jproperty.Name;
              JsonPosition jsonPosition3 = jsonPosition1;
              jsonPositionList1.Add(jsonPosition3);
              break;
          }
          jtoken1 = jtoken2;
        }
        positions.Reverse();
        return JsonPosition.BuildPath(positions, new JsonPosition?());
      }
    }

    internal JToken()
    {
    }

    /// <summary>
    /// Adds the specified content immediately after this token.
    /// </summary>
    /// <param name="content">A content object that contains simple content or a collection of content objects to be added after this token.</param>
    public void AddAfterSelf(object content)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.AddInternal(this._parent.IndexOfItem(this) + 1, content, false);
    }

    /// <summary>
    /// Adds the specified content immediately before this token.
    /// </summary>
    /// <param name="content">A content object that contains simple content or a collection of content objects to be added before this token.</param>
    public void AddBeforeSelf(object content)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.AddInternal(this._parent.IndexOfItem(this), content, false);
    }

    /// <summary>
    /// Returns a collection of the ancestor tokens of this token.
    /// </summary>
    /// <returns>A collection of the ancestor tokens of this token.</returns>
    public IEnumerable<JToken> Ancestors()
    {
      return this.GetAncestors(false);
    }

    /// <summary>
    /// Returns a collection of tokens that contain this token, and the ancestors of this token.
    /// </summary>
    /// <returns>A collection of tokens that contain this token, and the ancestors of this token.</returns>
    public IEnumerable<JToken> AncestorsAndSelf()
    {
      return this.GetAncestors(true);
    }

    internal IEnumerable<JToken> GetAncestors(bool self)
    {
      JToken current;
      for (current = self ? this : (JToken) this.Parent; current != null; current = (JToken) current.Parent)
        yield return current;
      current = (JToken) null;
    }

    /// <summary>
    /// Returns a collection of the sibling tokens after this token, in document order.
    /// </summary>
    /// <returns>A collection of the sibling tokens after this tokens, in document order.</returns>
    public IEnumerable<JToken> AfterSelf()
    {
      if (this.Parent != null)
      {
        JToken o;
        for (o = this.Next; o != null; o = o.Next)
          yield return o;
        o = (JToken) null;
      }
    }

    /// <summary>
    /// Returns a collection of the sibling tokens before this token, in document order.
    /// </summary>
    /// <returns>A collection of the sibling tokens before this token, in document order.</returns>
    public IEnumerable<JToken> BeforeSelf()
    {
      JToken o;
      for (o = this.Parent.First; o != this; o = o.Next)
        yield return o;
      o = (JToken) null;
    }

    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified key.
    /// </summary>
    /// <value>The <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified key.</value>
    public virtual JToken this[object key]
    {
      get
      {
        throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
      set
      {
        throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
    }

    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified key converted to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to convert the token to.</typeparam>
    /// <param name="key">The token key.</param>
    /// <returns>The converted token value.</returns>
    public virtual T Value<T>(object key)
    {
      JToken token = this[key];
      return token != null ? token.Convert<JToken, T>() : default (T);
    }

    /// <summary>Get the first child token of this token.</summary>
    /// <value>A <see cref="T:Newtonsoft.Json.Linq.JToken" /> containing the first child token of the <see cref="T:Newtonsoft.Json.Linq.JToken" />.</value>
    public virtual JToken First
    {
      get
      {
        throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
    }

    /// <summary>Get the last child token of this token.</summary>
    /// <value>A <see cref="T:Newtonsoft.Json.Linq.JToken" /> containing the last child token of the <see cref="T:Newtonsoft.Json.Linq.JToken" />.</value>
    public virtual JToken Last
    {
      get
      {
        throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
    }

    /// <summary>
    /// Returns a collection of the child tokens of this token, in document order.
    /// </summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> containing the child tokens of this <see cref="T:Newtonsoft.Json.Linq.JToken" />, in document order.</returns>
    public virtual JEnumerable<JToken> Children()
    {
      return JEnumerable<JToken>.Empty;
    }

    /// <summary>
    /// Returns a collection of the child tokens of this token, in document order, filtered by the specified type.
    /// </summary>
    /// <typeparam name="T">The type to filter the child tokens on.</typeparam>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JEnumerable`1" /> containing the child tokens of this <see cref="T:Newtonsoft.Json.Linq.JToken" />, in document order.</returns>
    public JEnumerable<T> Children<T>() where T : JToken
    {
      return new JEnumerable<T>(this.Children().OfType<T>());
    }

    /// <summary>
    /// Returns a collection of the child values of this token, in document order.
    /// </summary>
    /// <typeparam name="T">The type to convert the values to.</typeparam>
    /// <returns>A <see cref="T:System.Collections.Generic.IEnumerable`1" /> containing the child values of this <see cref="T:Newtonsoft.Json.Linq.JToken" />, in document order.</returns>
    public virtual IEnumerable<T> Values<T>()
    {
      throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
    }

    /// <summary>Removes this token from its parent.</summary>
    public void Remove()
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.RemoveItem(this);
    }

    /// <summary>Replaces this token with the specified token.</summary>
    /// <param name="value">The value.</param>
    public void Replace(JToken value)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.ReplaceItem(this, value);
    }

    /// <summary>
    /// Writes this token to a <see cref="T:Newtonsoft.Json.JsonWriter" />.
    /// </summary>
    /// <param name="writer">A <see cref="T:Newtonsoft.Json.JsonWriter" /> into which this method will write.</param>
    /// <param name="converters">A collection of <see cref="T:Newtonsoft.Json.JsonConverter" /> which will be used when writing the token.</param>
    public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

    /// <summary>Returns the indented JSON for this token.</summary>
    /// <returns>The indented JSON for this token.</returns>
    public override string ToString()
    {
      return this.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Returns the JSON for this token using the given formatting and converters.
    /// </summary>
    /// <param name="formatting">Indicates how the output is formatted.</param>
    /// <param name="converters">A collection of <see cref="T:Newtonsoft.Json.JsonConverter" /> which will be used when writing the token.</param>
    /// <returns>The JSON for this token using the given formatting and converters.</returns>
    public string ToString(Formatting formatting, params JsonConverter[] converters)
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter);
        jsonTextWriter.Formatting = formatting;
        this.WriteTo((JsonWriter) jsonTextWriter, converters);
        return stringWriter.ToString();
      }
    }

    private static JValue EnsureValue(JToken value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (value is JProperty)
        value = ((JProperty) value).Value;
      return value as JValue;
    }

    private static string GetType(JToken token)
    {
      ValidationUtils.ArgumentNotNull((object) token, nameof (token));
      if (token is JProperty)
        token = ((JProperty) token).Value;
      return token.Type.ToString();
    }

    private static bool ValidateToken(JToken o, JTokenType[] validTypes, bool nullable)
    {
      if (Array.IndexOf<JTokenType>(validTypes, o.Type) != -1)
        return true;
      if (!nullable)
        return false;
      return o.Type == JTokenType.Null || o.Type == JTokenType.Undefined;
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Boolean" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator bool(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.BooleanTypes, false))
        throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToBoolean(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.DateTimeOffset" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator DateTimeOffset(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.DateTimeTypes, false))
        throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value is DateTimeOffset)
        return (DateTimeOffset) jvalue.Value;
      return jvalue.Value is string ? DateTimeOffset.Parse((string) jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture) : new DateTimeOffset(Convert.ToDateTime(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator bool?(JToken value)
    {
      if (value == null)
        return new bool?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.BooleanTypes, true))
        throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new bool?() : new bool?(Convert.ToBoolean(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Int64" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator long(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Int64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToInt64(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator DateTime?(JToken value)
    {
      if (value == null)
        return new DateTime?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.DateTimeTypes, true))
        throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value is DateTimeOffset)
        return new DateTime?(((DateTimeOffset) jvalue.Value).DateTime);
      return jvalue.Value == null ? new DateTime?() : new DateTime?(Convert.ToDateTime(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator DateTimeOffset?(JToken value)
    {
      if (value == null)
        return new DateTimeOffset?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.DateTimeTypes, true))
        throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new DateTimeOffset?();
      if (jvalue.Value is DateTimeOffset)
        return (DateTimeOffset?) jvalue.Value;
      return jvalue.Value is string ? new DateTimeOffset?(DateTimeOffset.Parse((string) jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture)) : new DateTimeOffset?(new DateTimeOffset(Convert.ToDateTime(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture)));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Decimal?(JToken value)
    {
      if (value == null)
        return new Decimal?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new Decimal?() : new Decimal?(Convert.ToDecimal(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator double?(JToken value)
    {
      if (value == null)
        return new double?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Double.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new double?() : new double?(Convert.ToDouble(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator char?(JToken value)
    {
      if (value == null)
        return new char?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.CharTypes, true))
        throw new ArgumentException("Can not convert {0} to Char.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new char?() : new char?(Convert.ToChar(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Int32" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator int(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Int32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToInt32(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Int16" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator short(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Int16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToInt16(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.UInt16" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    [CLSCompliant(false)]
    public static explicit operator ushort(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToUInt16(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Char" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    [CLSCompliant(false)]
    public static explicit operator char(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.CharTypes, false))
        throw new ArgumentException("Can not convert {0} to Char.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToChar(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Byte" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator byte(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Byte.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToByte(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.SByte" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    [CLSCompliant(false)]
    public static explicit operator sbyte(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to SByte.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToSByte(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator int?(JToken value)
    {
      if (value == null)
        return new int?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Int32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new int?() : new int?(Convert.ToInt32(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator short?(JToken value)
    {
      if (value == null)
        return new short?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Int16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new short?() : new short?(Convert.ToInt16(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    [CLSCompliant(false)]
    public static explicit operator ushort?(JToken value)
    {
      if (value == null)
        return new ushort?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new ushort?() : new ushort?(Convert.ToUInt16(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator byte?(JToken value)
    {
      if (value == null)
        return new byte?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Byte.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new byte?() : new byte?(Convert.ToByte(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    [CLSCompliant(false)]
    public static explicit operator sbyte?(JToken value)
    {
      if (value == null)
        return new sbyte?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to SByte.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new sbyte?() : new sbyte?(Convert.ToSByte(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.DateTime" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator DateTime(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.DateTimeTypes, false))
        throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value is DateTimeOffset ? ((DateTimeOffset) jvalue.Value).DateTime : Convert.ToDateTime(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator long?(JToken value)
    {
      if (value == null)
        return new long?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Int64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new long?() : new long?(Convert.ToInt64(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator float?(JToken value)
    {
      if (value == null)
        return new float?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Single.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new float?() : new float?(Convert.ToSingle(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Decimal" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Decimal(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToDecimal(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    [CLSCompliant(false)]
    public static explicit operator uint?(JToken value)
    {
      if (value == null)
        return new uint?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new uint?() : new uint?(Convert.ToUInt32(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    [CLSCompliant(false)]
    public static explicit operator ulong?(JToken value)
    {
      if (value == null)
        return new ulong?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new ulong?() : new ulong?(Convert.ToUInt64(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Double" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator double(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Double.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToDouble(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Single" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator float(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Single.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToSingle(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.String" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator string(JToken value)
    {
      if (value == null)
        return (string) null;
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.StringTypes, true))
        throw new ArgumentException("Can not convert {0} to String.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return (string) null;
      return jvalue.Value is byte[] ? Convert.ToBase64String((byte[]) jvalue.Value) : Convert.ToString(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.UInt32" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    [CLSCompliant(false)]
    public static explicit operator uint(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToUInt32(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.UInt64" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    [CLSCompliant(false)]
    public static explicit operator ulong(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return Convert.ToUInt64(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Byte" />[].
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator byte[](JToken value)
    {
      if (value == null)
        return (byte[]) null;
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.BytesTypes, false))
        throw new ArgumentException("Can not convert {0} to byte array.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value is string)
        return Convert.FromBase64String(Convert.ToString(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
      if (jvalue.Value is byte[])
        return (byte[]) jvalue.Value;
      throw new ArgumentException("Can not convert {0} to byte array.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Guid" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Guid(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.GuidTypes, false))
        throw new ArgumentException("Can not convert {0} to Guid.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value is byte[])
        return new Guid((byte[]) jvalue.Value);
      return !(jvalue.Value is Guid) ? new Guid(Convert.ToString(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture)) : (Guid) jvalue.Value;
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Guid" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Guid?(JToken value)
    {
      if (value == null)
        return new Guid?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.GuidTypes, true))
        throw new ArgumentException("Can not convert {0} to Guid.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new Guid?();
      return jvalue.Value is byte[] ? new Guid?(new Guid((byte[]) jvalue.Value)) : new Guid?(jvalue.Value is Guid ? (Guid) jvalue.Value : new Guid(Convert.ToString(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture)));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.TimeSpan" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator TimeSpan(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.TimeSpanTypes, false))
        throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return !(jvalue.Value is TimeSpan) ? ConvertUtils.ParseTimeSpan(Convert.ToString(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture)) : (TimeSpan) jvalue.Value;
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.TimeSpan" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator TimeSpan?(JToken value)
    {
      if (value == null)
        return new TimeSpan?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.TimeSpanTypes, true))
        throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return jvalue.Value == null ? new TimeSpan?() : new TimeSpan?(jvalue.Value is TimeSpan ? (TimeSpan) jvalue.Value : ConvertUtils.ParseTimeSpan(Convert.ToString(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture)));
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="T:Newtonsoft.Json.Linq.JToken" /> to <see cref="T:System.Uri" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator Uri(JToken value)
    {
      if (value == null)
        return (Uri) null;
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateToken((JToken) jvalue, JToken.UriTypes, true))
        throw new ArgumentException("Can not convert {0} to Uri.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return (Uri) null;
      return (object) (jvalue.Value as Uri) == null ? new Uri(Convert.ToString(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture)) : (Uri) jvalue.Value;
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Boolean" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(bool value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.DateTimeOffset" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(DateTimeOffset value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Byte" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(byte value)
    {
      return (JToken) new JValue((long) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(byte? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.SByte" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    [CLSCompliant(false)]
    public static implicit operator JToken(sbyte value)
    {
      return (JToken) new JValue((long) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    [CLSCompliant(false)]
    public static implicit operator JToken(sbyte? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(bool? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(long value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(DateTime? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(DateTimeOffset? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(Decimal? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(double? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Int16" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    [CLSCompliant(false)]
    public static implicit operator JToken(short value)
    {
      return (JToken) new JValue((long) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.UInt16" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    [CLSCompliant(false)]
    public static implicit operator JToken(ushort value)
    {
      return (JToken) new JValue((long) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Int32" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(int value)
    {
      return (JToken) new JValue((long) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(int? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.DateTime" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(DateTime value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(long? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(float? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Decimal" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(Decimal value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    [CLSCompliant(false)]
    public static implicit operator JToken(short? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    [CLSCompliant(false)]
    public static implicit operator JToken(ushort? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    [CLSCompliant(false)]
    public static implicit operator JToken(uint? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    [CLSCompliant(false)]
    public static implicit operator JToken(ulong? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Double" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(double value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Single" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(float value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.String" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(string value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.UInt32" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    [CLSCompliant(false)]
    public static implicit operator JToken(uint value)
    {
      return (JToken) new JValue((long) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.UInt64" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    [CLSCompliant(false)]
    public static implicit operator JToken(ulong value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Byte" />[] to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(byte[] value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Uri" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(Uri value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.TimeSpan" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(TimeSpan value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(TimeSpan? value)
    {
      return (JToken) new JValue((object) value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Guid" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(Guid value)
    {
      return (JToken) new JValue(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:System.Nullable`1" /> to <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="value">The value to create a <see cref="T:Newtonsoft.Json.Linq.JValue" /> from.</param>
    /// <returns>The <see cref="T:Newtonsoft.Json.Linq.JValue" /> initialized with the specified value.</returns>
    public static implicit operator JToken(Guid? value)
    {
      return (JToken) new JValue((object) value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) ((IEnumerable<JToken>) this).GetEnumerator();
    }

    IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
    {
      return this.Children().GetEnumerator();
    }

    internal abstract int GetDeepHashCode();

    IJEnumerable<JToken> IJEnumerable<JToken>.this[object key]
    {
      get
      {
        return (IJEnumerable<JToken>) this[key];
      }
    }

    /// <summary>
    /// Creates an <see cref="T:Newtonsoft.Json.JsonReader" /> for this token.
    /// </summary>
    /// <returns>An <see cref="T:Newtonsoft.Json.JsonReader" /> that can be used to read this token and its descendants.</returns>
    public JsonReader CreateReader()
    {
      return (JsonReader) new JTokenReader(this);
    }

    internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
    {
      ValidationUtils.ArgumentNotNull(o, nameof (o));
      ValidationUtils.ArgumentNotNull((object) jsonSerializer, nameof (jsonSerializer));
      using (JTokenWriter jtokenWriter = new JTokenWriter())
      {
        jsonSerializer.Serialize((JsonWriter) jtokenWriter, o);
        return jtokenWriter.Token;
      }
    }

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JToken" /> from an object.
    /// </summary>
    /// <param name="o">The object that will be used to create <see cref="T:Newtonsoft.Json.Linq.JToken" />.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the value of the specified object</returns>
    public static JToken FromObject(object o)
    {
      return JToken.FromObjectInternal(o, JsonSerializer.CreateDefault());
    }

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JToken" /> from an object using the specified <see cref="T:Newtonsoft.Json.JsonSerializer" />.
    /// </summary>
    /// <param name="o">The object that will be used to create <see cref="T:Newtonsoft.Json.Linq.JToken" />.</param>
    /// <param name="jsonSerializer">The <see cref="T:Newtonsoft.Json.JsonSerializer" /> that will be used when reading the object.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the value of the specified object</returns>
    public static JToken FromObject(object o, JsonSerializer jsonSerializer)
    {
      return JToken.FromObjectInternal(o, jsonSerializer);
    }

    /// <summary>
    /// Creates the specified .NET type from the <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <typeparam name="T">The object type that the token will be deserialized to.</typeparam>
    /// <returns>The new object created from the JSON value.</returns>
    public T ToObject<T>()
    {
      return (T) this.ToObject(typeof (T));
    }

    /// <summary>
    /// Creates the specified .NET type from the <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="objectType">The object type that the token will be deserialized to.</param>
    /// <returns>The new object created from the JSON value.</returns>
    public object ToObject(System.Type objectType)
    {
      if (JsonConvert.DefaultSettings == null)
      {
        bool isEnum;
        PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(objectType, out isEnum);
        if (isEnum)
        {
          if (this.Type == JTokenType.String)
          {
            try
            {
              return this.ToObject(objectType, JsonSerializer.CreateDefault());
            }
            catch (Exception ex)
            {
              throw new ArgumentException("Could not convert '{0}' to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) (string) this, (object) (objectType.IsEnum() ? (MemberInfo) objectType : (MemberInfo) Nullable.GetUnderlyingType(objectType)).Name), ex);
            }
          }
          else if (this.Type == JTokenType.Integer)
            return Enum.ToObject(objectType.IsEnum() ? objectType : Nullable.GetUnderlyingType(objectType), ((JValue) this).Value);
        }
        switch (typeCode)
        {
          case PrimitiveTypeCode.Char:
            return (object) (char) this;
          case PrimitiveTypeCode.CharNullable:
            return (object) (char?) this;
          case PrimitiveTypeCode.Boolean:
            return (object) (bool) this;
          case PrimitiveTypeCode.BooleanNullable:
            return (object) (bool?) this;
          case PrimitiveTypeCode.SByte:
            return (object) (sbyte?) this;
          case PrimitiveTypeCode.SByteNullable:
            return (object) (sbyte) this;
          case PrimitiveTypeCode.Int16:
            return (object) (short) this;
          case PrimitiveTypeCode.Int16Nullable:
            return (object) (short?) this;
          case PrimitiveTypeCode.UInt16:
            return (object) (ushort) this;
          case PrimitiveTypeCode.UInt16Nullable:
            return (object) (ushort?) this;
          case PrimitiveTypeCode.Int32:
            return (object) (int) this;
          case PrimitiveTypeCode.Int32Nullable:
            return (object) (int?) this;
          case PrimitiveTypeCode.Byte:
            return (object) (byte) this;
          case PrimitiveTypeCode.ByteNullable:
            return (object) (byte?) this;
          case PrimitiveTypeCode.UInt32:
            return (object) (uint) this;
          case PrimitiveTypeCode.UInt32Nullable:
            return (object) (uint?) this;
          case PrimitiveTypeCode.Int64:
            return (object) (long) this;
          case PrimitiveTypeCode.Int64Nullable:
            return (object) (long?) this;
          case PrimitiveTypeCode.UInt64:
            return (object) (ulong) this;
          case PrimitiveTypeCode.UInt64Nullable:
            return (object) (ulong?) this;
          case PrimitiveTypeCode.Single:
            return (object) (float) this;
          case PrimitiveTypeCode.SingleNullable:
            return (object) (float?) this;
          case PrimitiveTypeCode.Double:
            return (object) (double) this;
          case PrimitiveTypeCode.DoubleNullable:
            return (object) (double?) this;
          case PrimitiveTypeCode.DateTime:
            return (object) (DateTime) this;
          case PrimitiveTypeCode.DateTimeNullable:
            return (object) (DateTime?) this;
          case PrimitiveTypeCode.DateTimeOffset:
            return (object) (DateTimeOffset) this;
          case PrimitiveTypeCode.DateTimeOffsetNullable:
            return (object) (DateTimeOffset?) this;
          case PrimitiveTypeCode.Decimal:
            return (object) (Decimal) this;
          case PrimitiveTypeCode.DecimalNullable:
            return (object) (Decimal?) this;
          case PrimitiveTypeCode.Guid:
            return (object) (Guid) this;
          case PrimitiveTypeCode.GuidNullable:
            return (object) (Guid?) this;
          case PrimitiveTypeCode.TimeSpan:
            return (object) (TimeSpan) this;
          case PrimitiveTypeCode.TimeSpanNullable:
            return (object) (TimeSpan?) this;
          case PrimitiveTypeCode.Uri:
            return (object) (Uri) this;
          case PrimitiveTypeCode.String:
            return (object) (string) this;
        }
      }
      return this.ToObject(objectType, JsonSerializer.CreateDefault());
    }

    /// <summary>
    /// Creates the specified .NET type from the <see cref="T:Newtonsoft.Json.Linq.JToken" /> using the specified <see cref="T:Newtonsoft.Json.JsonSerializer" />.
    /// </summary>
    /// <typeparam name="T">The object type that the token will be deserialized to.</typeparam>
    /// <param name="jsonSerializer">The <see cref="T:Newtonsoft.Json.JsonSerializer" /> that will be used when creating the object.</param>
    /// <returns>The new object created from the JSON value.</returns>
    public T ToObject<T>(JsonSerializer jsonSerializer)
    {
      return (T) this.ToObject(typeof (T), jsonSerializer);
    }

    /// <summary>
    /// Creates the specified .NET type from the <see cref="T:Newtonsoft.Json.Linq.JToken" /> using the specified <see cref="T:Newtonsoft.Json.JsonSerializer" />.
    /// </summary>
    /// <param name="objectType">The object type that the token will be deserialized to.</param>
    /// <param name="jsonSerializer">The <see cref="T:Newtonsoft.Json.JsonSerializer" /> that will be used when creating the object.</param>
    /// <returns>The new object created from the JSON value.</returns>
    public object ToObject(System.Type objectType, JsonSerializer jsonSerializer)
    {
      ValidationUtils.ArgumentNotNull((object) jsonSerializer, nameof (jsonSerializer));
      using (JTokenReader jtokenReader = new JTokenReader(this))
        return jsonSerializer.Deserialize((JsonReader) jtokenReader, objectType);
    }

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JToken" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">An <see cref="T:Newtonsoft.Json.JsonReader" /> positioned at the token to read into this <see cref="T:Newtonsoft.Json.Linq.JToken" />.</param>
    /// <returns>
    /// An <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the token and its descendant tokens
    /// that were read from the reader. The runtime type of the token is determined
    /// by the token type of the first token encountered in the reader.
    /// </returns>
    public static JToken ReadFrom(JsonReader reader)
    {
      return JToken.ReadFrom(reader, (JsonLoadSettings) null);
    }

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JToken" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">An <see cref="T:Newtonsoft.Json.JsonReader" /> positioned at the token to read into this <see cref="T:Newtonsoft.Json.Linq.JToken" />.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is null, default load settings will be used.</param>
    /// <returns>
    /// An <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the token and its descendant tokens
    /// that were read from the reader. The runtime type of the token is determined
    /// by the token type of the first token encountered in the reader.
    /// </returns>
    public static JToken ReadFrom(JsonReader reader, JsonLoadSettings settings)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      if (reader.TokenType == JsonToken.None && (settings == null || settings.CommentHandling != CommentHandling.Ignore ? (reader.Read() ? 1 : 0) : (reader.ReadAndMoveToContent() ? 1 : 0)) == 0)
        throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader.");
      IJsonLineInfo lineInfo = reader as IJsonLineInfo;
      switch (reader.TokenType)
      {
        case JsonToken.StartObject:
          return (JToken) JObject.Load(reader, settings);
        case JsonToken.StartArray:
          return (JToken) JArray.Load(reader, settings);
        case JsonToken.StartConstructor:
          return (JToken) JConstructor.Load(reader, settings);
        case JsonToken.PropertyName:
          return (JToken) JProperty.Load(reader, settings);
        case JsonToken.Comment:
          JValue comment = JValue.CreateComment(reader.Value.ToString());
          comment.SetLineInfo(lineInfo, settings);
          return (JToken) comment;
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Date:
        case JsonToken.Bytes:
          JValue jvalue1 = new JValue(reader.Value);
          jvalue1.SetLineInfo(lineInfo, settings);
          return (JToken) jvalue1;
        case JsonToken.Null:
          JValue jvalue2 = JValue.CreateNull();
          jvalue2.SetLineInfo(lineInfo, settings);
          return (JToken) jvalue2;
        case JsonToken.Undefined:
          JValue undefined = JValue.CreateUndefined();
          undefined.SetLineInfo(lineInfo, settings);
          return (JToken) undefined;
        default:
          throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      }
    }

    /// <summary>
    /// Load a <see cref="T:Newtonsoft.Json.Linq.JToken" /> from a string that contains JSON.
    /// </summary>
    /// <param name="json">A <see cref="T:System.String" /> that contains JSON.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JToken" /> populated from the string that contains JSON.</returns>
    public static JToken Parse(string json)
    {
      return JToken.Parse(json, (JsonLoadSettings) null);
    }

    /// <summary>
    /// Load a <see cref="T:Newtonsoft.Json.Linq.JToken" /> from a string that contains JSON.
    /// </summary>
    /// <param name="json">A <see cref="T:System.String" /> that contains JSON.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is null, default load settings will be used.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JToken" /> populated from the string that contains JSON.</returns>
    public static JToken Parse(string json, JsonLoadSettings settings)
    {
      using (JsonReader reader = (JsonReader) new JsonTextReader((TextReader) new StringReader(json)))
      {
        JToken jtoken = JToken.Load(reader, settings);
        if (reader.Read() && reader.TokenType != JsonToken.Comment)
          throw JsonReaderException.Create(reader, "Additional text found in JSON string after parsing content.");
        return jtoken;
      }
    }

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JToken" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">An <see cref="T:Newtonsoft.Json.JsonReader" /> positioned at the token to read into this <see cref="T:Newtonsoft.Json.Linq.JToken" />.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is null, default load settings will be used.</param>
    /// <returns>
    /// An <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the token and its descendant tokens
    /// that were read from the reader. The runtime type of the token is determined
    /// by the token type of the first token encountered in the reader.
    /// </returns>
    public static JToken Load(JsonReader reader, JsonLoadSettings settings)
    {
      return JToken.ReadFrom(reader, settings);
    }

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JToken" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">An <see cref="T:Newtonsoft.Json.JsonReader" /> positioned at the token to read into this <see cref="T:Newtonsoft.Json.Linq.JToken" />.</param>
    /// <returns>
    /// An <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the token and its descendant tokens
    /// that were read from the reader. The runtime type of the token is determined
    /// by the token type of the first token encountered in the reader.
    /// </returns>
    public static JToken Load(JsonReader reader)
    {
      return JToken.Load(reader, (JsonLoadSettings) null);
    }

    internal void SetLineInfo(IJsonLineInfo lineInfo, JsonLoadSettings settings)
    {
      if (settings != null && settings.LineInfoHandling == LineInfoHandling.Load || (lineInfo == null || !lineInfo.HasLineInfo()))
        return;
      this.SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
    }

    internal void SetLineInfo(int lineNumber, int linePosition)
    {
      this.AddAnnotation((object) new JToken.LineInfoAnnotation(lineNumber, linePosition));
    }

    bool IJsonLineInfo.HasLineInfo()
    {
      return this.Annotation<JToken.LineInfoAnnotation>() != null;
    }

    int IJsonLineInfo.LineNumber
    {
      get
      {
        JToken.LineInfoAnnotation lineInfoAnnotation = this.Annotation<JToken.LineInfoAnnotation>();
        return lineInfoAnnotation != null ? lineInfoAnnotation.LineNumber : 0;
      }
    }

    int IJsonLineInfo.LinePosition
    {
      get
      {
        JToken.LineInfoAnnotation lineInfoAnnotation = this.Annotation<JToken.LineInfoAnnotation>();
        return lineInfoAnnotation != null ? lineInfoAnnotation.LinePosition : 0;
      }
    }

    /// <summary>
    /// Selects a <see cref="T:Newtonsoft.Json.Linq.JToken" /> using a JPath expression. Selects the token that matches the object path.
    /// </summary>
    /// <param name="path">
    /// A <see cref="T:System.String" /> that contains a JPath expression.
    /// </param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JToken" />, or null.</returns>
    public JToken SelectToken(string path)
    {
      return this.SelectToken(path, false);
    }

    /// <summary>
    /// Selects a <see cref="T:Newtonsoft.Json.Linq.JToken" /> using a JPath expression. Selects the token that matches the object path.
    /// </summary>
    /// <param name="path">
    /// A <see cref="T:System.String" /> that contains a JPath expression.
    /// </param>
    /// <param name="errorWhenNoMatch">A flag to indicate whether an error should be thrown if no tokens are found when evaluating part of the expression.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JToken" />.</returns>
    public JToken SelectToken(string path, bool errorWhenNoMatch)
    {
      JPath jpath = new JPath(path);
      JToken jtoken1 = (JToken) null;
      int num = errorWhenNoMatch ? 1 : 0;
      foreach (JToken jtoken2 in jpath.Evaluate(this, num != 0))
      {
        if (jtoken1 != null)
          throw new JsonException("Path returned multiple tokens.");
        jtoken1 = jtoken2;
      }
      return jtoken1;
    }

    /// <summary>
    /// Selects a collection of elements using a JPath expression.
    /// </summary>
    /// <param name="path">
    /// A <see cref="T:System.String" /> that contains a JPath expression.
    /// </param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the selected elements.</returns>
    public IEnumerable<JToken> SelectTokens(string path)
    {
      return this.SelectTokens(path, false);
    }

    /// <summary>
    /// Selects a collection of elements using a JPath expression.
    /// </summary>
    /// <param name="path">
    /// A <see cref="T:System.String" /> that contains a JPath expression.
    /// </param>
    /// <param name="errorWhenNoMatch">A flag to indicate whether an error should be thrown if no tokens are found when evaluating part of the expression.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the selected elements.</returns>
    public IEnumerable<JToken> SelectTokens(string path, bool errorWhenNoMatch)
    {
      return new JPath(path).Evaluate(this, errorWhenNoMatch);
    }

    object ICloneable.Clone()
    {
      return (object) this.DeepClone();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="T:Newtonsoft.Json.Linq.JToken" />. All child tokens are recursively cloned.
    /// </summary>
    /// <returns>A new instance of the <see cref="T:Newtonsoft.Json.Linq.JToken" />.</returns>
    public JToken DeepClone()
    {
      return this.CloneToken();
    }

    /// <summary>
    /// Adds an object to the annotation list of this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="annotation">The annotation to add.</param>
    public void AddAnnotation(object annotation)
    {
      if (annotation == null)
        throw new ArgumentNullException(nameof (annotation));
      if (this._annotations == null)
      {
        object obj;
        if (!(annotation is object[]))
        {
          obj = annotation;
        }
        else
        {
          obj = (object) new object[1];
          obj[0] = annotation;
        }
        this._annotations = obj;
      }
      else if (!(this._annotations is object[] annotations))
      {
        this._annotations = (object) new object[2]
        {
          this._annotations,
          annotation
        };
      }
      else
      {
        int index = 0;
        while (index < annotations.Length && annotations[index] != null)
          ++index;
        if (index == annotations.Length)
        {
          Array.Resize<object>(ref annotations, index * 2);
          this._annotations = (object) annotations;
        }
        annotations[index] = annotation;
      }
    }

    /// <summary>
    /// Get the first annotation object of the specified type from this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <typeparam name="T">The type of the annotation to retrieve.</typeparam>
    /// <returns>The first annotation object that matches the specified type, or <c>null</c> if no annotation is of the specified type.</returns>
    public T Annotation<T>() where T : class
    {
      if (this._annotations != null)
      {
        if (!(this._annotations is object[] annotations))
          return this._annotations as T;
        for (int index = 0; index < annotations.Length; ++index)
        {
          object obj1 = annotations[index];
          if (obj1 != null)
          {
            if (obj1 is T obj)
              return obj;
          }
          else
            break;
        }
      }
      return default (T);
    }

    /// <summary>
    /// Gets the first annotation object of the specified type from this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="type">The <see cref="P:Newtonsoft.Json.Linq.JToken.Type" /> of the annotation to retrieve.</param>
    /// <returns>The first annotation object that matches the specified type, or <c>null</c> if no annotation is of the specified type.</returns>
    public object Annotation(System.Type type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      if (this._annotations != null)
      {
        if (!(this._annotations is object[] annotations))
        {
          if (type.IsInstanceOfType(this._annotations))
            return this._annotations;
        }
        else
        {
          for (int index = 0; index < annotations.Length; ++index)
          {
            object o = annotations[index];
            if (o != null)
            {
              if (type.IsInstanceOfType(o))
                return o;
            }
            else
              break;
          }
        }
      }
      return (object) null;
    }

    /// <summary>
    /// Gets a collection of annotations of the specified type for this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <typeparam name="T">The type of the annotations to retrieve.</typeparam>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" />  that contains the annotations for this <see cref="T:Newtonsoft.Json.Linq.JToken" />.</returns>
    public IEnumerable<T> Annotations<T>() where T : class
    {
      if (this._annotations != null)
      {
        if (this._annotations is object[] annotations)
        {
          for (int i = 0; i < annotations.Length; ++i)
          {
            object obj1 = annotations[i];
            if (obj1 == null)
              break;
            if (obj1 is T obj)
              yield return obj;
          }
        }
        else if (this._annotations is T annotations)
          yield return annotations;
      }
    }

    /// <summary>
    /// Gets a collection of annotations of the specified type for this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="type">The <see cref="P:Newtonsoft.Json.Linq.JToken.Type" /> of the annotations to retrieve.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Object" /> that contains the annotations that match the specified type for this <see cref="T:Newtonsoft.Json.Linq.JToken" />.</returns>
    public IEnumerable<object> Annotations(System.Type type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      if (this._annotations != null)
      {
        if (this._annotations is object[] annotations)
        {
          for (int i = 0; i < annotations.Length; ++i)
          {
            object o = annotations[i];
            if (o == null)
              break;
            if (type.IsInstanceOfType(o))
              yield return o;
          }
        }
        else if (type.IsInstanceOfType(this._annotations))
          yield return this._annotations;
      }
    }

    /// <summary>
    /// Removes the annotations of the specified type from this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <typeparam name="T">The type of annotations to remove.</typeparam>
    public void RemoveAnnotations<T>() where T : class
    {
      if (this._annotations == null)
        return;
      if (!(this._annotations is object[] annotations))
      {
        if (!(this._annotations is T))
          return;
        this._annotations = (object) null;
      }
      else
      {
        int index = 0;
        int num = 0;
        for (; index < annotations.Length; ++index)
        {
          object obj = annotations[index];
          if (obj != null)
          {
            if (!(obj is T))
              annotations[num++] = obj;
          }
          else
            break;
        }
        if (num != 0)
        {
          while (num < index)
            annotations[num++] = (object) null;
        }
        else
          this._annotations = (object) null;
      }
    }

    /// <summary>
    /// Removes the annotations of the specified type from this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="type">The <see cref="P:Newtonsoft.Json.Linq.JToken.Type" /> of annotations to remove.</param>
    public void RemoveAnnotations(System.Type type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      if (this._annotations == null)
        return;
      if (!(this._annotations is object[] annotations))
      {
        if (!type.IsInstanceOfType(this._annotations))
          return;
        this._annotations = (object) null;
      }
      else
      {
        int index = 0;
        int num = 0;
        for (; index < annotations.Length; ++index)
        {
          object o = annotations[index];
          if (o != null)
          {
            if (!type.IsInstanceOfType(o))
              annotations[num++] = o;
          }
          else
            break;
        }
        if (num != 0)
        {
          while (num < index)
            annotations[num++] = (object) null;
        }
        else
          this._annotations = (object) null;
      }
    }

    private class LineInfoAnnotation
    {
      internal readonly int LineNumber;
      internal readonly int LinePosition;

      public LineInfoAnnotation(int lineNumber, int linePosition)
      {
        this.LineNumber = lineNumber;
        this.LinePosition = linePosition;
      }
    }
  }
}
