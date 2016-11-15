using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Localization
{
  private static readonly IDictionary<string, string> s_Content = new Dictionary<string, string>();
  private static string s_Language = "EN";

  private static string Language
  {
    get {
      return s_Language;
    }
    set {
      if (s_Language != value) {
        s_Language = value;
        CreateContent();
      }
    }
  }

  private static IDictionary<string, string> Content
  {
    get {
      if (s_Content == null || s_Content.Count == 0) {
        CreateContent();
      }

      return s_Content;
    }
  }

  public static string GetText(string key)
  {
    var result = "";
    Content.TryGetValue(key, out result);

    if (string.IsNullOrEmpty(result)) {
      return key + "[" + Language + "]" + " No Text defined";
    }

    return result;
  }

  public static string GetLanguage()
  {
    return Language;
  }

  public static void SetLanguage(string language)
  {
    Language = language;
  }

  private static IDictionary<string, string> GetContent()
  {
    if (s_Content == null || s_Content.Count == 0) {
      CreateContent();
    }

    return s_Content;
  }

  private static void AddContent(XmlNode xNode)
  {
    foreach (XmlNode node in xNode.ChildNodes) {
      if (node.LocalName == "TextKey") {
        var value = node.Attributes.GetNamedItem("name").Value;
        var text = string.Empty;

        foreach (XmlNode langNode in node) {
          if (langNode.LocalName == s_Language) {
            text = langNode.InnerText;

            if (s_Content.ContainsKey(value)) {
              s_Content.Remove(value);
              s_Content.Add(value, value + " has been found multiple times in the XML allowed only once!");
            } else {
              s_Content.Add(value, !string.IsNullOrEmpty(text) ? text : "No Text for " + value + " found");
            }

            break;
          }
        }
      }
    }
  }

  private static void CreateContent()
  {
    foreach (var asset in Resources.LoadAll("Localization")) {
      var xmlDocument = new XmlDocument();

      xmlDocument.LoadXml(asset.ToString());

      if (s_Content != null) {
        s_Content.Clear();
      }

      var xNode = xmlDocument.ChildNodes.Item(1).ChildNodes.Item(0);

      AddContent(xNode);
    }
  }
}