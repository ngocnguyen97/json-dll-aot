// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonObjectContract
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="T:System.Type" /> used by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public class JsonObjectContract : JsonContainerContract
  {
    internal bool ExtensionDataIsJToken;
    private bool? _hasRequiredOrDefaultValueProperties;
    private ConstructorInfo _parametrizedConstructor;
    private ConstructorInfo _overrideConstructor;
    private ObjectConstructor<object> _overrideCreator;
    private ObjectConstructor<object> _parameterizedCreator;
    private JsonPropertyCollection _creatorParameters;
    private Type _extensionDataValueType;

    /// <summary>Gets or sets the object member serialization.</summary>
    /// <value>The member object serialization.</value>
    public MemberSerialization MemberSerialization { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether the object's properties are required.
    /// </summary>
    /// <value>
    /// 	A value indicating whether the object's properties are required.
    /// </value>
    public Required? ItemRequired { get; set; }

    /// <summary>Gets the object's properties.</summary>
    /// <value>The object's properties.</value>
    public JsonPropertyCollection Properties { get; private set; }

    /// <summary>
    /// Gets the constructor parameters required for any non-default constructor
    /// </summary>
    [Obsolete("ConstructorParameters is obsolete. Use CreatorParameters instead.")]
    public JsonPropertyCollection ConstructorParameters
    {
      get
      {
        return this.CreatorParameters;
      }
    }

    /// <summary>
    /// Gets a collection of <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> instances that define the parameters used with <see cref="P:Newtonsoft.Json.Serialization.JsonObjectContract.OverrideCreator" />.
    /// </summary>
    public JsonPropertyCollection CreatorParameters
    {
      get
      {
        if (this._creatorParameters == null)
          this._creatorParameters = new JsonPropertyCollection(this.UnderlyingType);
        return this._creatorParameters;
      }
    }

    /// <summary>
    /// Gets or sets the override constructor used to create the object.
    /// This is set when a constructor is marked up using the
    /// JsonConstructor attribute.
    /// </summary>
    /// <value>The override constructor.</value>
    [Obsolete("OverrideConstructor is obsolete. Use OverrideCreator instead.")]
    public ConstructorInfo OverrideConstructor
    {
      get
      {
        return this._overrideConstructor;
      }
      set
      {
        this._overrideConstructor = value;
        this._overrideCreator = value != null ? JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) value) : (ObjectConstructor<object>) null;
      }
    }

    /// <summary>
    /// Gets or sets the parametrized constructor used to create the object.
    /// </summary>
    /// <value>The parametrized constructor.</value>
    [Obsolete("ParametrizedConstructor is obsolete. Use OverrideCreator instead.")]
    public ConstructorInfo ParametrizedConstructor
    {
      get
      {
        return this._parametrizedConstructor;
      }
      set
      {
        this._parametrizedConstructor = value;
        this._parameterizedCreator = value != null ? JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) value) : (ObjectConstructor<object>) null;
      }
    }

    /// <summary>
    /// Gets or sets the function used to create the object. When set this function will override <see cref="P:Newtonsoft.Json.Serialization.JsonContract.DefaultCreator" />.
    /// This function is called with a collection of arguments which are defined by the <see cref="P:Newtonsoft.Json.Serialization.JsonObjectContract.CreatorParameters" /> collection.
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
        this._overrideConstructor = (ConstructorInfo) null;
      }
    }

    internal ObjectConstructor<object> ParameterizedCreator
    {
      get
      {
        return this._parameterizedCreator;
      }
    }

    /// <summary>Gets or sets the extension data setter.</summary>
    public ExtensionDataSetter ExtensionDataSetter { get; set; }

    /// <summary>Gets or sets the extension data getter.</summary>
    public ExtensionDataGetter ExtensionDataGetter { get; set; }

    /// <summary>Gets or sets the extension data value type.</summary>
    public Type ExtensionDataValueType
    {
      get
      {
        return this._extensionDataValueType;
      }
      set
      {
        this._extensionDataValueType = value;
        this.ExtensionDataIsJToken = value != null && typeof (JToken).IsAssignableFrom(value);
      }
    }

    internal bool HasRequiredOrDefaultValueProperties
    {
      get
      {
        if (!this._hasRequiredOrDefaultValueProperties.HasValue)
        {
          this._hasRequiredOrDefaultValueProperties = new bool?(false);
          if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
          {
            this._hasRequiredOrDefaultValueProperties = new bool?(true);
          }
          else
          {
            foreach (JsonProperty property in (Collection<JsonProperty>) this.Properties)
            {
              if (property.Required == Required.Default)
              {
                DefaultValueHandling? defaultValueHandling1 = property.DefaultValueHandling;
                DefaultValueHandling? nullable = defaultValueHandling1.HasValue ? new DefaultValueHandling?(defaultValueHandling1.GetValueOrDefault() & DefaultValueHandling.Populate) : new DefaultValueHandling?();
                DefaultValueHandling defaultValueHandling2 = DefaultValueHandling.Populate;
                if ((nullable.GetValueOrDefault() == defaultValueHandling2 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                  continue;
              }
              this._hasRequiredOrDefaultValueProperties = new bool?(true);
              break;
            }
          }
        }
        return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonObjectContract" /> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    public JsonObjectContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Object;
      this.Properties = new JsonPropertyCollection(this.UnderlyingType);
    }

    internal object GetUninitializedObject()
    {
      if (!JsonTypeReflector.FullyTrusted)
        throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.NonNullableUnderlyingType));
      return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
    }
  }
}
