﻿// Decompiled with JetBrains decompiler
// Type: System.ComponentModel.PropertyChangingEventArgs
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;

namespace System.ComponentModel
{
  [Preserve]
  public class PropertyChangingEventArgs : EventArgs
  {
    public PropertyChangingEventArgs(string propertyName)
    {
      this.PropertyName = propertyName;
    }

    public virtual string PropertyName { get; set; }
  }
}
