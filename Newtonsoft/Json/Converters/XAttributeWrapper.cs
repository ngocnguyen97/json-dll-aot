// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XAttributeWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XAttributeWrapper : XObjectWrapper
  {
    private XAttribute Attribute
    {
      get
      {
        return (XAttribute) this.WrappedNode;
      }
    }

    public XAttributeWrapper(XAttribute attribute)
      : base((XObject) attribute)
    {
    }

    public override string Value
    {
      get
      {
        return this.Attribute.Value;
      }
      set
      {
        this.Attribute.Value = value;
      }
    }

    public override string LocalName
    {
      get
      {
        return this.Attribute.Name.LocalName;
      }
    }

    public override string NamespaceUri
    {
      get
      {
        return this.Attribute.Name.NamespaceName;
      }
    }

    public override IXmlNode ParentNode
    {
      get
      {
        return this.Attribute.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Attribute.Parent);
      }
    }
  }
}
