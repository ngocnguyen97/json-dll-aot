// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ErrorEventArgs
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>Provides data for the Error event.</summary>
  [Preserve]
  public class ErrorEventArgs : EventArgs
  {
    /// <summary>
    /// Gets the current object the error event is being raised against.
    /// </summary>
    /// <value>The current object the error event is being raised against.</value>
    public object CurrentObject { get; private set; }

    /// <summary>Gets the error context.</summary>
    /// <value>The error context.</value>
    public ErrorContext ErrorContext { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.ErrorEventArgs" /> class.
    /// </summary>
    /// <param name="currentObject">The current object.</param>
    /// <param name="errorContext">The error context.</param>
    public ErrorEventArgs(object currentObject, ErrorContext errorContext)
    {
      this.CurrentObject = currentObject;
      this.ErrorContext = errorContext;
    }
  }
}
