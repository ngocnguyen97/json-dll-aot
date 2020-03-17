// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XElementWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
  {
    private List<IXmlNode> _attributes;

    private XElement Element
    {
      get
      {
        return (XElement) this.WrappedNode;
      }
    }

    public XElementWrapper(XElement element)
      : base((XContainer) element)
    {
    }

    public void SetAttributeNode(IXmlNode attribute)
    {
      this.Element.Add(((XObjectWrapper) attribute).WrappedNode);
      this._attributes = (List<IXmlNode>) null;
    }

    public override List<IXmlNode> Attributes
    {
      get
      {
        if (this._attributes == null)
        {
          this._attributes = new List<IXmlNode>();
          foreach (XAttribute attribute in this.Element.Attributes())
            this._attributes.Add((IXmlNode) new XAttributeWrapper(attribute));
          string namespaceUri = this.NamespaceUri;
          if (!string.IsNullOrEmpty(namespaceUri) && (namespaceUri != this.ParentNode?.NamespaceUri && string.IsNullOrEmpty(this.GetPrefixOfNamespace(namespaceUri))))
          {
            bool flag = false;
            foreach (IXmlNode attribute in this._attributes)
            {
              if (attribute.LocalName == "xmlns" && string.IsNullOrEmpty(attribute.NamespaceUri) && attribute.Value == namespaceUri)
                flag = true;
            }
            if (!flag)
              this._attributes.Insert(0, (IXmlNode) new XAttributeWrapper(new XAttribute((XName) "xmlns", (object) namespaceUri)));
          }
        }
        return this._attributes;
      }
    }

    public override IXmlNode AppendChild(IXmlNode newChild)
    {
      IXmlNode xmlNode = base.AppendChild(newChild);
      this._attributes = (List<IXmlNode>) null;
      return xmlNode;
    }

    public override string Value
    {
      get
      {
        return this.Element.Value;
      }
      set
      {
        this.Element.Value = value;
      }
    }

    public override string LocalName
    {
      get
      {
        return this.Element.Name.LocalName;
      }
    }

    public override string NamespaceUri
    {
      get
      {
        return this.Element.Name.NamespaceName;
      }
    }

    public string GetPrefixOfNamespace(string namespaceUri)
    {
      return this.Element.GetPrefixOfNamespace((XNamespace) namespaceUri);
    }

    public bool IsEmpty
    {
      get
      {
        return this.Element.IsEmpty;
      }
    }
  }
}
