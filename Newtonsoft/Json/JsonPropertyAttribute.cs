// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonPropertyAttribute
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Instructs the <see cref="T:Newtonsoft.Json.JsonSerializer" /> to always serialize the member with the specified name.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
  [Preserve]
  public sealed class JsonPropertyAttribute : Attribute
  {
    internal NullValueHandling? _nullValueHandling;
    internal DefaultValueHandling? _defaultValueHandling;
    internal ReferenceLoopHandling? _referenceLoopHandling;
    internal ObjectCreationHandling? _objectCreationHandling;
    internal TypeNameHandling? _typeNameHandling;
    internal bool? _isReference;
    internal int? _order;
    internal Required? _required;
    internal bool? _itemIsReference;
    internal ReferenceLoopHandling? _itemReferenceLoopHandling;
    internal TypeNameHandling? _itemTypeNameHandling;

    /// <summary>
    /// Gets or sets the converter used when serializing the property's collection items.
    /// </summary>
    /// <value>The collection's items converter.</value>
    public Type ItemConverterType { get; set; }

    /// <summary>
    /// The parameter list to use when constructing the JsonConverter described by ItemConverterType.
    /// If null, the default constructor is used.
    /// When non-null, there must be a constructor defined in the JsonConverter that exactly matches the number,
    /// order, and type of these parameters.
    /// </summary>
    /// <example>
    /// [JsonProperty(ItemConverterType = typeof(MyContainerConverter), ItemConverterParameters = new object[] { 123, "Four" })]
    /// </example>
    public object[] ItemConverterParameters { get; set; }

    /// <summary>
    /// Gets or sets the null value handling used when serializing this property.
    /// </summary>
    /// <value>The null value handling.</value>
    public NullValueHandling NullValueHandling
    {
      get
      {
        return this._nullValueHandling ?? NullValueHandling.Include;
      }
      set
      {
        this._nullValueHandling = new NullValueHandling?(value);
      }
    }

    /// <summary>
    /// Gets or sets the default value handling used when serializing this property.
    /// </summary>
    /// <value>The default value handling.</value>
    public DefaultValueHandling DefaultValueHandling
    {
      get
      {
        return this._defaultValueHandling ?? DefaultValueHandling.Include;
      }
      set
      {
        this._defaultValueHandling = new DefaultValueHandling?(value);
      }
    }

    /// <summary>
    /// Gets or sets the reference loop handling used when serializing this property.
    /// </summary>
    /// <value>The reference loop handling.</value>
    public ReferenceLoopHandling ReferenceLoopHandling
    {
      get
      {
        return this._referenceLoopHandling ?? ReferenceLoopHandling.Error;
      }
      set
      {
        this._referenceLoopHandling = new ReferenceLoopHandling?(value);
      }
    }

    /// <summary>
    /// Gets or sets the object creation handling used when deserializing this property.
    /// </summary>
    /// <value>The object creation handling.</value>
    public ObjectCreationHandling ObjectCreationHandling
    {
      get
      {
        return this._objectCreationHandling ?? ObjectCreationHandling.Auto;
      }
      set
      {
        this._objectCreationHandling = new ObjectCreationHandling?(value);
      }
    }

    /// <summary>
    /// Gets or sets the type name handling used when serializing this property.
    /// </summary>
    /// <value>The type name handling.</value>
    public TypeNameHandling TypeNameHandling
    {
      get
      {
        return this._typeNameHandling ?? TypeNameHandling.None;
      }
      set
      {
        this._typeNameHandling = new TypeNameHandling?(value);
      }
    }

    /// <summary>
    /// Gets or sets whether this property's value is serialized as a reference.
    /// </summary>
    /// <value>Whether this property's value is serialized as a reference.</value>
    public bool IsReference
    {
      get
      {
        return this._isReference ?? false;
      }
      set
      {
        this._isReference = new bool?(value);
      }
    }

    /// <summary>Gets or sets the order of serialization of a member.</summary>
    /// <value>The numeric order of serialization.</value>
    public int Order
    {
      get
      {
        return this._order ?? 0;
      }
      set
      {
        this._order = new int?(value);
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this property is required.
    /// </summary>
    /// <value>A value indicating whether this property is required.</value>
    public Required Required
    {
      get
      {
        return this._required ?? Required.Default;
      }
      set
      {
        this._required = new Required?(value);
      }
    }

    /// <summary>Gets or sets the name of the property.</summary>
    /// <value>The name of the property.</value>
    public string PropertyName { get; set; }

    /// <summary>
    /// Gets or sets the the reference loop handling used when serializing the property's collection items.
    /// </summary>
    /// <value>The collection's items reference loop handling.</value>
    public ReferenceLoopHandling ItemReferenceLoopHandling
    {
      get
      {
        return this._itemReferenceLoopHandling ?? ReferenceLoopHandling.Error;
      }
      set
      {
        this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
      }
    }

    /// <summary>
    /// Gets or sets the the type name handling used when serializing the property's collection items.
    /// </summary>
    /// <value>The collection's items type name handling.</value>
    public TypeNameHandling ItemTypeNameHandling
    {
      get
      {
        return this._itemTypeNameHandling ?? TypeNameHandling.None;
      }
      set
      {
        this._itemTypeNameHandling = new TypeNameHandling?(value);
      }
    }

    /// <summary>
    /// Gets or sets whether this property's collection items are serialized as a reference.
    /// </summary>
    /// <value>Whether this property's collection items are serialized as a reference.</value>
    public bool ItemIsReference
    {
      get
      {
        return this._itemIsReference ?? false;
      }
      set
      {
        this._itemIsReference = new bool?(value);
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonPropertyAttribute" /> class.
    /// </summary>
    public JsonPropertyAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonPropertyAttribute" /> class with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    public JsonPropertyAttribute(string propertyName)
    {
      this.PropertyName = propertyName;
    }
  }
}
