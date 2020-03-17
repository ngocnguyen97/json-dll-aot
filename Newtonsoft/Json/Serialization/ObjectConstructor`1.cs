// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ObjectConstructor`1
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>Represents a method that constructs an object.</summary>
  /// <typeparam name="T">The object type to create.</typeparam>
  [Preserve]
  public delegate object ObjectConstructor<T>(params object[] args);
}
