// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XDocumentTypeWrapper
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
  {
    private readonly XDocumentType _documentType;

    public XDocumentTypeWrapper(XDocumentType documentType)
      : base((XObject) documentType)
    {
      this._documentType = documentType;
    }

    public string Name
    {
      get
      {
        return this._documentType.Name;
      }
    }

    public string System
    {
      get
      {
        return this._documentType.SystemId;
      }
    }

    public string Public
    {
      get
      {
        return this._documentType.PublicId;
      }
    }

    public string InternalSubset
    {
      get
      {
        return this._documentType.InternalSubset;
      }
    }

    public override string LocalName
    {
      get
      {
        return "DOCTYPE";
      }
    }
  }
}
