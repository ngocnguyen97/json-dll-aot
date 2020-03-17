// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.IContractResolver
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Used by <see cref="T:Newtonsoft.Json.JsonSerializer" /> to resolves a <see cref="T:Newtonsoft.Json.Serialization.JsonContract" /> for a given <see cref="T:System.Type" />.
  /// </summary>
  /// <example>
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="ReducingSerializedJsonSizeContractResolverObject" title="IContractResolver Class" />
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="ReducingSerializedJsonSizeContractResolverExample" title="IContractResolver Example" />
  /// </example>
  [Preserve]
  public interface IContractResolver
  {
    /// <summary>Resolves the contract for a given type.</summary>
    /// <param name="type">The type to resolve a contract for.</param>
    /// <returns>The contract for a given type.</returns>
    JsonContract ResolveContract(Type type);
  }
}
