// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StringBuffer
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Utilities
{
  /// <summary>
  /// Builds a string. Unlike StringBuilder this class lets you reuse it's internal buffer.
  /// </summary>
  [Preserve]
  internal struct StringBuffer
  {
    private char[] _buffer;
    private int _position;

    public int Position
    {
      get
      {
        return this._position;
      }
      set
      {
        this._position = value;
      }
    }

    public bool IsEmpty
    {
      get
      {
        return this._buffer == null;
      }
    }

    public StringBuffer(IArrayPool<char> bufferPool, int initalSize)
      : this(BufferUtils.RentBuffer(bufferPool, initalSize))
    {
    }

    private StringBuffer(char[] buffer)
    {
      this._buffer = buffer;
      this._position = 0;
    }

    public void Append(IArrayPool<char> bufferPool, char value)
    {
      if (this._position == this._buffer.Length)
        this.EnsureSize(bufferPool, 1);
      this._buffer[this._position++] = value;
    }

    public void Append(IArrayPool<char> bufferPool, char[] buffer, int startIndex, int count)
    {
      if (this._position + count >= this._buffer.Length)
        this.EnsureSize(bufferPool, count);
      Array.Copy((Array) buffer, startIndex, (Array) this._buffer, this._position, count);
      this._position += count;
    }

    public void Clear(IArrayPool<char> bufferPool)
    {
      if (this._buffer != null)
      {
        BufferUtils.ReturnBuffer(bufferPool, this._buffer);
        this._buffer = (char[]) null;
      }
      this._position = 0;
    }

    private void EnsureSize(IArrayPool<char> bufferPool, int appendLength)
    {
      char[] chArray = BufferUtils.RentBuffer(bufferPool, (this._position + appendLength) * 2);
      if (this._buffer != null)
      {
        Array.Copy((Array) this._buffer, (Array) chArray, this._position);
        BufferUtils.ReturnBuffer(bufferPool, this._buffer);
      }
      this._buffer = chArray;
    }

    public override string ToString()
    {
      return this.ToString(0, this._position);
    }

    public string ToString(int start, int length)
    {
      return new string(this._buffer, start, length);
    }

    public char[] InternalBuffer
    {
      get
      {
        return this._buffer;
      }
    }
  }
}
