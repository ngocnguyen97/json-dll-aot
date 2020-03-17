// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonDictionaryContract
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="T:System.Type" /> used by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public class JsonDictionaryContract : JsonContainerContract
  {
    private readonly Type _genericCollectionDefinitionType;
    private Type _genericWrapperType;
    private ObjectConstructor<object> _genericWrapperCreator;
    private Func<object> _genericTemporaryDictionaryCreator;
    private readonly ConstructorInfo _parameterizedConstructor;
    private ObjectConstructor<object> _overrideCreator;
    private ObjectConstructor<object> _parameterizedCreator;

    /// <summary>Gets or sets the property name resolver.</summary>
    /// <value>The property name resolver.</value>
    [Obsolete("PropertyNameResolver is obsolete. Use DictionaryKeyResolver instead.")]
    public Func<string, string> PropertyNameResolver
    {
      get
      {
        return this.DictionaryKeyResolver;
      }
      set
      {
        this.DictionaryKeyResolver = value;
      }
    }

    /// <summary>Gets or sets the dictionary key resolver.</summary>
    /// <value>The dictionary key resolver.</value>
    public Func<string, string> DictionaryKeyResolver { get; set; }

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the dictionary keys.
    /// </summary>
    /// <value>The <see cref="T:System.Type" /> of the dictionary keys.</value>
    public Type DictionaryKeyType { get; private set; }

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the dictionary values.
    /// </summary>
    /// <value>The <see cref="T:System.Type" /> of the dictionary values.</value>
    public Type DictionaryValueType { get; private set; }

    internal JsonContract KeyContract { get; set; }

    internal bool ShouldCreateWrapper { get; private set; }

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
      }
    }

    /// <summary>
    /// Gets a value indicating whether the creator has a parameter with the dictionary values.
    /// </summary>
    /// <value><c>true</c> if the creator has a parameter with the dictionary values; otherwise, <c>false</c>.</value>
    public bool HasParameterizedCreator { get; set; }

    internal bool HasParameterizedCreatorInternal
    {
      get
      {
        return this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != null;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonDictionaryContract" /> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    public JsonDictionaryContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Dictionary;
      Type keyType;
      Type valueType;
      if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (IDictionary<,>), out this._genericCollectionDefinitionType))
      {
        keyType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
        valueType = this._genericCollectionDefinitionType.GetGenericArguments()[1];
        if (ReflectionUtils.IsGenericDefinition(this.UnderlyingType, typeof (IDictionary<,>)))
          this.CreatedType = typeof (Dictionary<,>).MakeGenericType(keyType, valueType);
      }
      else
      {
        ReflectionUtils.GetDictionaryKeyValueTypes(this.UnderlyingType, out keyType, out valueType);
        if (this.UnderlyingType == typeof (IDictionary))
          this.CreatedType = typeof (Dictionary<object, object>);
      }
      if (keyType != null && valueType != null)
        this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.CreatedType, typeof (KeyValuePair<,>).MakeGenericType(keyType, valueType), typeof (IDictionary<,>).MakeGenericType(keyType, valueType));
      this.ShouldCreateWrapper = !typeof (IDictionary).IsAssignableFrom(this.CreatedType);
      this.DictionaryKeyType = keyType;
      this.DictionaryValueType = valueType;
      if (this.DictionaryValueType == null || !ReflectionUtils.IsNullableType(this.DictionaryValueType) || !ReflectionUtils.InheritsGenericDefinition(this.CreatedType, typeof (Dictionary<,>), out Type _))
        return;
      this.ShouldCreateWrapper = true;
    }

    internal IWrappedDictionary CreateWrapper(object dictionary)
    {
      if (this._genericWrapperCreator == null)
      {
        this._genericWrapperType = typeof (DictionaryWrapper<,>).MakeGenericType(this.DictionaryKeyType, this.DictionaryValueType);
        this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) this._genericWrapperType.GetConstructor(new Type[1]
        {
          this._genericCollectionDefinitionType
        }));
      }
      return (IWrappedDictionary) this._genericWrapperCreator(new object[1]
      {
        dictionary
      });
    }

    internal IDictionary CreateTemporaryDictionary()
    {
      if (this._genericTemporaryDictionaryCreator == null)
        this._genericTemporaryDictionaryCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(typeof (Dictionary<,>).MakeGenericType(this.DictionaryKeyType ?? typeof (object), this.DictionaryValueType ?? typeof (object)));
      return (IDictionary) this._genericTemporaryDictionaryCreator();
    }
  }
}
