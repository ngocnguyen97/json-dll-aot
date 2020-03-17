// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CachedAttributeGetter`1
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Serialization
{
  [Preserve]
  internal static class CachedAttributeGetter<T> where T : Attribute
  {
    private static readonly ThreadSafeStore<object, T> TypeAttributeCache = new ThreadSafeStore<object, T>(new Func<object, T>(JsonTypeReflector.GetAttribute<T>));

    public static T GetAttribute(object type)
    {
      return CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
    }
  }
}
