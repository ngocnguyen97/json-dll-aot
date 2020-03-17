// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.MergeNullValueHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Specifies how null value properties are merged.</summary>
  [Flags]
  [Preserve]
  public enum MergeNullValueHandling
  {
    /// <summary>
    /// The content's null value properties will be ignored during merging.
    /// </summary>
    Ignore = 0,
    /// <summary>The content's null value properties will be merged.</summary>
    Merge = 1,
  }
}
