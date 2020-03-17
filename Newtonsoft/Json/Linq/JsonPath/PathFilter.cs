// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.PathFilter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq.JsonPath
{
  [Preserve]
  internal abstract class PathFilter
  {
    public abstract IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch);

    protected static JToken GetTokenIndex(JToken t, bool errorWhenNoMatch, int index)
    {
      JArray jarray = t as JArray;
      JConstructor jconstructor = t as JConstructor;
      if (jarray != null)
      {
        if (jarray.Count > index)
          return jarray[index];
        if (errorWhenNoMatch)
          throw new JsonException("Index {0} outside the bounds of JArray.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index));
        return (JToken) null;
      }
      if (jconstructor != null)
      {
        if (jconstructor.Count > index)
          return jconstructor[(object) index];
        if (errorWhenNoMatch)
          throw new JsonException("Index {0} outside the bounds of JConstructor.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index));
        return (JToken) null;
      }
      if (errorWhenNoMatch)
        throw new JsonException("Index {0} not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index, (object) t.GetType().Name));
      return (JToken) null;
    }
  }
}
