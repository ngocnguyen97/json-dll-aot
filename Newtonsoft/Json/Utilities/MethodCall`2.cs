﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.MethodCall`2
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal delegate TResult MethodCall<T, TResult>(T target, params object[] args);
}
