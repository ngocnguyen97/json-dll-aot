// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.MergeArrayHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Specifies how JSON arrays are merged together.</summary>
  [Preserve]
  public enum MergeArrayHandling
  {
    /// <summary>Concatenate arrays.</summary>
    Concat,
    /// <summary>Union arrays, skipping items that already exist.</summary>
    Union,
    /// <summary>Replace all array items.</summary>
    Replace,
    /// <summary>Merge array items together, matched by index.</summary>
    Merge,
  }
}
