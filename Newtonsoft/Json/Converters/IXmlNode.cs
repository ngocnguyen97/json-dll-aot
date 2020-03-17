// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IXmlNode
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal interface IXmlNode
  {
    XmlNodeType NodeType { get; }

    string LocalName { get; }

    List<IXmlNode> ChildNodes { get; }

    List<IXmlNode> Attributes { get; }

    IXmlNode ParentNode { get; }

    string Value { get; set; }

    IXmlNode AppendChild(IXmlNode newChild);

    string NamespaceUri { get; }

    object WrappedNode { get; }
  }
}
