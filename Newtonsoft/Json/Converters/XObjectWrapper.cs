// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XObjectWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XObjectWrapper : IXmlNode
  {
    private static readonly List<IXmlNode> EmptyChildNodes = new List<IXmlNode>();
    private readonly XObject _xmlObject;

    public XObjectWrapper(XObject xmlObject)
    {
      this._xmlObject = xmlObject;
    }

    public object WrappedNode
    {
      get
      {
        return (object) this._xmlObject;
      }
    }

    public virtual XmlNodeType NodeType
    {
      get
      {
        return this._xmlObject.NodeType;
      }
    }

    public virtual string LocalName
    {
      get
      {
        return (string) null;
      }
    }

    public virtual List<IXmlNode> ChildNodes
    {
      get
      {
        return XObjectWrapper.EmptyChildNodes;
      }
    }

    public virtual List<IXmlNode> Attributes
    {
      get
      {
        return (List<IXmlNode>) null;
      }
    }

    public virtual IXmlNode ParentNode
    {
      get
      {
        return (IXmlNode) null;
      }
    }

    public virtual string Value
    {
      get
      {
        return (string) null;
      }
      set
      {
        throw new InvalidOperationException();
      }
    }

    public virtual IXmlNode AppendChild(IXmlNode newChild)
    {
      throw new InvalidOperationException();
    }

    public virtual string NamespaceUri
    {
      get
      {
        return (string) null;
      }
    }
  }
}
