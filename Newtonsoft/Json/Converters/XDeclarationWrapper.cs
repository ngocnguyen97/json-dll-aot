// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
  {
    internal XDeclaration Declaration { get; private set; }

    public XDeclarationWrapper(XDeclaration declaration)
      : base((XObject) null)
    {
      this.Declaration = declaration;
    }

    public override XmlNodeType NodeType
    {
      get
      {
        return XmlNodeType.XmlDeclaration;
      }
    }

    public string Version
    {
      get
      {
        return this.Declaration.Version;
      }
    }

    public string Encoding
    {
      get
      {
        return this.Declaration.Encoding;
      }
      set
      {
        this.Declaration.Encoding = value;
      }
    }

    public string Standalone
    {
      get
      {
        return this.Declaration.Standalone;
      }
      set
      {
        this.Declaration.Standalone = value;
      }
    }
  }
}
