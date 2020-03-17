// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StringReference
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal struct StringReference
  {
    private readonly char[] _chars;
    private readonly int _startIndex;
    private readonly int _length;

    public char this[int i]
    {
      get
      {
        return this._chars[i];
      }
    }

    public char[] Chars
    {
      get
      {
        return this._chars;
      }
    }

    public int StartIndex
    {
      get
      {
        return this._startIndex;
      }
    }

    public int Length
    {
      get
      {
        return this._length;
      }
    }

    public StringReference(char[] chars, int startIndex, int length)
    {
      this._chars = chars;
      this._startIndex = startIndex;
      this._length = length;
    }

    public override string ToString()
    {
      return new string(this._chars, this._startIndex, this._length);
    }
  }
}
