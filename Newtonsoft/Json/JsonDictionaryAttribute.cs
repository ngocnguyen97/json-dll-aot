// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonDictionaryAttribute
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Instructs the <see cref="T:Newtonsoft.Json.JsonSerializer" /> how to serialize the collection.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
  [Preserve]
  public sealed class JsonDictionaryAttribute : JsonContainerAttribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonDictionaryAttribute" /> class.
    /// </summary>
    public JsonDictionaryAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonDictionaryAttribute" /> class with the specified container Id.
    /// </summary>
    /// <param name="id">The container Id.</param>
    public JsonDictionaryAttribute(string id)
      : base(id)
    {
    }
  }
}
