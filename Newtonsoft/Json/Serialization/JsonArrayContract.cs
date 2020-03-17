// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonArrayContract
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="T:System.Type" /> used by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public class JsonArrayContract : JsonContainerContract
  {
    private readonly Type _genericCollectionDefinitionType;
    private Type _genericWrapperType;
    private ObjectConstructor<object> _genericWrapperCreator;
    private Func<object> _genericTemporaryCollectionCreator;
    private readonly ConstructorInfo _parameterizedConstructor;
    private ObjectConstructor<object> _parameterizedCreator;
    private ObjectConstructor<object> _overrideCreator;

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the collection items.
    /// </summary>
    /// <value>The <see cref="T:System.Type" /> of the collection items.</value>
    public Type CollectionItemType { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the collection type is a multidimensional array.
    /// </summary>
    /// <value><c>true</c> if the collection type is a multidimensional array; otherwise, <c>false</c>.</value>
    public bool IsMultidimensionalArray { get; private set; }

    internal bool IsArray { get; private set; }

    internal bool ShouldCreateWrapper { get; private set; }

    internal bool CanDeserialize { get; private set; }

    internal ObjectConstructor<object> ParameterizedCreator
    {
      get
      {
        if (this._parameterizedCreator == null)
          this._parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) this._parameterizedConstructor);
        return this._parameterizedCreator;
      }
    }

    /// <summary>
    /// Gets or sets the function used to create the object. When set this function will override <see cref="P:Newtonsoft.Json.Serialization.JsonContract.DefaultCreator" />.
    /// </summary>
    /// <value>The function used to create the object.</value>
    public ObjectConstructor<object> OverrideCreator
    {
      get
      {
        return this._overrideCreator;
      }
      set
      {
        this._overrideCreator = value;
        this.CanDeserialize = true;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the creator has a parameter with the collection values.
    /// </summary>
    /// <value><c>true</c> if the creator has a parameter with the collection values; otherwise, <c>false</c>.</value>
    public bool HasParameterizedCreator { get; set; }

    internal bool HasParameterizedCreatorInternal
    {
      get
      {
        return this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != null;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonArrayContract" /> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    public JsonArrayContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Array;
      this.IsArray = this.CreatedType.IsArray;
      bool flag;
      Type implementingType;
      if (this.IsArray)
      {
        this.CollectionItemType = ReflectionUtils.GetCollectionItemType(this.UnderlyingType);
        this.IsReadOnlyOrFixedSize = true;
        this._genericCollectionDefinitionType = typeof (List<>).MakeGenericType(this.CollectionItemType);
        flag = true;
        this.IsMultidimensionalArray = this.IsArray && this.UnderlyingType.GetArrayRank() > 1;
      }
      else if (typeof (IList).IsAssignableFrom(underlyingType))
      {
        this.CollectionItemType = !ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (ICollection<>), out this._genericCollectionDefinitionType) ? ReflectionUtils.GetCollectionItemType(underlyingType) : this._genericCollectionDefinitionType.GetGenericArguments()[0];
        if (underlyingType == typeof (IList))
          this.CreatedType = typeof (List<object>);
        if (this.CollectionItemType != null)
          this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
        this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(underlyingType, typeof (ReadOnlyCollection<>));
        flag = true;
      }
      else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (ICollection<>), out this._genericCollectionDefinitionType))
      {
        this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
        if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof (ICollection<>)) || ReflectionUtils.IsGenericDefinition(underlyingType, typeof (IList<>)))
          this.CreatedType = typeof (List<>).MakeGenericType(this.CollectionItemType);
        this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
        flag = true;
        this.ShouldCreateWrapper = true;
      }
      else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (IEnumerable<>), out implementingType))
      {
        this.CollectionItemType = implementingType.GetGenericArguments()[0];
        if (ReflectionUtils.IsGenericDefinition(this.UnderlyingType, typeof (IEnumerable<>)))
          this.CreatedType = typeof (List<>).MakeGenericType(this.CollectionItemType);
        this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
        if (underlyingType.IsGenericType() && underlyingType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
        {
          this._genericCollectionDefinitionType = implementingType;
          this.IsReadOnlyOrFixedSize = false;
          this.ShouldCreateWrapper = false;
          flag = true;
        }
        else
        {
          this._genericCollectionDefinitionType = typeof (List<>).MakeGenericType(this.CollectionItemType);
          this.IsReadOnlyOrFixedSize = true;
          this.ShouldCreateWrapper = true;
          flag = this.HasParameterizedCreatorInternal;
        }
      }
      else
      {
        flag = false;
        this.ShouldCreateWrapper = true;
      }
      this.CanDeserialize = flag;
      if (this.CollectionItemType == null || !ReflectionUtils.IsNullableType(this.CollectionItemType) || !ReflectionUtils.InheritsGenericDefinition(this.CreatedType, typeof (List<>), out implementingType) && (!this.IsArray || this.IsMultidimensionalArray))
        return;
      this.ShouldCreateWrapper = true;
    }

    internal IWrappedCollection CreateWrapper(object list)
    {
      if (this._genericWrapperCreator == null)
      {
        this._genericWrapperType = typeof (CollectionWrapper<>).MakeGenericType(this.CollectionItemType);
        Type type;
        if (ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof (List<>)) || this._genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
          type = typeof (ICollection<>).MakeGenericType(this.CollectionItemType);
        else
          type = this._genericCollectionDefinitionType;
        this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) this._genericWrapperType.GetConstructor(new Type[1]
        {
          type
        }));
      }
      return (IWrappedCollection) this._genericWrapperCreator(new object[1]
      {
        list
      });
    }

    internal IList CreateTemporaryCollection()
    {
      if (this._genericTemporaryCollectionCreator == null)
        this._genericTemporaryCollectionCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(typeof (List<>).MakeGenericType(this.IsMultidimensionalArray || this.CollectionItemType == null ? typeof (object) : this.CollectionItemType));
      return (IList) this._genericTemporaryCollectionCreator();
    }
  }
}
