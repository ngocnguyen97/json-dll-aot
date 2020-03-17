// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonLoadSettings
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Specifies the settings used when loading JSON.</summary>
  [Preserve]
  public class JsonLoadSettings
  {
    private CommentHandling _commentHandling;
    private LineInfoHandling _lineInfoHandling;

    /// <summary>
    /// Gets or sets how JSON comments are handled when loading JSON.
    /// </summary>
    /// <value>The JSON comment handling.</value>
    public CommentHandling CommentHandling
    {
      get
      {
        return this._commentHandling;
      }
      set
      {
        if (value < CommentHandling.Ignore || value > CommentHandling.Load)
          throw new ArgumentOutOfRangeException(nameof (value));
        this._commentHandling = value;
      }
    }

    /// <summary>
    /// Gets or sets how JSON line info is handled when loading JSON.
    /// </summary>
    /// <value>The JSON line info handling.</value>
    public LineInfoHandling LineInfoHandling
    {
      get
      {
        return this._lineInfoHandling;
      }
      set
      {
        if (value < LineInfoHandling.Ignore || value > LineInfoHandling.Load)
          throw new ArgumentOutOfRangeException(nameof (value));
        this._lineInfoHandling = value;
      }
    }
  }
}
