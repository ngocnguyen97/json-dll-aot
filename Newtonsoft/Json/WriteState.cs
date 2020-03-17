// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.WriteState
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies the state of the <see cref="T:Newtonsoft.Json.JsonWriter" />.
  /// </summary>
  [Preserve]
  public enum WriteState
  {
    /// <summary>
    /// An exception has been thrown, which has left the <see cref="T:Newtonsoft.Json.JsonWriter" /> in an invalid state.
    /// You may call the <see cref="M:Newtonsoft.Json.JsonWriter.Close" /> method to put the <see cref="T:Newtonsoft.Json.JsonWriter" /> in the <c>Closed</c> state.
    /// Any other <see cref="T:Newtonsoft.Json.JsonWriter" /> method calls results in an <see cref="T:System.InvalidOperationException" /> being thrown.
    /// </summary>
    Error,
    /// <summary>
    /// The <see cref="M:Newtonsoft.Json.JsonWriter.Close" /> method has been called.
    /// </summary>
    Closed,
    /// <summary>An object is being written.</summary>
    Object,
    /// <summary>A array is being written.</summary>
    Array,
    /// <summary>A constructor is being written.</summary>
    Constructor,
    /// <summary>A property is being written.</summary>
    Property,
    /// <summary>A write method has not been called.</summary>
    Start,
  }
}
