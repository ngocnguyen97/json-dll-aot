// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ScanFilter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
  [Preserve]
  internal class ScanFilter : PathFilter
  {
    public string Name { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken jtoken1 in current)
      {
        JToken root = jtoken1;
        if (this.Name == null)
          yield return root;
        JToken value = root;
        JToken jtoken2 = root;
        while (true)
        {
          if (jtoken2 != null && jtoken2.HasValues)
          {
            value = jtoken2.First;
          }
          else
          {
            while (value != null && value != root && value == value.Parent.Last)
              value = (JToken) value.Parent;
            if (value != null && value != root)
              value = value.Next;
            else
              break;
          }
          if (value is JProperty jproperty)
          {
            if (jproperty.Name == this.Name)
              yield return jproperty.Value;
          }
          else if (this.Name == null)
            yield return value;
          jtoken2 = (JToken) (value as JContainer);
        }
        value = (JToken) null;
        root = (JToken) null;
      }
    }
  }
}
