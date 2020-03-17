// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ThreadSafeStore`2
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal class ThreadSafeStore<TKey, TValue>
  {
    private readonly object _lock = new object();
    private Dictionary<TKey, TValue> _store;
    private readonly Func<TKey, TValue> _creator;

    [Preserve]
    public ThreadSafeStore(Func<TKey, TValue> creator)
    {
      if (creator == null)
        throw new ArgumentNullException(nameof (creator));
      this._creator = creator;
      this._store = new Dictionary<TKey, TValue>();
    }

    [Preserve]
    public TValue Get(TKey key)
    {
      TValue obj;
      return !this._store.TryGetValue(key, out obj) ? this.AddValue(key) : obj;
    }

    [Preserve]
    private TValue AddValue(TKey key)
    {
      TValue obj1 = this._creator(key);
      lock (this._lock)
      {
        if (this._store == null)
        {
          this._store = new Dictionary<TKey, TValue>();
          this._store[key] = obj1;
        }
        else
        {
          TValue obj2;
          if (this._store.TryGetValue(key, out obj2))
            return obj2;
          this._store = new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>) this._store)
          {
            [key] = obj1
          };
        }
        return obj1;
      }
    }
  }
}
