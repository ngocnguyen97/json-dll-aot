// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JTokenType
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Specifies the type of token.</summary>
  [Preserve]
  public enum JTokenType
  {
    /// <summary>No token type has been set.</summary>
    None,
    /// <summary>A JSON object.</summary>
    Object,
    /// <summary>A JSON array.</summary>
    Array,
    /// <summary>A JSON constructor.</summary>
    Constructor,
    /// <summary>A JSON object property.</summary>
    Property,
    /// <summary>A comment.</summary>
    Comment,
    /// <summary>An integer value.</summary>
    Integer,
    /// <summary>A float value.</summary>
    Float,
    /// <summary>A string value.</summary>
    String,
    /// <summary>A boolean value.</summary>
    Boolean,
    /// <summary>A null value.</summary>
    Null,
    /// <summary>An undefined value.</summary>
    Undefined,
    /// <summary>A date value.</summary>
    Date,
    /// <summary>A raw JSON value.</summary>
    Raw,
    /// <summary>A collection of bytes value.</summary>
    Bytes,
    /// <summary>A Guid value.</summary>
    Guid,
    /// <summary>A Uri value.</summary>
    Uri,
    /// <summary>A TimeSpan value.</summary>
    TimeSpan,
  }
}
