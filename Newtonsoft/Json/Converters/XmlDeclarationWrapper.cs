// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
  {
    private readonly XmlDeclaration _declaration;

    public XmlDeclarationWrapper(XmlDeclaration declaration)
      : base((XmlNode) declaration)
    {
      this._declaration = declaration;
    }

    public string Version
    {
      get
      {
        return this._declaration.Version;
      }
    }

    public string Encoding
    {
      get
      {
        return this._declaration.Encoding;
      }
      set
      {
        this._declaration.Encoding = value;
      }
    }

    public string Standalone
    {
      get
      {
        return this._declaration.Standalone;
      }
      set
      {
        this._declaration.Standalone = value;
      }
    }
  }
}
