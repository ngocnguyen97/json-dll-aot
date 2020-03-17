// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.FieldFilter
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
  internal class FieldFilter : PathFilter
  {
    public string Name { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken jtoken1 in current)
      {
        JToken t = jtoken1;
        if (t is JObject o)
        {
          if (this.Name != null)
          {
            JToken jtoken2 = o[this.Name];
            if (jtoken2 != null)
              yield return jtoken2;
            else if (errorWhenNoMatch)
              throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.Name));
          }
          else
          {
            foreach (KeyValuePair<string, JToken> keyValuePair in o)
              yield return keyValuePair.Value;
          }
        }
        else if (errorWhenNoMatch)
          throw new JsonException("Property '{0}' not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) (this.Name ?? "*"), (object) t.GetType().Name));
        o = (JObject) null;
        t = (JToken) null;
      }
    }
  }
}
