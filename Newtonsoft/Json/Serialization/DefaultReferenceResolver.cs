// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultReferenceResolver
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Serialization
{
  [Preserve]
  internal class DefaultReferenceResolver : IReferenceResolver
  {
    private int _referenceCount;

    private BidirectionalDictionary<string, object> GetMappings(
      object context)
    {
      JsonSerializerInternalBase serializerInternalBase;
      switch (context)
      {
        case JsonSerializerInternalBase _:
          serializerInternalBase = (JsonSerializerInternalBase) context;
          break;
        case JsonSerializerProxy _:
          serializerInternalBase = ((JsonSerializerProxy) context).GetInternalSerializer();
          break;
        default:
          throw new JsonException("The DefaultReferenceResolver can only be used internally.");
      }
      return serializerInternalBase.DefaultReferenceMappings;
    }

    public object ResolveReference(object context, string reference)
    {
      object second;
      this.GetMappings(context).TryGetByFirst(reference, out second);
      return second;
    }

    public string GetReference(object context, object value)
    {
      BidirectionalDictionary<string, object> mappings = this.GetMappings(context);
      string first;
      if (!mappings.TryGetBySecond(value, out first))
      {
        ++this._referenceCount;
        first = this._referenceCount.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        mappings.Set(first, value);
      }
      return first;
    }

    public void AddReference(object context, string reference, object value)
    {
      this.GetMappings(context).Set(reference, value);
    }

    public bool IsReferenced(object context, object value)
    {
      return this.GetMappings(context).TryGetBySecond(value, out string _);
    }
  }
}
