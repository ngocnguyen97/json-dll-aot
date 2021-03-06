﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.SerializationErrorCallback
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Handles <see cref="T:Newtonsoft.Json.JsonSerializer" /> serialization error callback events.
  /// </summary>
  /// <param name="o">The object that raised the callback event.</param>
  /// <param name="context">The streaming context.</param>
  /// <param name="errorContext">The error context.</param>
  [Preserve]
  public delegate void SerializationErrorCallback(
    object o,
    StreamingContext context,
    ErrorContext errorContext);
}
