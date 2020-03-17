// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.BufferUtils
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal static class BufferUtils
  {
    public static char[] RentBuffer(IArrayPool<char> bufferPool, int minSize)
    {
      return bufferPool == null ? new char[minSize] : bufferPool.Rent(minSize);
    }

    public static void ReturnBuffer(IArrayPool<char> bufferPool, char[] buffer)
    {
      bufferPool?.Return(buffer);
    }

    public static char[] EnsureBufferSize(IArrayPool<char> bufferPool, int size, char[] buffer)
    {
      if (bufferPool == null)
        return new char[size];
      if (buffer != null)
        bufferPool.Return(buffer);
      return bufferPool.Rent(size);
    }
  }
}
