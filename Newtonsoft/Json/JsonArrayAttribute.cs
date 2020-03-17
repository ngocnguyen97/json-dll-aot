// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonArrayAttribute
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Instructs the <see cref="T:Newtonsoft.Json.JsonSerializer" /> how to serialize the collection.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
  [Preserve]
  public sealed class JsonArrayAttribute : JsonContainerAttribute
  {
    private bool _allowNullItems;

    /// <summary>
    /// Gets or sets a value indicating whether null items are allowed in the collection.
    /// </summary>
    /// <value><c>true</c> if null items are allowed in the collection; otherwise, <c>false</c>.</value>
    public bool AllowNullItems
    {
      get
      {
        return this._allowNullItems;
      }
      set
      {
        this._allowNullItems = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonArrayAttribute" /> class.
    /// </summary>
    public JsonArrayAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonObjectAttribute" /> class with a flag indicating whether the array can contain null items
    /// </summary>
    /// <param name="allowNullItems">A flag indicating whether the array can contain null items.</param>
    public JsonArrayAttribute(bool allowNullItems)
    {
      this._allowNullItems = allowNullItems;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonArrayAttribute" /> class with the specified container Id.
    /// </summary>
    /// <param name="id">The container Id.</param>
    public JsonArrayAttribute(string id)
      : base(id)
    {
    }
  }
}
