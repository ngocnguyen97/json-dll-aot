// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XTextWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XTextWrapper : XObjectWrapper
  {
    private XText Text
    {
      get
      {
        return (XText) this.WrappedNode;
      }
    }

    public XTextWrapper(XText text)
      : base((XObject) text)
    {
    }

    public override string Value
    {
      get
      {
        return this.Text.Value;
      }
      set
      {
        this.Text.Value = value;
      }
    }

    public override IXmlNode ParentNode
    {
      get
      {
        return this.Text.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Text.Parent);
      }
    }
  }
}
