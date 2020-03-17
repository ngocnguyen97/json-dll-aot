// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XCommentWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XCommentWrapper : XObjectWrapper
  {
    private XComment Text
    {
      get
      {
        return (XComment) this.WrappedNode;
      }
    }

    public XCommentWrapper(XComment text)
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
