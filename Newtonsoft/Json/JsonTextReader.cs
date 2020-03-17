// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonTextReader
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Represents a reader that provides fast, non-cached, forward-only access to JSON text data.
  /// </summary>
  [Preserve]
  public class JsonTextReader : JsonReader, IJsonLineInfo
  {
    private const char UnicodeReplacementChar = '�';
    private const int MaximumJavascriptIntegerCharacterLength = 380;
    private readonly TextReader _reader;
    private char[] _chars;
    private int _charsUsed;
    private int _charPos;
    private int _lineStartPos;
    private int _lineNumber;
    private bool _isEndOfFile;
    private StringBuffer _stringBuffer;
    private StringReference _stringReference;
    private IArrayPool<char> _arrayPool;
    internal PropertyNameTable NameTable;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonReader" /> class with the specified <see cref="T:System.IO.TextReader" />.
    /// </summary>
    /// <param name="reader">The <c>TextReader</c> containing the XML data to read.</param>
    public JsonTextReader(TextReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      this._reader = reader;
      this._lineNumber = 1;
    }

    /// <summary>Gets or sets the reader's character buffer pool.</summary>
    public IArrayPool<char> ArrayPool
    {
      get
      {
        return this._arrayPool;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        this._arrayPool = value;
      }
    }

    private void EnsureBufferNotEmpty()
    {
      if (!this._stringBuffer.IsEmpty)
        return;
      this._stringBuffer = new StringBuffer(this._arrayPool, 1024);
    }

    private void OnNewLine(int pos)
    {
      ++this._lineNumber;
      this._lineStartPos = pos;
    }

    private void ParseString(char quote, ReadType readType)
    {
      ++this._charPos;
      this.ShiftBufferIfNeeded();
      this.ReadStringIntoBuffer(quote);
      this.SetPostValueState(true);
      switch (readType)
      {
        case ReadType.ReadAsInt32:
          break;
        case ReadType.ReadAsBytes:
          Guid g;
          this.SetToken(JsonToken.Bytes, this._stringReference.Length != 0 ? (this._stringReference.Length != 36 || !ConvertUtils.TryConvertGuid(this._stringReference.ToString(), out g) ? (object) Convert.FromBase64CharArray(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length) : (object) g.ToByteArray()) : (object) new byte[0], false);
          break;
        case ReadType.ReadAsString:
          this.SetToken(JsonToken.String, (object) this._stringReference.ToString(), false);
          this._quoteChar = quote;
          break;
        case ReadType.ReadAsDecimal:
          break;
        case ReadType.ReadAsBoolean:
          break;
        default:
          if (this._dateParseHandling != DateParseHandling.None)
          {
            DateParseHandling dateParseHandling;
            switch (readType)
            {
              case ReadType.ReadAsDateTime:
                dateParseHandling = DateParseHandling.DateTime;
                break;
              case ReadType.ReadAsDateTimeOffset:
                dateParseHandling = DateParseHandling.DateTimeOffset;
                break;
              default:
                dateParseHandling = this._dateParseHandling;
                break;
            }
            if (dateParseHandling == DateParseHandling.DateTime)
            {
              DateTime dt;
              if (DateTimeUtils.TryParseDateTime(this._stringReference, this.DateTimeZoneHandling, this.DateFormatString, this.Culture, out dt))
              {
                this.SetToken(JsonToken.Date, (object) dt, false);
                break;
              }
            }
            else
            {
              DateTimeOffset dt;
              if (DateTimeUtils.TryParseDateTimeOffset(this._stringReference, this.DateFormatString, this.Culture, out dt))
              {
                this.SetToken(JsonToken.Date, (object) dt, false);
                break;
              }
            }
          }
          this.SetToken(JsonToken.String, (object) this._stringReference.ToString(), false);
          this._quoteChar = quote;
          break;
      }
    }

    private static void BlockCopyChars(
      char[] src,
      int srcOffset,
      char[] dst,
      int dstOffset,
      int count)
    {
      Buffer.BlockCopy((Array) src, srcOffset * 2, (Array) dst, dstOffset * 2, count * 2);
    }

    private void ShiftBufferIfNeeded()
    {
      int length = this._chars.Length;
      if ((double) (length - this._charPos) > (double) length * 0.1)
        return;
      int count = this._charsUsed - this._charPos;
      if (count > 0)
        JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, count);
      this._lineStartPos -= this._charPos;
      this._charPos = 0;
      this._charsUsed = count;
      this._chars[this._charsUsed] = char.MinValue;
    }

    private int ReadData(bool append)
    {
      return this.ReadData(append, 0);
    }

    private int ReadData(bool append, int charsRequired)
    {
      if (this._isEndOfFile)
        return 0;
      if (this._charsUsed + charsRequired >= this._chars.Length - 1)
      {
        if (append)
        {
          char[] dst = BufferUtils.RentBuffer(this._arrayPool, Math.Max(this._chars.Length * 2, this._charsUsed + charsRequired + 1));
          JsonTextReader.BlockCopyChars(this._chars, 0, dst, 0, this._chars.Length);
          BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
          this._chars = dst;
        }
        else
        {
          int count = this._charsUsed - this._charPos;
          if (count + charsRequired + 1 >= this._chars.Length)
          {
            char[] dst = BufferUtils.RentBuffer(this._arrayPool, count + charsRequired + 1);
            if (count > 0)
              JsonTextReader.BlockCopyChars(this._chars, this._charPos, dst, 0, count);
            BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
            this._chars = dst;
          }
          else if (count > 0)
            JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, count);
          this._lineStartPos -= this._charPos;
          this._charPos = 0;
          this._charsUsed = count;
        }
      }
      int num = this._reader.Read(this._chars, this._charsUsed, this._chars.Length - this._charsUsed - 1);
      this._charsUsed += num;
      if (num == 0)
        this._isEndOfFile = true;
      this._chars[this._charsUsed] = char.MinValue;
      return num;
    }

    private bool EnsureChars(int relativePosition, bool append)
    {
      return this._charPos + relativePosition < this._charsUsed || this.ReadChars(relativePosition, append);
    }

    private bool ReadChars(int relativePosition, bool append)
    {
      if (this._isEndOfFile)
        return false;
      int num1 = this._charPos + relativePosition - this._charsUsed + 1;
      int num2 = 0;
      do
      {
        int num3 = this.ReadData(append, num1 - num2);
        if (num3 != 0)
          num2 += num3;
        else
          break;
      }
      while (num2 < num1);
      return num2 >= num1;
    }

    /// <summary>Reads the next JSON token from the stream.</summary>
    /// <returns>
    /// true if the next token was read successfully; false if there are no more tokens to read.
    /// </returns>
    public override bool Read()
    {
      this.EnsureBuffer();
      do
      {
        switch (this._currentState)
        {
          case JsonReader.State.Start:
          case JsonReader.State.Property:
          case JsonReader.State.ArrayStart:
          case JsonReader.State.Array:
          case JsonReader.State.ConstructorStart:
          case JsonReader.State.Constructor:
            return this.ParseValue();
          case JsonReader.State.ObjectStart:
          case JsonReader.State.Object:
            return this.ParseObject();
          case JsonReader.State.PostValue:
            continue;
          case JsonReader.State.Finished:
            goto label_6;
          default:
            goto label_13;
        }
      }
      while (!this.ParsePostValue());
      return true;
label_6:
      if (this.EnsureChars(0, false))
      {
        this.EatWhitespace(false);
        if (this._isEndOfFile)
        {
          this.SetToken(JsonToken.None);
          return false;
        }
        if (this._chars[this._charPos] != '/')
          throw JsonReaderException.Create((JsonReader) this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
        this.ParseComment(true);
        return true;
      }
      this.SetToken(JsonToken.None);
      return false;
label_13:
      throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public override int? ReadAsInt32()
    {
      return (int?) this.ReadNumberValue(ReadType.ReadAsInt32);
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public override DateTime? ReadAsDateTime()
    {
      return (DateTime?) this.ReadStringValue(ReadType.ReadAsDateTime);
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.String" />.
    /// </summary>
    /// <returns>A <see cref="T:System.String" />. This method will return <c>null</c> at the end of an array.</returns>
    public override string ReadAsString()
    {
      return (string) this.ReadStringValue(ReadType.ReadAsString);
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Byte" />[].
    /// </summary>
    /// <returns>A <see cref="T:System.Byte" />[] or a null reference if the next JSON token is null. This method will return <c>null</c> at the end of an array.</returns>
    public override byte[] ReadAsBytes()
    {
      this.EnsureBuffer();
      bool flag = false;
      switch (this._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.PostValue:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          char ch;
          do
          {
            do
            {
              ch = this._chars[this._charPos];
              switch (ch)
              {
                case char.MinValue:
                  continue;
                case '\t':
                case ' ':
                  goto label_19;
                case '\n':
                  goto label_18;
                case '\r':
                  goto label_17;
                case '"':
                case '\'':
                  goto label_4;
                case ',':
                  goto label_13;
                case '/':
                  goto label_12;
                case '[':
                  goto label_10;
                case ']':
                  goto label_14;
                case 'n':
                  goto label_11;
                case '{':
                  goto label_9;
                default:
                  goto label_20;
              }
            }
            while (!this.ReadNullChar());
            this.SetToken(JsonToken.None, (object) null, false);
            return (byte[]) null;
label_4:
            this.ParseString(ch, ReadType.ReadAsBytes);
            byte[] numArray = (byte[]) this.Value;
            if (flag)
            {
              this.ReaderReadAndAssert();
              if (this.TokenType != JsonToken.EndObject)
                throw JsonReaderException.Create((JsonReader) this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
              this.SetToken(JsonToken.Bytes, (object) numArray, false);
            }
            return numArray;
label_9:
            ++this._charPos;
            this.SetToken(JsonToken.StartObject);
            this.ReadIntoWrappedTypeObject();
            flag = true;
            continue;
label_10:
            ++this._charPos;
            this.SetToken(JsonToken.StartArray);
            return this.ReadArrayIntoByteArray();
label_11:
            this.HandleNull();
            return (byte[]) null;
label_12:
            this.ParseComment(false);
            continue;
label_13:
            this.ProcessValueComma();
            continue;
label_14:
            ++this._charPos;
            if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.PostValue)
              throw this.CreateUnexpectedCharacterException(ch);
            this.SetToken(JsonToken.EndArray);
            return (byte[]) null;
label_17:
            this.ProcessCarriageReturn(false);
            continue;
label_18:
            this.ProcessLineFeed();
            continue;
label_19:
            ++this._charPos;
            continue;
label_20:
            ++this._charPos;
          }
          while (char.IsWhiteSpace(ch));
          throw this.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.Finished:
          this.ReadFinished();
          return (byte[]) null;
        default:
          throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
      }
    }

    private object ReadStringValue(ReadType readType)
    {
      this.EnsureBuffer();
      switch (this._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.PostValue:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          char ch;
          do
          {
            do
            {
              ch = this._chars[this._charPos];
              switch (ch)
              {
                case char.MinValue:
                  continue;
                case '\t':
                case ' ':
                  goto label_35;
                case '\n':
                  goto label_34;
                case '\r':
                  goto label_33;
                case '"':
                case '\'':
                  goto label_4;
                case ',':
                  goto label_29;
                case '-':
                  goto label_14;
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                  goto label_17;
                case '/':
                  goto label_28;
                case 'I':
                  goto label_25;
                case 'N':
                  goto label_26;
                case ']':
                  goto label_30;
                case 'f':
                case 't':
                  goto label_20;
                case 'n':
                  goto label_27;
                default:
                  goto label_36;
              }
            }
            while (!this.ReadNullChar());
            this.SetToken(JsonToken.None, (object) null, false);
            return (object) null;
label_4:
            this.ParseString(ch, readType);
            switch (readType)
            {
              case ReadType.ReadAsBytes:
                return this.Value;
              case ReadType.ReadAsString:
                return this.Value;
              case ReadType.ReadAsDateTime:
                return this.Value is DateTime ? (object) (DateTime) this.Value : (object) this.ReadDateTimeString((string) this.Value);
              case ReadType.ReadAsDateTimeOffset:
                return this.Value is DateTimeOffset ? (object) (DateTimeOffset) this.Value : (object) this.ReadDateTimeOffsetString((string) this.Value);
              default:
                throw new ArgumentOutOfRangeException(nameof (readType));
            }
label_14:
            if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
              return this.ParseNumberNegativeInfinity(readType);
            this.ParseNumber(readType);
            return this.Value;
label_17:
            if (readType != ReadType.ReadAsString)
            {
              ++this._charPos;
              throw this.CreateUnexpectedCharacterException(ch);
            }
            this.ParseNumber(ReadType.ReadAsString);
            return this.Value;
label_20:
            if (readType != ReadType.ReadAsString)
            {
              ++this._charPos;
              throw this.CreateUnexpectedCharacterException(ch);
            }
            string str = ch == 't' ? JsonConvert.True : JsonConvert.False;
            if (!this.MatchValueWithTrailingSeparator(str))
              throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
            this.SetToken(JsonToken.String, (object) str);
            return (object) str;
label_25:
            return this.ParseNumberPositiveInfinity(readType);
label_26:
            return this.ParseNumberNaN(readType);
label_27:
            this.HandleNull();
            return (object) null;
label_28:
            this.ParseComment(false);
            continue;
label_29:
            this.ProcessValueComma();
            continue;
label_30:
            ++this._charPos;
            if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.PostValue)
              throw this.CreateUnexpectedCharacterException(ch);
            this.SetToken(JsonToken.EndArray);
            return (object) null;
label_33:
            this.ProcessCarriageReturn(false);
            continue;
label_34:
            this.ProcessLineFeed();
            continue;
label_35:
            ++this._charPos;
            continue;
label_36:
            ++this._charPos;
          }
          while (char.IsWhiteSpace(ch));
          throw this.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.Finished:
          this.ReadFinished();
          return (object) null;
        default:
          throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
      }
    }

    private JsonReaderException CreateUnexpectedCharacterException(char c)
    {
      return JsonReaderException.Create((JsonReader) this, "Unexpected character encountered while parsing value: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) c));
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public override bool? ReadAsBoolean()
    {
      this.EnsureBuffer();
      switch (this._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.PostValue:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          char ch;
          do
          {
            do
            {
              ch = this._chars[this._charPos];
              switch (ch)
              {
                case char.MinValue:
                  continue;
                case '\t':
                case ' ':
                  goto label_17;
                case '\n':
                  goto label_16;
                case '\r':
                  goto label_15;
                case '"':
                case '\'':
                  goto label_4;
                case ',':
                  goto label_11;
                case '-':
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                  goto label_6;
                case '/':
                  goto label_10;
                case ']':
                  goto label_12;
                case 'f':
                case 't':
                  goto label_7;
                case 'n':
                  goto label_5;
                default:
                  goto label_18;
              }
            }
            while (!this.ReadNullChar());
            this.SetToken(JsonToken.None, (object) null, false);
            return new bool?();
label_4:
            this.ParseString(ch, ReadType.Read);
            return this.ReadBooleanString(this._stringReference.ToString());
label_5:
            this.HandleNull();
            return new bool?();
label_6:
            this.ParseNumber(ReadType.Read);
            bool boolean = Convert.ToBoolean(this.Value, (IFormatProvider) CultureInfo.InvariantCulture);
            this.SetToken(JsonToken.Boolean, (object) boolean, false);
            return new bool?(boolean);
label_7:
            bool flag = ch == 't';
            if (!this.MatchValueWithTrailingSeparator(flag ? JsonConvert.True : JsonConvert.False))
              throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
            this.SetToken(JsonToken.Boolean, (object) flag);
            return new bool?(flag);
label_10:
            this.ParseComment(false);
            continue;
label_11:
            this.ProcessValueComma();
            continue;
label_12:
            ++this._charPos;
            if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.PostValue)
              throw this.CreateUnexpectedCharacterException(ch);
            this.SetToken(JsonToken.EndArray);
            return new bool?();
label_15:
            this.ProcessCarriageReturn(false);
            continue;
label_16:
            this.ProcessLineFeed();
            continue;
label_17:
            ++this._charPos;
            continue;
label_18:
            ++this._charPos;
          }
          while (char.IsWhiteSpace(ch));
          throw this.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.Finished:
          this.ReadFinished();
          return new bool?();
        default:
          throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
      }
    }

    private void ProcessValueComma()
    {
      ++this._charPos;
      if (this._currentState != JsonReader.State.PostValue)
      {
        this.SetToken(JsonToken.Undefined);
        throw this.CreateUnexpectedCharacterException(',');
      }
      this.SetStateBasedOnCurrent();
    }

    private object ReadNumberValue(ReadType readType)
    {
      this.EnsureBuffer();
      switch (this._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.PostValue:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          char ch;
          do
          {
            do
            {
              ch = this._chars[this._charPos];
              switch (ch)
              {
                case char.MinValue:
                  continue;
                case '\t':
                case ' ':
                  goto label_25;
                case '\n':
                  goto label_24;
                case '\r':
                  goto label_23;
                case '"':
                case '\'':
                  goto label_4;
                case ',':
                  goto label_19;
                case '-':
                  goto label_14;
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                  goto label_17;
                case '/':
                  goto label_18;
                case 'I':
                  goto label_13;
                case 'N':
                  goto label_12;
                case ']':
                  goto label_20;
                case 'n':
                  goto label_11;
                default:
                  goto label_26;
              }
            }
            while (!this.ReadNullChar());
            this.SetToken(JsonToken.None, (object) null, false);
            return (object) null;
label_4:
            this.ParseString(ch, readType);
            if (readType == ReadType.ReadAsInt32)
              return (object) this.ReadInt32String(this._stringReference.ToString());
            if (readType == ReadType.ReadAsDecimal)
              return (object) this.ReadDecimalString(this._stringReference.ToString());
            if (readType == ReadType.ReadAsDouble)
              return (object) this.ReadDoubleString(this._stringReference.ToString());
            throw new ArgumentOutOfRangeException(nameof (readType));
label_11:
            this.HandleNull();
            return (object) null;
label_12:
            return this.ParseNumberNaN(readType);
label_13:
            return this.ParseNumberPositiveInfinity(readType);
label_14:
            if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
              return this.ParseNumberNegativeInfinity(readType);
            this.ParseNumber(readType);
            return this.Value;
label_17:
            this.ParseNumber(readType);
            return this.Value;
label_18:
            this.ParseComment(false);
            continue;
label_19:
            this.ProcessValueComma();
            continue;
label_20:
            ++this._charPos;
            if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.PostValue)
              throw this.CreateUnexpectedCharacterException(ch);
            this.SetToken(JsonToken.EndArray);
            return (object) null;
label_23:
            this.ProcessCarriageReturn(false);
            continue;
label_24:
            this.ProcessLineFeed();
            continue;
label_25:
            ++this._charPos;
            continue;
label_26:
            ++this._charPos;
          }
          while (char.IsWhiteSpace(ch));
          throw this.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.Finished:
          this.ReadFinished();
          return (object) null;
        default:
          throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
      }
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public override DateTimeOffset? ReadAsDateTimeOffset()
    {
      return (DateTimeOffset?) this.ReadStringValue(ReadType.ReadAsDateTimeOffset);
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public override Decimal? ReadAsDecimal()
    {
      return (Decimal?) this.ReadNumberValue(ReadType.ReadAsDecimal);
    }

    /// <summary>
    /// Reads the next JSON token from the stream as a <see cref="T:System.Nullable`1" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Nullable`1" />. This method will return <c>null</c> at the end of an array.</returns>
    public override double? ReadAsDouble()
    {
      return (double?) this.ReadNumberValue(ReadType.ReadAsDouble);
    }

    private void HandleNull()
    {
      if (this.EnsureChars(1, true))
      {
        if (this._chars[this._charPos + 1] == 'u')
        {
          this.ParseNull();
        }
        else
        {
          this._charPos += 2;
          throw this.CreateUnexpectedCharacterException(this._chars[this._charPos - 1]);
        }
      }
      else
      {
        this._charPos = this._charsUsed;
        throw this.CreateUnexpectedEndException();
      }
    }

    private void ReadFinished()
    {
      if (this.EnsureChars(0, false))
      {
        this.EatWhitespace(false);
        if (this._isEndOfFile)
          return;
        if (this._chars[this._charPos] != '/')
          throw JsonReaderException.Create((JsonReader) this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
        this.ParseComment(false);
      }
      this.SetToken(JsonToken.None);
    }

    private bool ReadNullChar()
    {
      if (this._charsUsed == this._charPos)
      {
        if (this.ReadData(false) == 0)
        {
          this._isEndOfFile = true;
          return true;
        }
      }
      else
        ++this._charPos;
      return false;
    }

    private void EnsureBuffer()
    {
      if (this._chars != null)
        return;
      this._chars = BufferUtils.RentBuffer(this._arrayPool, 1024);
      this._chars[0] = char.MinValue;
    }

    private void ReadStringIntoBuffer(char quote)
    {
      int charPos1 = this._charPos;
      int charPos2 = this._charPos;
      int num1 = this._charPos;
      this._stringBuffer.Position = 0;
      do
      {
        char ch1 = this._chars[charPos1++];
        if (ch1 <= '\r')
        {
          if (ch1 != char.MinValue)
          {
            if (ch1 != '\n')
            {
              if (ch1 == '\r')
              {
                this._charPos = charPos1 - 1;
                this.ProcessCarriageReturn(true);
                charPos1 = this._charPos;
              }
            }
            else
            {
              this._charPos = charPos1 - 1;
              this.ProcessLineFeed();
              charPos1 = this._charPos;
            }
          }
          else if (this._charsUsed == charPos1 - 1)
          {
            --charPos1;
            if (this.ReadData(true) == 0)
            {
              this._charPos = charPos1;
              throw JsonReaderException.Create((JsonReader) this, "Unterminated string. Expected delimiter: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) quote));
            }
          }
        }
        else if (ch1 != '"' && ch1 != '\'')
        {
          if (ch1 == '\\')
          {
            this._charPos = charPos1;
            if (!this.EnsureChars(0, true))
              throw JsonReaderException.Create((JsonReader) this, "Unterminated string. Expected delimiter: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) quote));
            int writeToPosition = charPos1 - 1;
            char ch2 = this._chars[charPos1];
            ++charPos1;
            char ch3;
            switch (ch2)
            {
              case '"':
              case '\'':
              case '/':
                ch3 = ch2;
                break;
              case '\\':
                ch3 = '\\';
                break;
              case 'b':
                ch3 = '\b';
                break;
              case 'f':
                ch3 = '\f';
                break;
              case 'n':
                ch3 = '\n';
                break;
              case 'r':
                ch3 = '\r';
                break;
              case 't':
                ch3 = '\t';
                break;
              case 'u':
                this._charPos = charPos1;
                ch3 = this.ParseUnicode();
                if (StringUtils.IsLowSurrogate(ch3))
                  ch3 = '�';
                else if (StringUtils.IsHighSurrogate(ch3))
                {
                  bool flag;
                  do
                  {
                    flag = false;
                    if (this.EnsureChars(2, true) && this._chars[this._charPos] == '\\' && this._chars[this._charPos + 1] == 'u')
                    {
                      char writeChar = ch3;
                      this._charPos += 2;
                      ch3 = this.ParseUnicode();
                      if (!StringUtils.IsLowSurrogate(ch3))
                      {
                        if (StringUtils.IsHighSurrogate(ch3))
                        {
                          writeChar = '�';
                          flag = true;
                        }
                        else
                          writeChar = '�';
                      }
                      this.EnsureBufferNotEmpty();
                      this.WriteCharToBuffer(writeChar, num1, writeToPosition);
                      num1 = this._charPos;
                    }
                    else
                      ch3 = '�';
                  }
                  while (flag);
                }
                charPos1 = this._charPos;
                break;
              default:
                this._charPos = charPos1;
                throw JsonReaderException.Create((JsonReader) this, "Bad JSON escape sequence: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) ("\\" + ch2.ToString())));
            }
            this.EnsureBufferNotEmpty();
            this.WriteCharToBuffer(ch3, num1, writeToPosition);
            num1 = charPos1;
          }
        }
      }
      while ((int) this._chars[charPos1 - 1] != (int) quote);
      int num2 = charPos1 - 1;
      if (charPos2 == num1)
      {
        this._stringReference = new StringReference(this._chars, charPos2, num2 - charPos2);
      }
      else
      {
        this.EnsureBufferNotEmpty();
        if (num2 > num1)
          this._stringBuffer.Append(this._arrayPool, this._chars, num1, num2 - num1);
        this._stringReference = new StringReference(this._stringBuffer.InternalBuffer, 0, this._stringBuffer.Position);
      }
      this._charPos = num2 + 1;
    }

    private void WriteCharToBuffer(char writeChar, int lastWritePosition, int writeToPosition)
    {
      if (writeToPosition > lastWritePosition)
        this._stringBuffer.Append(this._arrayPool, this._chars, lastWritePosition, writeToPosition - lastWritePosition);
      this._stringBuffer.Append(this._arrayPool, writeChar);
    }

    private char ParseUnicode()
    {
      if (!this.EnsureChars(4, true))
        throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing unicode character.");
      char ch = Convert.ToChar(ConvertUtils.HexTextToInt(this._chars, this._charPos, this._charPos + 4));
      this._charPos += 4;
      return ch;
    }

    private void ReadNumberIntoBuffer()
    {
      int charPos = this._charPos;
      while (true)
      {
        do
        {
          switch (this._chars[charPos])
          {
            case char.MinValue:
              this._charPos = charPos;
              continue;
            case '+':
            case '-':
            case '.':
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
            case 'A':
            case 'B':
            case 'C':
            case 'D':
            case 'E':
            case 'F':
            case 'X':
            case 'a':
            case 'b':
            case 'c':
            case 'd':
            case 'e':
            case 'f':
            case 'x':
              goto label_5;
            default:
              goto label_6;
          }
        }
        while (this._charsUsed == charPos && this.ReadData(true) != 0);
        break;
label_5:
        ++charPos;
      }
      return;
label_6:
      this._charPos = charPos;
      char c = this._chars[this._charPos];
      if (!char.IsWhiteSpace(c) && c != ',' && (c != '}' && c != ']') && (c != ')' && c != '/'))
        throw JsonReaderException.Create((JsonReader) this, "Unexpected character encountered while parsing number: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) c));
    }

    private void ClearRecentString()
    {
      this._stringBuffer.Position = 0;
      this._stringReference = new StringReference();
    }

    private bool ParsePostValue()
    {
      char c;
      while (true)
      {
        do
        {
          c = this._chars[this._charPos];
          switch (c)
          {
            case char.MinValue:
              if (this._charsUsed == this._charPos)
                continue;
              goto label_4;
            case '\t':
            case ' ':
              goto label_10;
            case '\n':
              goto label_12;
            case '\r':
              goto label_11;
            case ')':
              goto label_7;
            case ',':
              goto label_9;
            case '/':
              goto label_8;
            case ']':
              goto label_6;
            case '}':
              goto label_5;
            default:
              goto label_13;
          }
        }
        while (this.ReadData(false) != 0);
        break;
label_4:
        ++this._charPos;
        continue;
label_10:
        ++this._charPos;
        continue;
label_11:
        this.ProcessCarriageReturn(false);
        continue;
label_12:
        this.ProcessLineFeed();
        continue;
label_13:
        if (char.IsWhiteSpace(c))
          ++this._charPos;
        else
          goto label_15;
      }
      this._currentState = JsonReader.State.Finished;
      return false;
label_5:
      ++this._charPos;
      this.SetToken(JsonToken.EndObject);
      return true;
label_6:
      ++this._charPos;
      this.SetToken(JsonToken.EndArray);
      return true;
label_7:
      ++this._charPos;
      this.SetToken(JsonToken.EndConstructor);
      return true;
label_8:
      this.ParseComment(true);
      return true;
label_9:
      ++this._charPos;
      this.SetStateBasedOnCurrent();
      return false;
label_15:
      throw JsonReaderException.Create((JsonReader) this, "After parsing a value an unexpected character was encountered: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) c));
    }

    private bool ParseObject()
    {
      while (true)
      {
        char c;
        do
        {
          c = this._chars[this._charPos];
          switch (c)
          {
            case char.MinValue:
              if (this._charsUsed == this._charPos)
                continue;
              goto label_4;
            case '\t':
            case ' ':
              goto label_9;
            case '\n':
              goto label_8;
            case '\r':
              goto label_7;
            case '/':
              goto label_6;
            case '}':
              goto label_5;
            default:
              goto label_10;
          }
        }
        while (this.ReadData(false) != 0);
        break;
label_4:
        ++this._charPos;
        continue;
label_7:
        this.ProcessCarriageReturn(false);
        continue;
label_8:
        this.ProcessLineFeed();
        continue;
label_9:
        ++this._charPos;
        continue;
label_10:
        if (char.IsWhiteSpace(c))
          ++this._charPos;
        else
          goto label_12;
      }
      return false;
label_5:
      this.SetToken(JsonToken.EndObject);
      ++this._charPos;
      return true;
label_6:
      this.ParseComment(true);
      return true;
label_12:
      return this.ParseProperty();
    }

    private bool ParseProperty()
    {
      char ch = this._chars[this._charPos];
      char quote;
      switch (ch)
      {
        case '"':
        case '\'':
          ++this._charPos;
          quote = ch;
          this.ShiftBufferIfNeeded();
          this.ReadStringIntoBuffer(quote);
          break;
        default:
          if (!this.ValidIdentifierChar(ch))
            throw JsonReaderException.Create((JsonReader) this, "Invalid property identifier character: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
          quote = char.MinValue;
          this.ShiftBufferIfNeeded();
          this.ParseUnquotedProperty();
          break;
      }
      string str = this.NameTable == null ? this._stringReference.ToString() : this.NameTable.Get(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length) ?? this._stringReference.ToString();
      this.EatWhitespace(false);
      if (this._chars[this._charPos] != ':')
        throw JsonReaderException.Create((JsonReader) this, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
      ++this._charPos;
      this.SetToken(JsonToken.PropertyName, (object) str);
      this._quoteChar = quote;
      this.ClearRecentString();
      return true;
    }

    private bool ValidIdentifierChar(char value)
    {
      return char.IsLetterOrDigit(value) || value == '_' || value == '$';
    }

    private void ParseUnquotedProperty()
    {
      int charPos = this._charPos;
      do
      {
        for (; this._chars[this._charPos] != char.MinValue; ++this._charPos)
        {
          char c = this._chars[this._charPos];
          if (!this.ValidIdentifierChar(c))
          {
            if (!char.IsWhiteSpace(c) && c != ':')
              throw JsonReaderException.Create((JsonReader) this, "Invalid JavaScript property identifier character: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) c));
            this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
            return;
          }
        }
        if (this._charsUsed != this._charPos)
          goto label_5;
      }
      while (this.ReadData(true) != 0);
      throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing unquoted property name.");
label_5:
      this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
    }

    private bool ParseValue()
    {
      char ch;
      while (true)
      {
        do
        {
          ch = this._chars[this._charPos];
          switch (ch)
          {
            case char.MinValue:
              if (this._charsUsed == this._charPos)
                continue;
              goto label_4;
            case '\t':
            case ' ':
              goto label_30;
            case '\n':
              goto label_29;
            case '\r':
              goto label_28;
            case '"':
            case '\'':
              goto label_5;
            case ')':
              goto label_27;
            case ',':
              goto label_26;
            case '-':
              goto label_17;
            case '/':
              goto label_21;
            case 'I':
              goto label_16;
            case 'N':
              goto label_15;
            case '[':
              goto label_24;
            case ']':
              goto label_25;
            case 'f':
              goto label_7;
            case 'n':
              goto label_8;
            case 't':
              goto label_6;
            case 'u':
              goto label_22;
            case '{':
              goto label_23;
            default:
              goto label_31;
          }
        }
        while (this.ReadData(false) != 0);
        break;
label_4:
        ++this._charPos;
        continue;
label_28:
        this.ProcessCarriageReturn(false);
        continue;
label_29:
        this.ProcessLineFeed();
        continue;
label_30:
        ++this._charPos;
        continue;
label_31:
        if (char.IsWhiteSpace(ch))
          ++this._charPos;
        else
          goto label_33;
      }
      return false;
label_5:
      this.ParseString(ch, ReadType.Read);
      return true;
label_6:
      this.ParseTrue();
      return true;
label_7:
      this.ParseFalse();
      return true;
label_8:
      if (this.EnsureChars(1, true))
      {
        switch (this._chars[this._charPos + 1])
        {
          case 'e':
            this.ParseConstructor();
            break;
          case 'u':
            this.ParseNull();
            break;
          default:
            throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
        }
        return true;
      }
      ++this._charPos;
      throw this.CreateUnexpectedEndException();
label_15:
      this.ParseNumberNaN(ReadType.Read);
      return true;
label_16:
      this.ParseNumberPositiveInfinity(ReadType.Read);
      return true;
label_17:
      if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
        this.ParseNumberNegativeInfinity(ReadType.Read);
      else
        this.ParseNumber(ReadType.Read);
      return true;
label_21:
      this.ParseComment(true);
      return true;
label_22:
      this.ParseUndefined();
      return true;
label_23:
      ++this._charPos;
      this.SetToken(JsonToken.StartObject);
      return true;
label_24:
      ++this._charPos;
      this.SetToken(JsonToken.StartArray);
      return true;
label_25:
      ++this._charPos;
      this.SetToken(JsonToken.EndArray);
      return true;
label_26:
      this.SetToken(JsonToken.Undefined);
      return true;
label_27:
      ++this._charPos;
      this.SetToken(JsonToken.EndConstructor);
      return true;
label_33:
      if (!char.IsNumber(ch) && ch != '-' && ch != '.')
        throw this.CreateUnexpectedCharacterException(ch);
      this.ParseNumber(ReadType.Read);
      return true;
    }

    private void ProcessLineFeed()
    {
      ++this._charPos;
      this.OnNewLine(this._charPos);
    }

    private void ProcessCarriageReturn(bool append)
    {
      ++this._charPos;
      if (this.EnsureChars(1, append) && this._chars[this._charPos] == '\n')
        ++this._charPos;
      this.OnNewLine(this._charPos);
    }

    private bool EatWhitespace(bool oneOrMore)
    {
      bool flag1 = false;
      bool flag2 = false;
      while (!flag1)
      {
        char c = this._chars[this._charPos];
        switch (c)
        {
          case char.MinValue:
            if (this._charsUsed == this._charPos)
            {
              if (this.ReadData(false) == 0)
              {
                flag1 = true;
                continue;
              }
              continue;
            }
            ++this._charPos;
            continue;
          case '\n':
            this.ProcessLineFeed();
            continue;
          case '\r':
            this.ProcessCarriageReturn(false);
            continue;
          case ' ':
            flag2 = true;
            ++this._charPos;
            continue;
          default:
            if (!char.IsWhiteSpace(c))
            {
              flag1 = true;
              continue;
            }
            goto case ' ';
        }
      }
      return !oneOrMore | flag2;
    }

    private void ParseConstructor()
    {
      if (!this.MatchValueWithTrailingSeparator("new"))
        throw JsonReaderException.Create((JsonReader) this, "Unexpected content while parsing JSON.");
      this.EatWhitespace(false);
      int charPos1 = this._charPos;
      char c;
      while (true)
      {
        do
        {
          c = this._chars[this._charPos];
          if (c == char.MinValue)
          {
            if (this._charsUsed != this._charPos)
              goto label_6;
          }
          else
            goto label_7;
        }
        while (this.ReadData(true) != 0);
        break;
label_7:
        if (char.IsLetterOrDigit(c))
          ++this._charPos;
        else
          goto label_9;
      }
      throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing constructor.");
label_6:
      int charPos2 = this._charPos;
      ++this._charPos;
      goto label_17;
label_9:
      switch (c)
      {
        case '\n':
          charPos2 = this._charPos;
          this.ProcessLineFeed();
          break;
        case '\r':
          charPos2 = this._charPos;
          this.ProcessCarriageReturn(true);
          break;
        default:
          if (char.IsWhiteSpace(c))
          {
            charPos2 = this._charPos;
            ++this._charPos;
            break;
          }
          if (c != '(')
            throw JsonReaderException.Create((JsonReader) this, "Unexpected character while parsing constructor: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) c));
          charPos2 = this._charPos;
          break;
      }
label_17:
      this._stringReference = new StringReference(this._chars, charPos1, charPos2 - charPos1);
      string str = this._stringReference.ToString();
      this.EatWhitespace(false);
      if (this._chars[this._charPos] != '(')
        throw JsonReaderException.Create((JsonReader) this, "Unexpected character while parsing constructor: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
      ++this._charPos;
      this.ClearRecentString();
      this.SetToken(JsonToken.StartConstructor, (object) str);
    }

    private void ParseNumber(ReadType readType)
    {
      this.ShiftBufferIfNeeded();
      char c = this._chars[this._charPos];
      int charPos = this._charPos;
      this.ReadNumberIntoBuffer();
      this.SetPostValueState(true);
      this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
      bool flag1 = char.IsDigit(c) && this._stringReference.Length == 1;
      bool flag2 = c == '0' && this._stringReference.Length > 1 && (this._stringReference.Chars[this._stringReference.StartIndex + 1] != '.' && this._stringReference.Chars[this._stringReference.StartIndex + 1] != 'e') && this._stringReference.Chars[this._stringReference.StartIndex + 1] != 'E';
      JsonToken newToken;
      object obj;
      switch (readType)
      {
        case ReadType.ReadAsInt32:
          if (flag1)
            obj = (object) ((int) c - 48);
          else if (flag2)
          {
            string str = this._stringReference.ToString();
            try
            {
              obj = (object) (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(str, 16) : Convert.ToInt32(str, 8));
            }
            catch (Exception ex)
            {
              throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid integer.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str), ex);
            }
          }
          else
          {
            int num;
            switch (ConvertUtils.Int32TryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num))
            {
              case ParseResult.Success:
                obj = (object) num;
                break;
              case ParseResult.Overflow:
                throw JsonReaderException.Create((JsonReader) this, "JSON integer {0} is too large or small for an Int32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
              default:
                throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid integer.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
            }
          }
          newToken = JsonToken.Integer;
          break;
        case ReadType.ReadAsString:
          string s1 = this._stringReference.ToString();
          if (flag2)
          {
            try
            {
              if (s1.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                Convert.ToInt64(s1, 16);
              else
                Convert.ToInt64(s1, 8);
            }
            catch (Exception ex)
            {
              throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid number.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s1), ex);
            }
          }
          else if (!double.TryParse(s1, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out double _))
            throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid number.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
          newToken = JsonToken.String;
          obj = (object) s1;
          break;
        case ReadType.ReadAsDecimal:
          if (flag1)
            obj = (object) ((Decimal) c - new Decimal(48));
          else if (flag2)
          {
            string str = this._stringReference.ToString();
            try
            {
              obj = (object) Convert.ToDecimal(str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str, 16) : Convert.ToInt64(str, 8));
            }
            catch (Exception ex)
            {
              throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str), ex);
            }
          }
          else
          {
            Decimal result;
            if (!Decimal.TryParse(this._stringReference.ToString(), NumberStyles.Number | NumberStyles.AllowExponent, (IFormatProvider) CultureInfo.InvariantCulture, out result))
              throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
            obj = (object) result;
          }
          newToken = JsonToken.Float;
          break;
        case ReadType.ReadAsDouble:
          if (flag1)
            obj = (object) ((double) c - 48.0);
          else if (flag2)
          {
            string str = this._stringReference.ToString();
            try
            {
              obj = (object) Convert.ToDouble(str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str, 16) : Convert.ToInt64(str, 8));
            }
            catch (Exception ex)
            {
              throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid double.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str), ex);
            }
          }
          else
          {
            double result;
            if (!double.TryParse(this._stringReference.ToString(), NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
              throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid double.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
            obj = (object) result;
          }
          newToken = JsonToken.Float;
          break;
        default:
          if (flag1)
          {
            obj = (object) ((long) c - 48L);
            newToken = JsonToken.Integer;
            break;
          }
          if (flag2)
          {
            string str = this._stringReference.ToString();
            try
            {
              obj = (object) (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str, 16) : Convert.ToInt64(str, 8));
            }
            catch (Exception ex)
            {
              throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid number.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str), ex);
            }
            newToken = JsonToken.Integer;
            break;
          }
          long num1;
          switch (ConvertUtils.Int64TryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num1))
          {
            case ParseResult.Success:
              obj = (object) num1;
              newToken = JsonToken.Integer;
              break;
            case ParseResult.Overflow:
              throw JsonReaderException.Create((JsonReader) this, "JSON integer {0} is too large or small for an Int64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
            default:
              string s2 = this._stringReference.ToString();
              if (this._floatParseHandling == FloatParseHandling.Decimal)
              {
                Decimal result;
                if (!Decimal.TryParse(s2, NumberStyles.Number | NumberStyles.AllowExponent, (IFormatProvider) CultureInfo.InvariantCulture, out result))
                  throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s2));
                obj = (object) result;
              }
              else
              {
                double result;
                if (!double.TryParse(s2, NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result))
                  throw JsonReaderException.Create((JsonReader) this, "Input string '{0}' is not a valid number.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s2));
                obj = (object) result;
              }
              newToken = JsonToken.Float;
              break;
          }
          break;
      }
      this.ClearRecentString();
      this.SetToken(newToken, obj, false);
    }

    private void ParseComment(bool setToken)
    {
      ++this._charPos;
      if (!this.EnsureChars(1, false))
        throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing comment.");
      bool flag;
      if (this._chars[this._charPos] == '*')
      {
        flag = false;
      }
      else
      {
        if (this._chars[this._charPos] != '/')
          throw JsonReaderException.Create((JsonReader) this, "Error parsing comment. Expected: *, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
        flag = true;
      }
      ++this._charPos;
      int charPos = this._charPos;
      while (true)
      {
        do
        {
          do
          {
            switch (this._chars[this._charPos])
            {
              case char.MinValue:
                if (this._charsUsed == this._charPos)
                  continue;
                goto label_14;
              case '\n':
                goto label_20;
              case '\r':
                goto label_17;
              case '*':
                goto label_15;
              default:
                goto label_23;
            }
          }
          while (this.ReadData(true) != 0);
          if (!flag)
            throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing comment.");
          this.EndComment(setToken, charPos, this._charPos);
          return;
label_14:
          ++this._charPos;
          continue;
label_15:
          ++this._charPos;
        }
        while (flag || !this.EnsureChars(0, true) || this._chars[this._charPos] != '/');
        break;
label_17:
        if (!flag)
        {
          this.ProcessCarriageReturn(true);
          continue;
        }
        goto label_18;
label_20:
        if (!flag)
        {
          this.ProcessLineFeed();
          continue;
        }
        goto label_21;
label_23:
        ++this._charPos;
      }
      this.EndComment(setToken, charPos, this._charPos - 1);
      ++this._charPos;
      return;
label_18:
      this.EndComment(setToken, charPos, this._charPos);
      return;
label_21:
      this.EndComment(setToken, charPos, this._charPos);
    }

    private void EndComment(bool setToken, int initialPosition, int endPosition)
    {
      if (!setToken)
        return;
      this.SetToken(JsonToken.Comment, (object) new string(this._chars, initialPosition, endPosition - initialPosition));
    }

    private bool MatchValue(string value)
    {
      if (!this.EnsureChars(value.Length - 1, true))
      {
        this._charPos = this._charsUsed;
        throw this.CreateUnexpectedEndException();
      }
      for (int index = 0; index < value.Length; ++index)
      {
        if ((int) this._chars[this._charPos + index] != (int) value[index])
        {
          this._charPos += index;
          return false;
        }
      }
      this._charPos += value.Length;
      return true;
    }

    private bool MatchValueWithTrailingSeparator(string value)
    {
      if (!this.MatchValue(value))
        return false;
      return !this.EnsureChars(0, false) || this.IsSeparator(this._chars[this._charPos]) || this._chars[this._charPos] == char.MinValue;
    }

    private bool IsSeparator(char c)
    {
      switch (c)
      {
        case '\t':
        case '\n':
        case '\r':
        case ' ':
          return true;
        case ')':
          if (this.CurrentState == JsonReader.State.Constructor || this.CurrentState == JsonReader.State.ConstructorStart)
            return true;
          break;
        case ',':
        case ']':
        case '}':
          return true;
        case '/':
          if (!this.EnsureChars(1, false))
            return false;
          char ch = this._chars[this._charPos + 1];
          return ch == '*' || ch == '/';
        default:
          if (char.IsWhiteSpace(c))
            return true;
          break;
      }
      return false;
    }

    private void ParseTrue()
    {
      if (!this.MatchValueWithTrailingSeparator(JsonConvert.True))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing boolean value.");
      this.SetToken(JsonToken.Boolean, (object) true);
    }

    private void ParseNull()
    {
      if (!this.MatchValueWithTrailingSeparator(JsonConvert.Null))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing null value.");
      this.SetToken(JsonToken.Null);
    }

    private void ParseUndefined()
    {
      if (!this.MatchValueWithTrailingSeparator(JsonConvert.Undefined))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing undefined value.");
      this.SetToken(JsonToken.Undefined);
    }

    private void ParseFalse()
    {
      if (!this.MatchValueWithTrailingSeparator(JsonConvert.False))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing boolean value.");
      this.SetToken(JsonToken.Boolean, (object) false);
    }

    private object ParseNumberNegativeInfinity(ReadType readType)
    {
      if (this.MatchValueWithTrailingSeparator(JsonConvert.NegativeInfinity))
      {
        if (readType != ReadType.Read)
        {
          if (readType != ReadType.ReadAsString)
          {
            if (readType != ReadType.ReadAsDouble)
              goto label_7;
          }
          else
          {
            this.SetToken(JsonToken.String, (object) JsonConvert.NegativeInfinity);
            return (object) JsonConvert.NegativeInfinity;
          }
        }
        if (this._floatParseHandling == FloatParseHandling.Double)
        {
          this.SetToken(JsonToken.Float, (object) double.NegativeInfinity);
          return (object) double.NegativeInfinity;
        }
label_7:
        throw JsonReaderException.Create((JsonReader) this, "Cannot read -Infinity value.");
      }
      throw JsonReaderException.Create((JsonReader) this, "Error parsing -Infinity value.");
    }

    private object ParseNumberPositiveInfinity(ReadType readType)
    {
      if (this.MatchValueWithTrailingSeparator(JsonConvert.PositiveInfinity))
      {
        if (readType != ReadType.Read)
        {
          if (readType != ReadType.ReadAsString)
          {
            if (readType != ReadType.ReadAsDouble)
              goto label_7;
          }
          else
          {
            this.SetToken(JsonToken.String, (object) JsonConvert.PositiveInfinity);
            return (object) JsonConvert.PositiveInfinity;
          }
        }
        if (this._floatParseHandling == FloatParseHandling.Double)
        {
          this.SetToken(JsonToken.Float, (object) double.PositiveInfinity);
          return (object) double.PositiveInfinity;
        }
label_7:
        throw JsonReaderException.Create((JsonReader) this, "Cannot read Infinity value.");
      }
      throw JsonReaderException.Create((JsonReader) this, "Error parsing Infinity value.");
    }

    private object ParseNumberNaN(ReadType readType)
    {
      if (this.MatchValueWithTrailingSeparator(JsonConvert.NaN))
      {
        if (readType != ReadType.Read)
        {
          if (readType != ReadType.ReadAsString)
          {
            if (readType != ReadType.ReadAsDouble)
              goto label_7;
          }
          else
          {
            this.SetToken(JsonToken.String, (object) JsonConvert.NaN);
            return (object) JsonConvert.NaN;
          }
        }
        if (this._floatParseHandling == FloatParseHandling.Double)
        {
          this.SetToken(JsonToken.Float, (object) double.NaN);
          return (object) double.NaN;
        }
label_7:
        throw JsonReaderException.Create((JsonReader) this, "Cannot read NaN value.");
      }
      throw JsonReaderException.Create((JsonReader) this, "Error parsing NaN value.");
    }

    /// <summary>Changes the state to closed.</summary>
    public override void Close()
    {
      base.Close();
      if (this._chars != null)
      {
        BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
        this._chars = (char[]) null;
      }
      if (this.CloseInput && this._reader != null)
        this._reader.Close();
      this._stringBuffer.Clear(this._arrayPool);
    }

    /// <summary>
    /// Gets a value indicating whether the class can return line information.
    /// </summary>
    /// <returns>
    /// 	<c>true</c> if LineNumber and LinePosition can be provided; otherwise, <c>false</c>.
    /// </returns>
    public bool HasLineInfo()
    {
      return true;
    }

    /// <summary>Gets the current line number.</summary>
    /// <value>
    /// The current line number or 0 if no line information is available (for example, HasLineInfo returns false).
    /// </value>
    public int LineNumber
    {
      get
      {
        return this.CurrentState == JsonReader.State.Start && this.LinePosition == 0 && this.TokenType != JsonToken.Comment ? 0 : this._lineNumber;
      }
    }

    /// <summary>Gets the current line position.</summary>
    /// <value>
    /// The current line position or 0 if no line information is available (for example, HasLineInfo returns false).
    /// </value>
    public int LinePosition
    {
      get
      {
        return this._charPos - this._lineStartPos;
      }
    }
  }
}
