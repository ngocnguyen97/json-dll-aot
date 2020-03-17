// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.TypeNameHandling
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies type name handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  /// <remarks>
  /// <see cref="T:Newtonsoft.Json.TypeNameHandling" /> should be used with caution when your application deserializes JSON from an external source.
  /// Incoming types should be validated with a custom <see cref="T:System.Runtime.Serialization.SerializationBinder" />
  /// when deserializing with a value other than <c>TypeNameHandling.None</c>.
  /// </remarks>
  [Flags]
  [Preserve]
  public enum TypeNameHandling
  {
    /// <summary>
    /// Do not include the .NET type name when serializing types.
    /// </summary>
    None = 0,
    /// <summary>
    /// Include the .NET type name when serializing into a JSON object structure.
    /// </summary>
    Objects = 1,
    /// <summary>
    /// Include the .NET type name when serializing into a JSON array structure.
    /// </summary>
    Arrays = 2,
    /// <summary>Always include the .NET type name when serializing.</summary>
    All = Arrays | Objects, // 0x00000003
    /// <summary>
    /// Include the .NET type name when the type of the object being serialized is not the same as its declared type.
    /// </summary>
    Auto = 4,
  }
}
