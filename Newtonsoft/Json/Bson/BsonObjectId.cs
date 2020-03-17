// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonObjectId
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Bson
{
  /// <summary>Represents a BSON Oid (object id).</summary>
  [Preserve]
  public class BsonObjectId
  {
    /// <summary>Gets or sets the value of the Oid.</summary>
    /// <value>The value of the Oid.</value>
    public byte[] Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Bson.BsonObjectId" /> class.
    /// </summary>
    /// <param name="value">The Oid value.</param>
    public BsonObjectId(byte[] value)
    {
      ValidationUtils.ArgumentNotNull((object) value, nameof (value));
      if (value.Length != 12)
        throw new ArgumentException("An ObjectId must be 12 bytes", nameof (value));
      this.Value = value;
    }
  }
}
