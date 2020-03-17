// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ArrayIndexFilter
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
  internal class ArrayIndexFilter : PathFilter
  {
    public int? Index { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken jtoken1 in current)
      {
        JToken t = jtoken1;
        if (this.Index.HasValue)
        {
          JToken tokenIndex = PathFilter.GetTokenIndex(t, errorWhenNoMatch, this.Index.GetValueOrDefault());
          if (tokenIndex != null)
            yield return tokenIndex;
        }
        else if (t is JArray || t is JConstructor)
        {
          foreach (JToken jtoken2 in (IEnumerable<JToken>) t)
            yield return jtoken2;
        }
        else if (errorWhenNoMatch)
          throw new JsonException("Index * not valid on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) t.GetType().Name));
        t = (JToken) null;
      }
    }
  }
}
