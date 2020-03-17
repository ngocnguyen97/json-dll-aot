// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.NullValueHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies null value handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  /// <example>
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="ReducingSerializedJsonSizeNullValueHandlingObject" title="NullValueHandling Class" />
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="ReducingSerializedJsonSizeNullValueHandlingExample" title="NullValueHandling Ignore Example" />
  /// </example>
  [Preserve]
  public enum NullValueHandling
  {
    /// <summary>
    /// Include null values when serializing and deserializing objects.
    /// </summary>
    Include,
    /// <summary>
    /// Ignore null values when serializing and deserializing objects.
    /// </summary>
    Ignore,
  }
}
