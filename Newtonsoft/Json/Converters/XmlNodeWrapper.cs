// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlNodeWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlNodeWrapper : IXmlNode
  {
    private readonly XmlNode _node;
    private List<IXmlNode> _childNodes;
    private List<IXmlNode> _attributes;

    public XmlNodeWrapper(XmlNode node)
    {
      this._node = node;
    }

    public object WrappedNode
    {
      get
      {
        return (object) this._node;
      }
    }

    public XmlNodeType NodeType
    {
      get
      {
        return this._node.NodeType;
      }
    }

    public virtual string LocalName
    {
      get
      {
        return this._node.LocalName;
      }
    }

    public List<IXmlNode> ChildNodes
    {
      get
      {
        if (this._childNodes == null)
        {
          this._childNodes = new List<IXmlNode>(this._node.ChildNodes.Count);
          foreach (XmlNode childNode in this._node.ChildNodes)
            this._childNodes.Add(XmlNodeWrapper.WrapNode(childNode));
        }
        return this._childNodes;
      }
    }

    internal static IXmlNode WrapNode(XmlNode node)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          return (IXmlNode) new XmlElementWrapper((XmlElement) node);
        case XmlNodeType.DocumentType:
          return (IXmlNode) new XmlDocumentTypeWrapper((XmlDocumentType) node);
        case XmlNodeType.XmlDeclaration:
          return (IXmlNode) new XmlDeclarationWrapper((XmlDeclaration) node);
        default:
          return (IXmlNode) new XmlNodeWrapper(node);
      }
    }

    public List<IXmlNode> Attributes
    {
      get
      {
        if (this._node.Attributes == null)
          return (List<IXmlNode>) null;
        if (this._attributes == null)
        {
          this._attributes = new List<IXmlNode>(this._node.Attributes.Count);
          foreach (XmlNode attribute in (XmlNamedNodeMap) this._node.Attributes)
            this._attributes.Add(XmlNodeWrapper.WrapNode(attribute));
        }
        return this._attributes;
      }
    }

    public IXmlNode ParentNode
    {
      get
      {
        XmlNode node = this._node is XmlAttribute ? (XmlNode) ((XmlAttribute) this._node).OwnerElement : this._node.ParentNode;
        return node == null ? (IXmlNode) null : XmlNodeWrapper.WrapNode(node);
      }
    }

    public string Value
    {
      get
      {
        return this._node.Value;
      }
      set
      {
        this._node.Value = value;
      }
    }

    public IXmlNode AppendChild(IXmlNode newChild)
    {
      this._node.AppendChild(((XmlNodeWrapper) newChild)._node);
      this._childNodes = (List<IXmlNode>) null;
      this._attributes = (List<IXmlNode>) null;
      return newChild;
    }

    public string NamespaceUri
    {
      get
      {
        return this._node.NamespaceURI;
      }
    }
  }
}
