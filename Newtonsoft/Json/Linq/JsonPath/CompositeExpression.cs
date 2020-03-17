// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.CompositeExpression
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
  [Preserve]
  internal class CompositeExpression : QueryExpression
  {
    public List<QueryExpression> Expressions { get; set; }

    public CompositeExpression()
    {
      this.Expressions = new List<QueryExpression>();
    }

    public override bool IsMatch(JToken t)
    {
      switch (this.Operator)
      {
        case QueryOperator.And:
          foreach (QueryExpression expression in this.Expressions)
          {
            if (!expression.IsMatch(t))
              return false;
          }
          return true;
        case QueryOperator.Or:
          foreach (QueryExpression expression in this.Expressions)
          {
            if (expression.IsMatch(t))
              return true;
          }
          return false;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
