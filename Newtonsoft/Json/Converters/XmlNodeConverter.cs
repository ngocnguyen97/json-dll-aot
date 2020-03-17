// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlNodeConverter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  /// <summary>Converts XML to and from JSON.</summary>
  [Preserve]
  public class XmlNodeConverter : JsonConverter
  {
    private const string TextName = "#text";
    private const string CommentName = "#comment";
    private const string CDataName = "#cdata-section";
    private const string WhitespaceName = "#whitespace";
    private const string SignificantWhitespaceName = "#significant-whitespace";
    private const string DeclarationName = "?xml";
    private const string JsonNamespaceUri = "http://james.newtonking.com/projects/json";

    /// <summary>
    /// Gets or sets the name of the root element to insert when deserializing to XML if the JSON structure has produces multiple root elements.
    /// </summary>
    /// <value>The name of the deserialize root element.</value>
    public string DeserializeRootElementName { get; set; }

    /// <summary>
    /// Gets or sets a flag to indicate whether to write the Json.NET array attribute.
    /// This attribute helps preserve arrays when converting the written XML back to JSON.
    /// </summary>
    /// <value><c>true</c> if the array attibute is written to the XML; otherwise, <c>false</c>.</value>
    public bool WriteArrayAttribute { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to write the root JSON object.
    /// </summary>
    /// <value><c>true</c> if the JSON root object is omitted; otherwise, <c>false</c>.</value>
    public bool OmitRootObject { get; set; }

    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <param name="value">The value.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      IXmlNode node = this.WrapXml(value);
      XmlNamespaceManager manager = new XmlNamespaceManager((XmlNameTable) new NameTable());
      this.PushParentNamespaces(node, manager);
      if (!this.OmitRootObject)
        writer.WriteStartObject();
      this.SerializeNode(writer, node, manager, !this.OmitRootObject);
      if (this.OmitRootObject)
        return;
      writer.WriteEndObject();
    }

    private IXmlNode WrapXml(object value)
    {
      switch (value)
      {
        case XObject _:
          return XContainerWrapper.WrapNode((XObject) value);
        case XmlNode _:
          return XmlNodeWrapper.WrapNode((XmlNode) value);
        default:
          throw new ArgumentException("Value must be an XML object.", nameof (value));
      }
    }

    private void PushParentNamespaces(IXmlNode node, XmlNamespaceManager manager)
    {
      List<IXmlNode> xmlNodeList = (List<IXmlNode>) null;
      IXmlNode xmlNode1 = node;
      while ((xmlNode1 = xmlNode1.ParentNode) != null)
      {
        if (xmlNode1.NodeType == XmlNodeType.Element)
        {
          if (xmlNodeList == null)
            xmlNodeList = new List<IXmlNode>();
          xmlNodeList.Add(xmlNode1);
        }
      }
      if (xmlNodeList == null)
        return;
      xmlNodeList.Reverse();
      foreach (IXmlNode xmlNode2 in xmlNodeList)
      {
        manager.PushScope();
        foreach (IXmlNode attribute in xmlNode2.Attributes)
        {
          if (attribute.NamespaceUri == "http://www.w3.org/2000/xmlns/" && attribute.LocalName != "xmlns")
            manager.AddNamespace(attribute.LocalName, attribute.Value);
        }
      }
    }

    private string ResolveFullName(IXmlNode node, XmlNamespaceManager manager)
    {
      string str = node.NamespaceUri == null || node.LocalName == "xmlns" && node.NamespaceUri == "http://www.w3.org/2000/xmlns/" ? (string) null : manager.LookupPrefix(node.NamespaceUri);
      return !string.IsNullOrEmpty(str) ? str + ":" + XmlConvert.DecodeName(node.LocalName) : XmlConvert.DecodeName(node.LocalName);
    }

    private string GetPropertyName(IXmlNode node, XmlNamespaceManager manager)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          return node.NamespaceUri == "http://james.newtonking.com/projects/json" ? "$" + node.LocalName : this.ResolveFullName(node, manager);
        case XmlNodeType.Attribute:
          return node.NamespaceUri == "http://james.newtonking.com/projects/json" ? "$" + node.LocalName : "@" + this.ResolveFullName(node, manager);
        case XmlNodeType.Text:
          return "#text";
        case XmlNodeType.CDATA:
          return "#cdata-section";
        case XmlNodeType.ProcessingInstruction:
          return "?" + this.ResolveFullName(node, manager);
        case XmlNodeType.Comment:
          return "#comment";
        case XmlNodeType.DocumentType:
          return "!" + this.ResolveFullName(node, manager);
        case XmlNodeType.Whitespace:
          return "#whitespace";
        case XmlNodeType.SignificantWhitespace:
          return "#significant-whitespace";
        case XmlNodeType.XmlDeclaration:
          return "?xml";
        default:
          throw new JsonSerializationException("Unexpected XmlNodeType when getting node name: " + (object) node.NodeType);
      }
    }

    private bool IsArray(IXmlNode node)
    {
      if (node.Attributes != null)
      {
        foreach (IXmlNode attribute in node.Attributes)
        {
          if (attribute.LocalName == "Array" && attribute.NamespaceUri == "http://james.newtonking.com/projects/json")
            return XmlConvert.ToBoolean(attribute.Value);
        }
      }
      return false;
    }

    private void SerializeGroupedNodes(
      JsonWriter writer,
      IXmlNode node,
      XmlNamespaceManager manager,
      bool writePropertyName)
    {
      Dictionary<string, List<IXmlNode>> dictionary = new Dictionary<string, List<IXmlNode>>();
      for (int index = 0; index < node.ChildNodes.Count; ++index)
      {
        IXmlNode childNode = node.ChildNodes[index];
        string propertyName = this.GetPropertyName(childNode, manager);
        List<IXmlNode> xmlNodeList;
        if (!dictionary.TryGetValue(propertyName, out xmlNodeList))
        {
          xmlNodeList = new List<IXmlNode>();
          dictionary.Add(propertyName, xmlNodeList);
        }
        xmlNodeList.Add(childNode);
      }
      foreach (KeyValuePair<string, List<IXmlNode>> keyValuePair in dictionary)
      {
        List<IXmlNode> xmlNodeList = keyValuePair.Value;
        if (xmlNodeList.Count == 1 && !this.IsArray(xmlNodeList[0]))
        {
          this.SerializeNode(writer, xmlNodeList[0], manager, writePropertyName);
        }
        else
        {
          string key = keyValuePair.Key;
          if (writePropertyName)
            writer.WritePropertyName(key);
          writer.WriteStartArray();
          for (int index = 0; index < xmlNodeList.Count; ++index)
            this.SerializeNode(writer, xmlNodeList[index], manager, false);
          writer.WriteEndArray();
        }
      }
    }

    private void SerializeNode(
      JsonWriter writer,
      IXmlNode node,
      XmlNamespaceManager manager,
      bool writePropertyName)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          if (this.IsArray(node) && XmlNodeConverter.AllSameName(node) && node.ChildNodes.Count > 0)
          {
            this.SerializeGroupedNodes(writer, node, manager, false);
            break;
          }
          manager.PushScope();
          foreach (IXmlNode attribute in node.Attributes)
          {
            if (attribute.NamespaceUri == "http://www.w3.org/2000/xmlns/")
            {
              string prefix = attribute.LocalName != "xmlns" ? XmlConvert.DecodeName(attribute.LocalName) : string.Empty;
              string uri = attribute.Value;
              manager.AddNamespace(prefix, uri);
            }
          }
          if (writePropertyName)
            writer.WritePropertyName(this.GetPropertyName(node, manager));
          if (!this.ValueAttributes(node.Attributes) && node.ChildNodes.Count == 1 && node.ChildNodes[0].NodeType == XmlNodeType.Text)
            writer.WriteValue(node.ChildNodes[0].Value);
          else if (node.ChildNodes.Count == 0 && CollectionUtils.IsNullOrEmpty<IXmlNode>((ICollection<IXmlNode>) node.Attributes))
          {
            if (((IXmlElement) node).IsEmpty)
              writer.WriteNull();
            else
              writer.WriteValue(string.Empty);
          }
          else
          {
            writer.WriteStartObject();
            for (int index = 0; index < node.Attributes.Count; ++index)
              this.SerializeNode(writer, node.Attributes[index], manager, true);
            this.SerializeGroupedNodes(writer, node, manager, true);
            writer.WriteEndObject();
          }
          manager.PopScope();
          break;
        case XmlNodeType.Attribute:
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
        case XmlNodeType.ProcessingInstruction:
        case XmlNodeType.Whitespace:
        case XmlNodeType.SignificantWhitespace:
          if (node.NamespaceUri == "http://www.w3.org/2000/xmlns/" && node.Value == "http://james.newtonking.com/projects/json" || node.NamespaceUri == "http://james.newtonking.com/projects/json" && node.LocalName == "Array")
            break;
          if (writePropertyName)
            writer.WritePropertyName(this.GetPropertyName(node, manager));
          writer.WriteValue(node.Value);
          break;
        case XmlNodeType.Comment:
          if (!writePropertyName)
            break;
          writer.WriteComment(node.Value);
          break;
        case XmlNodeType.Document:
        case XmlNodeType.DocumentFragment:
          this.SerializeGroupedNodes(writer, node, manager, writePropertyName);
          break;
        case XmlNodeType.DocumentType:
          IXmlDocumentType xmlDocumentType = (IXmlDocumentType) node;
          writer.WritePropertyName(this.GetPropertyName(node, manager));
          writer.WriteStartObject();
          if (!string.IsNullOrEmpty(xmlDocumentType.Name))
          {
            writer.WritePropertyName("@name");
            writer.WriteValue(xmlDocumentType.Name);
          }
          if (!string.IsNullOrEmpty(xmlDocumentType.Public))
          {
            writer.WritePropertyName("@public");
            writer.WriteValue(xmlDocumentType.Public);
          }
          if (!string.IsNullOrEmpty(xmlDocumentType.System))
          {
            writer.WritePropertyName("@system");
            writer.WriteValue(xmlDocumentType.System);
          }
          if (!string.IsNullOrEmpty(xmlDocumentType.InternalSubset))
          {
            writer.WritePropertyName("@internalSubset");
            writer.WriteValue(xmlDocumentType.InternalSubset);
          }
          writer.WriteEndObject();
          break;
        case XmlNodeType.XmlDeclaration:
          IXmlDeclaration xmlDeclaration = (IXmlDeclaration) node;
          writer.WritePropertyName(this.GetPropertyName(node, manager));
          writer.WriteStartObject();
          if (!string.IsNullOrEmpty(xmlDeclaration.Version))
          {
            writer.WritePropertyName("@version");
            writer.WriteValue(xmlDeclaration.Version);
          }
          if (!string.IsNullOrEmpty(xmlDeclaration.Encoding))
          {
            writer.WritePropertyName("@encoding");
            writer.WriteValue(xmlDeclaration.Encoding);
          }
          if (!string.IsNullOrEmpty(xmlDeclaration.Standalone))
          {
            writer.WritePropertyName("@standalone");
            writer.WriteValue(xmlDeclaration.Standalone);
          }
          writer.WriteEndObject();
          break;
        default:
          throw new JsonSerializationException("Unexpected XmlNodeType when serializing nodes: " + (object) node.NodeType);
      }
    }

    private static bool AllSameName(IXmlNode node)
    {
      foreach (IXmlNode childNode in node.ChildNodes)
      {
        if (childNode.LocalName != node.LocalName)
          return false;
      }
      return true;
    }

    /// <summary>Reads the JSON representation of the object.</summary>
    /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      XmlNamespaceManager manager = new XmlNamespaceManager((XmlNameTable) new NameTable());
      IXmlDocument document = (IXmlDocument) null;
      IXmlNode currentNode = (IXmlNode) null;
      if (typeof (XObject).IsAssignableFrom(objectType))
      {
        if (objectType != typeof (XDocument) && objectType != typeof (XElement))
          throw new JsonSerializationException("XmlNodeConverter only supports deserializing XDocument or XElement.");
        document = (IXmlDocument) new XDocumentWrapper(new XDocument());
        currentNode = (IXmlNode) document;
      }
      if (typeof (XmlNode).IsAssignableFrom(objectType))
      {
        if (objectType != typeof (XmlDocument))
          throw new JsonSerializationException("XmlNodeConverter only supports deserializing XmlDocuments");
        document = (IXmlDocument) new XmlDocumentWrapper(new XmlDocument()
        {
          XmlResolver = (XmlResolver) null
        });
        currentNode = (IXmlNode) document;
      }
      if (document == null || currentNode == null)
        throw new JsonSerializationException("Unexpected type when converting XML: " + (object) objectType);
      if (reader.TokenType != JsonToken.StartObject)
        throw new JsonSerializationException("XmlNodeConverter can only convert JSON that begins with an object.");
      if (!string.IsNullOrEmpty(this.DeserializeRootElementName))
      {
        this.ReadElement(reader, document, currentNode, this.DeserializeRootElementName, manager);
      }
      else
      {
        reader.Read();
        this.DeserializeNode(reader, document, manager, currentNode);
      }
      if (objectType != typeof (XElement))
        return document.WrappedNode;
      XElement wrappedNode = (XElement) document.DocumentElement.WrappedNode;
      wrappedNode.Remove();
      return (object) wrappedNode;
    }

    private void DeserializeValue(
      JsonReader reader,
      IXmlDocument document,
      XmlNamespaceManager manager,
      string propertyName,
      IXmlNode currentNode)
    {
      if (!(propertyName == "#text"))
      {
        if (!(propertyName == "#cdata-section"))
        {
          if (!(propertyName == "#whitespace"))
          {
            if (propertyName == "#significant-whitespace")
              currentNode.AppendChild(document.CreateSignificantWhitespace(reader.Value.ToString()));
            else if (!string.IsNullOrEmpty(propertyName) && propertyName[0] == '?')
              this.CreateInstruction(reader, document, currentNode, propertyName);
            else if (string.Equals(propertyName, "!DOCTYPE", StringComparison.OrdinalIgnoreCase))
              this.CreateDocumentType(reader, document, currentNode);
            else if (reader.TokenType == JsonToken.StartArray)
              this.ReadArrayElements(reader, document, propertyName, currentNode, manager);
            else
              this.ReadElement(reader, document, currentNode, propertyName, manager);
          }
          else
            currentNode.AppendChild(document.CreateWhitespace(reader.Value.ToString()));
        }
        else
          currentNode.AppendChild(document.CreateCDataSection(reader.Value.ToString()));
      }
      else
        currentNode.AppendChild(document.CreateTextNode(reader.Value.ToString()));
    }

    private void ReadElement(
      JsonReader reader,
      IXmlDocument document,
      IXmlNode currentNode,
      string propertyName,
      XmlNamespaceManager manager)
    {
      if (string.IsNullOrEmpty(propertyName))
        throw JsonSerializationException.Create(reader, "XmlNodeConverter cannot convert JSON with an empty property name to XML.");
      Dictionary<string, string> attributeNameValues = this.ReadAttributeElements(reader, manager);
      string prefix1 = MiscellaneousUtils.GetPrefix(propertyName);
      if (propertyName.StartsWith('@'))
      {
        string str = propertyName.Substring(1);
        string prefix2 = MiscellaneousUtils.GetPrefix(str);
        XmlNodeConverter.AddAttribute(reader, document, currentNode, str, manager, prefix2);
      }
      else
      {
        if (propertyName.StartsWith('$'))
        {
          if (!(propertyName == "$values"))
          {
            if (propertyName == "$id" || propertyName == "$ref" || (propertyName == "$type" || propertyName == "$value"))
            {
              string attributeName = propertyName.Substring(1);
              string attributePrefix = manager.LookupPrefix("http://james.newtonking.com/projects/json");
              XmlNodeConverter.AddAttribute(reader, document, currentNode, attributeName, manager, attributePrefix);
              return;
            }
          }
          else
          {
            propertyName = propertyName.Substring(1);
            string elementPrefix = manager.LookupPrefix("http://james.newtonking.com/projects/json");
            this.CreateElement(reader, document, currentNode, propertyName, manager, elementPrefix, attributeNameValues);
            return;
          }
        }
        this.CreateElement(reader, document, currentNode, propertyName, manager, prefix1, attributeNameValues);
      }
    }

    private void CreateElement(
      JsonReader reader,
      IXmlDocument document,
      IXmlNode currentNode,
      string elementName,
      XmlNamespaceManager manager,
      string elementPrefix,
      Dictionary<string, string> attributeNameValues)
    {
      IXmlElement element = this.CreateElement(elementName, document, elementPrefix, manager);
      currentNode.AppendChild((IXmlNode) element);
      foreach (KeyValuePair<string, string> attributeNameValue in attributeNameValues)
      {
        string str = XmlConvert.EncodeName(attributeNameValue.Key);
        string prefix = MiscellaneousUtils.GetPrefix(attributeNameValue.Key);
        IXmlNode attribute = !string.IsNullOrEmpty(prefix) ? document.CreateAttribute(str, manager.LookupNamespace(prefix) ?? string.Empty, attributeNameValue.Value) : document.CreateAttribute(str, attributeNameValue.Value);
        element.SetAttributeNode(attribute);
      }
      if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Integer || (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Boolean) || reader.TokenType == JsonToken.Date)
      {
        string xmlValue = this.ConvertTokenToXmlValue(reader);
        if (xmlValue == null)
          return;
        element.AppendChild(document.CreateTextNode(xmlValue));
      }
      else
      {
        if (reader.TokenType == JsonToken.Null)
          return;
        if (reader.TokenType != JsonToken.EndObject)
        {
          manager.PushScope();
          this.DeserializeNode(reader, document, manager, (IXmlNode) element);
          manager.PopScope();
        }
        manager.RemoveNamespace(string.Empty, manager.DefaultNamespace);
      }
    }

    private static void AddAttribute(
      JsonReader reader,
      IXmlDocument document,
      IXmlNode currentNode,
      string attributeName,
      XmlNamespaceManager manager,
      string attributePrefix)
    {
      string str1 = XmlConvert.EncodeName(attributeName);
      string str2 = reader.Value.ToString();
      IXmlNode attribute = !string.IsNullOrEmpty(attributePrefix) ? document.CreateAttribute(str1, manager.LookupNamespace(attributePrefix), str2) : document.CreateAttribute(str1, str2);
      ((IXmlElement) currentNode).SetAttributeNode(attribute);
    }

    private string ConvertTokenToXmlValue(JsonReader reader)
    {
      if (reader.TokenType == JsonToken.String)
        return reader.Value == null ? (string) null : reader.Value.ToString();
      if (reader.TokenType == JsonToken.Integer)
        return XmlConvert.ToString(Convert.ToInt64(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
      if (reader.TokenType == JsonToken.Float)
      {
        if (reader.Value is Decimal)
          return XmlConvert.ToString((Decimal) reader.Value);
        return reader.Value is float ? XmlConvert.ToString((float) reader.Value) : XmlConvert.ToString(Convert.ToDouble(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
      }
      if (reader.TokenType == JsonToken.Boolean)
        return XmlConvert.ToString(Convert.ToBoolean(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
      if (reader.TokenType == JsonToken.Date)
      {
        if (reader.Value is DateTimeOffset)
          return XmlConvert.ToString((DateTimeOffset) reader.Value);
        DateTime dateTime = Convert.ToDateTime(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        return XmlConvert.ToString(dateTime, DateTimeUtils.ToSerializationMode(dateTime.Kind));
      }
      if (reader.TokenType == JsonToken.Null)
        return (string) null;
      throw JsonSerializationException.Create(reader, "Cannot get an XML string value from token type '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
    }

    private void ReadArrayElements(
      JsonReader reader,
      IXmlDocument document,
      string propertyName,
      IXmlNode currentNode,
      XmlNamespaceManager manager)
    {
      string prefix = MiscellaneousUtils.GetPrefix(propertyName);
      IXmlElement element1 = this.CreateElement(propertyName, document, prefix, manager);
      currentNode.AppendChild((IXmlNode) element1);
      int num = 0;
      while (reader.Read() && reader.TokenType != JsonToken.EndArray)
      {
        this.DeserializeValue(reader, document, manager, propertyName, (IXmlNode) element1);
        ++num;
      }
      if (this.WriteArrayAttribute)
        this.AddJsonArrayAttribute(element1, document);
      if (num != 1 || !this.WriteArrayAttribute)
        return;
      foreach (IXmlNode childNode in element1.ChildNodes)
      {
        if (childNode is IXmlElement element2 && element2.LocalName == propertyName)
        {
          this.AddJsonArrayAttribute(element2, document);
          break;
        }
      }
    }

    private void AddJsonArrayAttribute(IXmlElement element, IXmlDocument document)
    {
      element.SetAttributeNode(document.CreateAttribute("json:Array", "http://james.newtonking.com/projects/json", "true"));
      if (!(element is XElementWrapper) || element.GetPrefixOfNamespace("http://james.newtonking.com/projects/json") != null)
        return;
      element.SetAttributeNode(document.CreateAttribute("xmlns:json", "http://www.w3.org/2000/xmlns/", "http://james.newtonking.com/projects/json"));
    }

    private Dictionary<string, string> ReadAttributeElements(
      JsonReader reader,
      XmlNamespaceManager manager)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      bool flag1 = false;
      bool flag2 = false;
      if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null && (reader.TokenType != JsonToken.Boolean && reader.TokenType != JsonToken.Integer) && (reader.TokenType != JsonToken.Float && reader.TokenType != JsonToken.Date && reader.TokenType != JsonToken.StartConstructor))
      {
        while (!flag1 && !flag2 && reader.Read())
        {
          switch (reader.TokenType)
          {
            case JsonToken.PropertyName:
              string str1 = reader.Value.ToString();
              if (!string.IsNullOrEmpty(str1))
              {
                switch (str1[0])
                {
                  case '$':
                    if (str1 == "$values" || str1 == "$id" || (str1 == "$ref" || str1 == "$type") || str1 == "$value")
                    {
                      string prefix = manager.LookupPrefix("http://james.newtonking.com/projects/json");
                      if (prefix == null)
                      {
                        int? nullable = new int?();
                        while (manager.LookupNamespace("json" + (object) nullable) != null)
                          nullable = new int?(nullable.GetValueOrDefault() + 1);
                        prefix = "json" + (object) nullable;
                        dictionary.Add("xmlns:" + prefix, "http://james.newtonking.com/projects/json");
                        manager.AddNamespace(prefix, "http://james.newtonking.com/projects/json");
                      }
                      if (str1 == "$values")
                      {
                        flag1 = true;
                        continue;
                      }
                      string str2 = str1.Substring(1);
                      reader.Read();
                      if (!JsonTokenUtils.IsPrimitiveToken(reader.TokenType))
                        throw JsonSerializationException.Create(reader, "Unexpected JsonToken: " + (object) reader.TokenType);
                      string str3 = reader.Value != null ? reader.Value.ToString() : (string) null;
                      dictionary.Add(prefix + ":" + str2, str3);
                      continue;
                    }
                    flag1 = true;
                    continue;
                  case '@':
                    string str4 = str1.Substring(1);
                    reader.Read();
                    string xmlValue = this.ConvertTokenToXmlValue(reader);
                    dictionary.Add(str4, xmlValue);
                    string prefix1;
                    if (this.IsNamespaceAttribute(str4, out prefix1))
                    {
                      manager.AddNamespace(prefix1, xmlValue);
                      continue;
                    }
                    continue;
                  default:
                    flag1 = true;
                    continue;
                }
              }
              else
              {
                flag1 = true;
                continue;
              }
            case JsonToken.Comment:
              flag2 = true;
              continue;
            case JsonToken.EndObject:
              flag2 = true;
              continue;
            default:
              throw JsonSerializationException.Create(reader, "Unexpected JsonToken: " + (object) reader.TokenType);
          }
        }
      }
      return dictionary;
    }

    private void CreateInstruction(
      JsonReader reader,
      IXmlDocument document,
      IXmlNode currentNode,
      string propertyName)
    {
      if (propertyName == "?xml")
      {
        string version = (string) null;
        string encoding = (string) null;
        string standalone = (string) null;
        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
        {
          string str = reader.Value.ToString();
          if (!(str == "@version"))
          {
            if (!(str == "@encoding"))
            {
              if (!(str == "@standalone"))
                throw JsonSerializationException.Create(reader, "Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
              reader.Read();
              standalone = reader.Value.ToString();
            }
            else
            {
              reader.Read();
              encoding = reader.Value.ToString();
            }
          }
          else
          {
            reader.Read();
            version = reader.Value.ToString();
          }
        }
        IXmlNode xmlDeclaration = document.CreateXmlDeclaration(version, encoding, standalone);
        currentNode.AppendChild(xmlDeclaration);
      }
      else
      {
        IXmlNode processingInstruction = document.CreateProcessingInstruction(propertyName.Substring(1), reader.Value.ToString());
        currentNode.AppendChild(processingInstruction);
      }
    }

    private void CreateDocumentType(JsonReader reader, IXmlDocument document, IXmlNode currentNode)
    {
      string name = (string) null;
      string publicId = (string) null;
      string systemId = (string) null;
      string internalSubset = (string) null;
      while (reader.Read() && reader.TokenType != JsonToken.EndObject)
      {
        string str = reader.Value.ToString();
        if (!(str == "@name"))
        {
          if (!(str == "@public"))
          {
            if (!(str == "@system"))
            {
              if (!(str == "@internalSubset"))
                throw JsonSerializationException.Create(reader, "Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
              reader.Read();
              internalSubset = reader.Value.ToString();
            }
            else
            {
              reader.Read();
              systemId = reader.Value.ToString();
            }
          }
          else
          {
            reader.Read();
            publicId = reader.Value.ToString();
          }
        }
        else
        {
          reader.Read();
          name = reader.Value.ToString();
        }
      }
      IXmlNode xmlDocumentType = document.CreateXmlDocumentType(name, publicId, systemId, internalSubset);
      currentNode.AppendChild(xmlDocumentType);
    }

    private IXmlElement CreateElement(
      string elementName,
      IXmlDocument document,
      string elementPrefix,
      XmlNamespaceManager manager)
    {
      string str = XmlConvert.EncodeName(elementName);
      string namespaceUri = string.IsNullOrEmpty(elementPrefix) ? manager.DefaultNamespace : manager.LookupNamespace(elementPrefix);
      return string.IsNullOrEmpty(namespaceUri) ? document.CreateElement(str) : document.CreateElement(str, namespaceUri);
    }

    private void DeserializeNode(
      JsonReader reader,
      IXmlDocument document,
      XmlNamespaceManager manager,
      IXmlNode currentNode)
    {
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.StartConstructor:
            string propertyName1 = reader.Value.ToString();
            while (reader.Read() && reader.TokenType != JsonToken.EndConstructor)
              this.DeserializeValue(reader, document, manager, propertyName1, currentNode);
            break;
          case JsonToken.PropertyName:
            if (currentNode.NodeType == XmlNodeType.Document && document.DocumentElement != null)
              throw JsonSerializationException.Create(reader, "JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifing a DeserializeRootElementName.");
            string propertyName2 = reader.Value.ToString();
            reader.Read();
            if (reader.TokenType == JsonToken.StartArray)
            {
              int num = 0;
              while (reader.Read() && reader.TokenType != JsonToken.EndArray)
              {
                this.DeserializeValue(reader, document, manager, propertyName2, currentNode);
                ++num;
              }
              if (num == 1 && this.WriteArrayAttribute)
              {
                using (List<IXmlNode>.Enumerator enumerator = currentNode.ChildNodes.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    if (enumerator.Current is IXmlElement current && current.LocalName == propertyName2)
                    {
                      this.AddJsonArrayAttribute(current, document);
                      break;
                    }
                  }
                  break;
                }
              }
              else
                break;
            }
            else
            {
              this.DeserializeValue(reader, document, manager, propertyName2, currentNode);
              break;
            }
          case JsonToken.Comment:
            currentNode.AppendChild(document.CreateComment((string) reader.Value));
            break;
          case JsonToken.EndObject:
            return;
          case JsonToken.EndArray:
            return;
          default:
            throw JsonSerializationException.Create(reader, "Unexpected JsonToken when deserializing node: " + (object) reader.TokenType);
        }
      }
      while (reader.TokenType == JsonToken.PropertyName || reader.Read());
    }

    /// <summary>Checks if the attributeName is a namespace attribute.</summary>
    /// <param name="attributeName">Attribute name to test.</param>
    /// <param name="prefix">The attribute name prefix if it has one, otherwise an empty string.</param>
    /// <returns>True if attribute name is for a namespace attribute, otherwise false.</returns>
    private bool IsNamespaceAttribute(string attributeName, out string prefix)
    {
      if (attributeName.StartsWith("xmlns", StringComparison.Ordinal))
      {
        if (attributeName.Length == 5)
        {
          prefix = string.Empty;
          return true;
        }
        if (attributeName[5] == ':')
        {
          prefix = attributeName.Substring(6, attributeName.Length - 6);
          return true;
        }
      }
      prefix = (string) null;
      return false;
    }

    private bool ValueAttributes(List<IXmlNode> c)
    {
      foreach (IXmlNode xmlNode in c)
      {
        if (xmlNode.NamespaceUri != "http://james.newtonking.com/projects/json")
          return true;
      }
      return false;
    }

    /// <summary>
    /// Determines whether this instance can convert the specified value type.
    /// </summary>
    /// <param name="valueType">Type of the value.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can convert the specified value type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type valueType)
    {
      return typeof (XObject).IsAssignableFrom(valueType) || typeof (XmlNode).IsAssignableFrom(valueType);
    }
  }
}
