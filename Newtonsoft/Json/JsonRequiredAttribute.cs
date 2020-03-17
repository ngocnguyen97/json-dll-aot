// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonRequiredAttribute
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Instructs the <see cref="T:Newtonsoft.Json.JsonSerializer" /> to always serialize the member, and require the member has a value.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  [Preserve]
  public sealed class JsonRequiredAttribute : Attribute
  {
  }
}
