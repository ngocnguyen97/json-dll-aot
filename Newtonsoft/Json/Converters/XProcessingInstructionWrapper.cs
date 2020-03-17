// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XProcessingInstructionWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XProcessingInstructionWrapper : XObjectWrapper
  {
    private XProcessingInstruction ProcessingInstruction
    {
      get
      {
        return (XProcessingInstruction) this.WrappedNode;
      }
    }

    public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction)
      : base((XObject) processingInstruction)
    {
    }

    public override string LocalName
    {
      get
      {
        return this.ProcessingInstruction.Target;
      }
    }

    public override string Value
    {
      get
      {
        return this.ProcessingInstruction.Data;
      }
      set
      {
        this.ProcessingInstruction.Data = value;
      }
    }
  }
}
