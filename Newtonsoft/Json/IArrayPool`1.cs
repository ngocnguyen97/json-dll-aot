// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.IArrayPool`1
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>Provides an interface for using pooled arrays.</summary>
  /// <typeparam name="T">The array type content.</typeparam>
  [Preserve]
  public interface IArrayPool<T>
  {
    /// <summary>
    /// Rent a array from the pool. This array must be returned when it is no longer needed.
    /// </summary>
    /// <param name="minimumLength">The minimum required length of the array. The returned array may be longer.</param>
    /// <returns>The rented array from the pool. This array must be returned when it is no longer needed.</returns>
    T[] Rent(int minimumLength);

    /// <summary>Return an array to the pool.</summary>
    /// <param name="array">The array that is being returned.</param>
    void Return(T[] array);
  }
}
