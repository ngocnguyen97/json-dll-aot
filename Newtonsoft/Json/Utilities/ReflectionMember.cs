// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ReflectionMember
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal class ReflectionMember
  {
    public Type MemberType { get; set; }

    public Func<object, object> Getter { get; set; }

    public Action<object, object> Setter { get; set; }
  }
}
