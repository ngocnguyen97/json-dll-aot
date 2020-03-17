// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.DefaultValueHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies default value handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  /// <example>
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="ReducingSerializedJsonSizeDefaultValueHandlingObject" title="DefaultValueHandling Class" />
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="ReducingSerializedJsonSizeDefaultValueHandlingExample" title="DefaultValueHandling Ignore Example" />
  /// </example>
  [Flags]
  [Preserve]
  public enum DefaultValueHandling
  {
    /// <summary>
    /// Include members where the member value is the same as the member's default value when serializing objects.
    /// Included members are written to JSON. Has no effect when deserializing.
    /// </summary>
    Include = 0,
    /// <summary>
    /// Ignore members where the member value is the same as the member's default value when serializing objects
    /// so that is is not written to JSON.
    /// This option will ignore all default values (e.g. <c>null</c> for objects and nullable types; <c>0</c> for integers,
    /// decimals and floating point numbers; and <c>false</c> for booleans). The default value ignored can be changed by
    /// placing the <see cref="T:System.ComponentModel.DefaultValueAttribute" /> on the property.
    /// </summary>
    Ignore = 1,
    /// <summary>
    /// Members with a default value but no JSON will be set to their default value when deserializing.
    /// </summary>
    Populate = 2,
    /// <summary>
    /// Ignore members where the member value is the same as the member's default value when serializing objects
    /// and sets members to their default value when deserializing.
    /// </summary>
    IgnoreAndPopulate = Populate | Ignore, // 0x00000003
  }
}
