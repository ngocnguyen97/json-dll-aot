// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonContainerContract
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="T:System.Type" /> used by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public class JsonContainerContract : JsonContract
  {
    private JsonContract _itemContract;
    private JsonContract _finalItemContract;

    internal JsonContract ItemContract
    {
      get
      {
        return this._itemContract;
      }
      set
      {
        this._itemContract = value;
        if (this._itemContract != null)
          this._finalItemContract = this._itemContract.UnderlyingType.IsSealed() ? this._itemContract : (JsonContract) null;
        else
          this._finalItemContract = (JsonContract) null;
      }
    }

    internal JsonContract FinalItemContract
    {
      get
      {
        return this._finalItemContract;
      }
    }

    /// <summary>
    /// Gets or sets the default collection items <see cref="T:Newtonsoft.Json.JsonConverter" />.
    /// </summary>
    /// <value>The converter.</value>
    public JsonConverter ItemConverter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the collection items preserve object references.
    /// </summary>
    /// <value><c>true</c> if collection items preserve object references; otherwise, <c>false</c>.</value>
    public bool? ItemIsReference { get; set; }

    /// <summary>
    /// Gets or sets the collection item reference loop handling.
    /// </summary>
    /// <value>The reference loop handling.</value>
    public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

    /// <summary>Gets or sets the collection item type name handling.</summary>
    /// <value>The type name handling.</value>
    public TypeNameHandling? ItemTypeNameHandling { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonContainerContract" /> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    internal JsonContainerContract(Type underlyingType)
      : base(underlyingType)
    {
      JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>((object) underlyingType);
      if (cachedAttribute == null)
        return;
      if (cachedAttribute.ItemConverterType != null)
        this.ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(cachedAttribute.ItemConverterType, cachedAttribute.ItemConverterParameters);
      this.ItemIsReference = cachedAttribute._itemIsReference;
      this.ItemReferenceLoopHandling = cachedAttribute._itemReferenceLoopHandling;
      this.ItemTypeNameHandling = cachedAttribute._itemTypeNameHandling;
    }
  }
}
