// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonPrimitiveContract
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="T:System.Type" /> used by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  [Preserve]
  public class JsonPrimitiveContract : JsonContract
  {
    private static readonly Dictionary<Type, ReadType> ReadTypeMap = new Dictionary<Type, ReadType>()
    {
      [typeof (byte[])] = ReadType.ReadAsBytes,
      [typeof (byte)] = ReadType.ReadAsInt32,
      [typeof (short)] = ReadType.ReadAsInt32,
      [typeof (int)] = ReadType.ReadAsInt32,
      [typeof (Decimal)] = ReadType.ReadAsDecimal,
      [typeof (bool)] = ReadType.ReadAsBoolean,
      [typeof (string)] = ReadType.ReadAsString,
      [typeof (DateTime)] = ReadType.ReadAsDateTime,
      [typeof (DateTimeOffset)] = ReadType.ReadAsDateTimeOffset,
      [typeof (float)] = ReadType.ReadAsDouble,
      [typeof (double)] = ReadType.ReadAsDouble
    };

    internal PrimitiveTypeCode TypeCode { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonPrimitiveContract" /> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    public JsonPrimitiveContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Primitive;
      this.TypeCode = ConvertUtils.GetTypeCode(underlyingType);
      this.IsReadOnlyOrFixedSize = true;
      ReadType readType;
      if (!JsonPrimitiveContract.ReadTypeMap.TryGetValue(this.NonNullableUnderlyingType, out readType))
        return;
      this.InternalReadType = readType;
    }
  }
}
