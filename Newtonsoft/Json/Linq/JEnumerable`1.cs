﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JEnumerable`1
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Linq
{
  /// <summary>
  /// Represents a collection of <see cref="T:Newtonsoft.Json.Linq.JToken" /> objects.
  /// </summary>
  /// <typeparam name="T">The type of token</typeparam>
  [Preserve]
  public struct JEnumerable<T> : IJEnumerable<T>, IEnumerable<T>, IEnumerable
    where T : JToken
  {
    /// <summary>
    /// An empty collection of <see cref="T:Newtonsoft.Json.Linq.JToken" /> objects.
    /// </summary>
    public static readonly JEnumerable<T> Empty = new JEnumerable<T>(Enumerable.Empty<T>());
    private readonly IEnumerable<T> _enumerable;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JEnumerable`1" /> struct.
    /// </summary>
    /// <param name="enumerable">The enumerable.</param>
    public JEnumerable(IEnumerable<T> enumerable)
    {
      ValidationUtils.ArgumentNotNull((object) enumerable, nameof (enumerable));
      this._enumerable = enumerable;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<T> GetEnumerator()
    {
      return this._enumerable == null ? JEnumerable<T>.Empty.GetEnumerator() : this._enumerable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Linq.IJEnumerable`1" /> with the specified key.
    /// </summary>
    /// <value></value>
    public IJEnumerable<JToken> this[object key]
    {
      get
      {
        return this._enumerable == null ? (IJEnumerable<JToken>) JEnumerable<JToken>.Empty : (IJEnumerable<JToken>) new JEnumerable<JToken>(this._enumerable.Values<T, JToken>(key));
      }
    }

    /// <summary>
    /// Determines whether the specified <see cref="T:Newtonsoft.Json.Linq.JEnumerable`1" /> is equal to this instance.
    /// </summary>
    /// <param name="other">The <see cref="T:Newtonsoft.Json.Linq.JEnumerable`1" /> to compare with this instance.</param>
    /// <returns>
    /// 	<c>true</c> if the specified <see cref="T:Newtonsoft.Json.Linq.JEnumerable`1" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(JEnumerable<T> other)
    {
      return object.Equals((object) this._enumerable, (object) other._enumerable);
    }

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
    /// <returns>
    /// 	<c>true</c> if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      return obj is JEnumerable<T> other && this.Equals(other);
    }

    /// <summary>Returns a hash code for this instance.</summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode()
    {
      return this._enumerable == null ? 0 : this._enumerable.GetHashCode();
    }
  }
}
