using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public static class Lang
{
    static bool inited = false;

    static Dictionary<string, Dictionary<string, string>> translates = new Dictionary<string, Dictionary<string, string>>();
    static string select_language;

    static void Init()
    {
        inited = true;

        var language_files = Resources.LoadAll<TextAsset>("Languages");
        language_files.ToList().ForEach(x => Load(x.name, x.text));

        select_language = GetSelectLanguage();
    }

    static void Load(string filename, string text)
    {
        translates.Add(filename, new Dictionary<string, string>());

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(text);

        var nodes = doc.SelectNodes("languages/string");
        foreach (XmlNode node in nodes)
        {
            translates[filename].Add(node.Attributes[0].Value, node.InnerText);
        }
    }

    public static string GetSelectLanguage()
    {
        if (PlayerPrefs.HasKey("language"))
        {
            return PlayerPrefs.GetString("language");
        }
        else
        {
            return "japanese";
        }
    }

    public static List<string> GetLanguages()
    {
        if (!inited)
        {
            Init();
        }

        return translates.Keys.ToList();
    }

    public static void SetLanguage(string select_language)
    {
        if (!inited)
        {
            Init();
        }

        Lang.select_language = select_language;

        PlayerPrefs.SetString("language", select_language);
    }

    public static string Get(string name)
    {
        if(!inited)
        {
            Init();
        }

        if(translates[select_language].ContainsKey(name))
        {
            return translates[select_language][name];
        }
        else
        {
            return "";
        }
    }
}