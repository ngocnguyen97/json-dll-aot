// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonMergeSettings
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Specifies the settings used when merging JSON.</summary>
  [Preserve]
  public class JsonMergeSettings
  {
    private MergeArrayHandling _mergeArrayHandling;
    private MergeNullValueHandling _mergeNullValueHandling;

    /// <summary>
    /// Gets or sets the method used when merging JSON arrays.
    /// </summary>
    /// <value>The method used when merging JSON arrays.</value>
    public MergeArrayHandling MergeArrayHandling
    {
      get
      {
        return this._mergeArrayHandling;
      }
      set
      {
        if (value < MergeArrayHandling.Concat || value > MergeArrayHandling.Merge)
          throw new ArgumentOutOfRangeException(nameof (value));
        this._mergeArrayHandling = value;
      }
    }

    /// <summary>
    /// Gets or sets how how null value properties are merged.
    /// </summary>
    /// <value>How null value properties are merged.</value>
    public MergeNullValueHandling MergeNullValueHandling
    {
      get
      {
        return this._mergeNullValueHandling;
      }
      set
      {
        if (value < MergeNullValueHandling.Ignore || value > MergeNullValueHandling.Merge)
          throw new ArgumentOutOfRangeException(nameof (value));
        this._mergeNullValueHandling = value;
      }
    }
  }
}
