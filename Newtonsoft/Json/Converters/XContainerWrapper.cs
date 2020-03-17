// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XContainerWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XContainerWrapper : XObjectWrapper
  {
    private List<IXmlNode> _childNodes;

    private XContainer Container
    {
      get
      {
        return (XContainer) this.WrappedNode;
      }
    }

    public XContainerWrapper(XContainer container)
      : base((XObject) container)
    {
    }

    public override List<IXmlNode> ChildNodes
    {
      get
      {
        if (this._childNodes == null)
        {
          this._childNodes = new List<IXmlNode>();
          foreach (XObject node in this.Container.Nodes())
            this._childNodes.Add(XContainerWrapper.WrapNode(node));
        }
        return this._childNodes;
      }
    }

    public override IXmlNode ParentNode
    {
      get
      {
        return this.Container.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Container.Parent);
      }
    }

    internal static IXmlNode WrapNode(XObject node)
    {
      switch (node)
      {
        case XDocument _:
          return (IXmlNode) new XDocumentWrapper((XDocument) node);
        case XElement _:
          return (IXmlNode) new XElementWrapper((XElement) node);
        case XContainer _:
          return (IXmlNode) new XContainerWrapper((XContainer) node);
        case XProcessingInstruction _:
          return (IXmlNode) new XProcessingInstructionWrapper((XProcessingInstruction) node);
        case XText _:
          return (IXmlNode) new XTextWrapper((XText) node);
        case XComment _:
          return (IXmlNode) new XCommentWrapper((XComment) node);
        case XAttribute _:
          return (IXmlNode) new XAttributeWrapper((XAttribute) node);
        case XDocumentType _:
          return (IXmlNode) new XDocumentTypeWrapper((XDocumentType) node);
        default:
          return (IXmlNode) new XObjectWrapper(node);
      }
    }

    public override IXmlNode AppendChild(IXmlNode newChild)
    {
      this.Container.Add(newChild.WrappedNode);
      this._childNodes = (List<IXmlNode>) null;
      return newChild;
    }
  }
}
