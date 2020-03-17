// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.QueryFilter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
  [Preserve]
  internal class QueryFilter : PathFilter
  {
    public QueryExpression Expression { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (IEnumerable<JToken> jtokens in current)
      {
        foreach (JToken t in jtokens)
        {
          if (this.Expression.IsMatch(t))
            yield return t;
        }
      }
    }
  }
}
