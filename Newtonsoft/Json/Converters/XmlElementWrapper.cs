// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlElementWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
  {
    private readonly XmlElement _element;

    public XmlElementWrapper(XmlElement element)
      : base((XmlNode) element)
    {
      this._element = element;
    }

    public void SetAttributeNode(IXmlNode attribute)
    {
      this._element.SetAttributeNode((XmlAttribute) ((XmlNodeWrapper) attribute).WrappedNode);
    }

    public string GetPrefixOfNamespace(string namespaceUri)
    {
      return this._element.GetPrefixOfNamespace(namespaceUri);
    }

    public bool IsEmpty
    {
      get
      {
        return this._element.IsEmpty;
      }
    }
  }
}
