// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverterAttribute
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Instructs the <see cref="T:Newtonsoft.Json.JsonSerializer" /> to use the specified <see cref="T:Newtonsoft.Json.JsonConverter" /> when serializing the member or class.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
  [Preserve]
  public sealed class JsonConverterAttribute : Attribute
  {
    private readonly Type _converterType;

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the converter.
    /// </summary>
    /// <value>The <see cref="T:System.Type" /> of the converter.</value>
    public Type ConverterType
    {
      get
      {
        return this._converterType;
      }
    }

    /// <summary>
    /// The parameter list to use when constructing the JsonConverter described by ConverterType.
    /// If null, the default constructor is used.
    /// </summary>
    public object[] ConverterParameters { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonConverterAttribute" /> class.
    /// </summary>
    /// <param name="converterType">Type of the converter.</param>
    public JsonConverterAttribute(Type converterType)
    {
      if (converterType == null)
        throw new ArgumentNullException(nameof (converterType));
      this._converterType = converterType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonConverterAttribute" /> class.
    /// </summary>
    /// <param name="converterType">Type of the converter.</param>
    /// <param name="converterParameters">Parameter list to use when constructing the JsonConverter. Can be null.</param>
    public JsonConverterAttribute(Type converterType, params object[] converterParameters)
      : this(converterType)
    {
      this.ConverterParameters = converterParameters;
    }
  }
}
