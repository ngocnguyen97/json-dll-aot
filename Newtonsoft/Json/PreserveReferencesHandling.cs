// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.PreserveReferencesHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies reference handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// Note that references cannot be preserved when a value is set via a non-default constructor such as types that implement ISerializable.
  /// </summary>
  /// <example>
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="PreservingObjectReferencesOn" title="Preserve Object References" />
  /// </example>
  [Flags]
  [Preserve]
  public enum PreserveReferencesHandling
  {
    /// <summary>Do not preserve references when serializing types.</summary>
    None = 0,
    /// <summary>
    /// Preserve references when serializing into a JSON object structure.
    /// </summary>
    Objects = 1,
    /// <summary>
    /// Preserve references when serializing into a JSON array structure.
    /// </summary>
    Arrays = 2,
    /// <summary>Preserve references when serializing.</summary>
    All = Arrays | Objects, // 0x00000003
  }
}
