using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LuaFramework;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CSharpGeneratorForProton.Json {
  public interface ConfigElement {
    string GetAttribute(string name);
    string GetText();
    IEnumerable<ConfigElement> GetElements();
    ConfigElement GetElement(string name);
  }

  public interface IGeneratorObject {
    void Read(ConfigElement item);
  }

  public interface IDelayInit {
    void OnDelayInit();
  }

  public static class GeneratorConfig {
    public static string ConfigDir = "Generator/";
    private static List<Action> DelayInitAction = new List<Action>();

    public static void InvokeDelayInitAction() {
      while (DelayInitAction != null && DelayInitAction.Count > 0) {
        var list = DelayInitAction;
        DelayInitAction = null;
        foreach (Action action in list) {
          action();
        }
      }
      DelayInitAction = null;
    }

    public static void AddDelayAction(Action action) {
      if (DelayInitAction == null) {
        DelayInitAction = new List<Action>();
      }
      DelayInitAction.Add(action);
    }
  }

  public static class GeneratorUtility {
    public static object Get(ConfigElement element, string name, object _) {
      string s = element.GetAttribute(name);
      return Convert(s, System.Convert.ToInt32(_));
    }

    public static int Get(ConfigElement element, string name, int _) {
      string s = element.GetAttribute(name);
      return Convert(s, _);
    }

    public static double Get(ConfigElement element, string name, double _) {
      string s = element.GetAttribute(name);
      return Convert(s, _);
    }

    public static string Get(ConfigElement element, string name, string _) {
      string s = element.GetAttribute(name);
      return Convert(s, _);
    }

    public static bool Get(ConfigElement element, string name, bool _) {
      string s = element.GetAttribute(name);
      return Convert(s, _);
    }

    public static T Get<T>(ConfigElement element, string name, T _) where T : IGeneratorObject, new() {
      var node = element.GetElement(name);
      return Convert(node, _);
    }

    /// <summary>
    /// lua function
    /// </summary>
    public static LuaInterface.LuaTable Get(ConfigElement element, string name, LuaInterface.LuaTable _, LuaInterface.LuaTable obj) {
      var node = element.GetElement(name);
      LuaInterface.LuaTable table = Convert(node, obj);
      obj.Dispose();
      obj = null;
      return table;
    }

    public static int[] Gets(ConfigElement element, string itemName, int[] _) {
      return GetArray<int>(element, itemName, _, Convert);
    }

    public static double[] Gets(ConfigElement element, string itemName, double[] _) {
      return GetArray<double>(element, itemName, _, Convert);
    }

    public static string[] Gets(ConfigElement element, string itemName, string[] _) {
      return GetArray<string>(element, itemName, _, Convert);
    }

    public static T[] Gets<T>(ConfigElement element, string itemName, T[] _) where T : IGeneratorObject, new() {
      return GetArray<T>(element, itemName, _, Convert);
    }

    /// <summary>
    /// lua function
    /// </summary>
    public static LuaInterface.LuaTable[] Gets(ConfigElement element, string itemName, LuaInterface.LuaTable[] _, LuaInterface.LuaTable obj) {
      LuaInterface.LuaTable[] tables = GetArray(element, itemName, obj, Convert);
      obj.Dispose();
      obj = null;
      return tables;
    }

    public static T[] Load<T>(string fileName, string itemName) where T : IGeneratorObject, new() {
      using (var stream = GetContentStream(fileName)) {
        var root = LoadRootElement(stream);
        T[] items = GetArray(root, default(T[]), Convert);
        return items != null ? items : new T[0];
      }
    }

    public static T Load<T>(string fileName) where T : IGeneratorObject, new() {
      using (var stream = GetContentStream(fileName)) {
        var root = LoadRootElement(stream);
        return Convert(root, default(T));
      }
    }

    /// <summary>
    /// lua function
    /// </summary>
    public static LuaInterface.LuaTable Load(string fileName, LuaInterface.LuaTable obj) {
      using (var stream = GetContentStream(fileName)) {
        var root = LoadRootElement(stream);
        LuaInterface.LuaTable table = Convert(root, obj);
        obj.Dispose();
        obj = null;
        return table;
      }
    }

    /// <summary>
    /// lua function
    /// </summary>
    public static LuaInterface.LuaTable[] Load(string fileName, string itemName, LuaInterface.LuaTable obj) {
      using (var stream = GetContentStream(fileName)) {
        var root = LoadRootElement(stream);
        LuaInterface.LuaTable[] tables = GetArray(root, obj, Convert);
        obj.Dispose();
        obj = null;
        return tables != null ? tables : null;
      }
    }

    /// <summary>
    /// lua function
    /// </summary>
    private static LuaInterface.LuaTable[] GetArray(ConfigElement element, LuaInterface.LuaTable obj, Func<ConfigElement, LuaInterface.LuaTable, LuaInterface.LuaTable> convert) {
      List<LuaInterface.LuaTable> list = new List<LuaInterface.LuaTable>();
      foreach (var node in element.GetElements()) {
        list.Add(convert(node, obj));
      }
      return list.Count > 0 ? list.ToArray() : null;
    }

    /// <summary>
    /// lua function
    /// </summary>
    private static LuaInterface.LuaTable[] GetArray(ConfigElement element, string itemName, LuaInterface.LuaTable obj, Func<ConfigElement, LuaInterface.LuaTable, LuaInterface.LuaTable> convert) {
      var listNode = element.GetElement(itemName);
      if (listNode != null) {
        return GetArray(listNode, obj, convert);
      }
      return null;
    }

    private static T[] GetArray<T>(ConfigElement element, T[] _, Func<ConfigElement, T, T> convert) {
      List<T> list = new List<T>();
      foreach (var node in element.GetElements()) {
        list.Add(convert(node, default(T)));
      }
      return list.Count > 0 ? list.ToArray() : null;
    }

    private static T[] GetArray<T>(ConfigElement element, string itemName, T[] _, Func<ConfigElement, T, T> convert) {
      var listNode = element.GetElement(itemName);
      if (listNode != null) {
        return GetArray<T>(listNode, _, convert);
      }
      return null;
    }

    private static int Convert(ConfigElement e, int _) {
      return Convert(e.GetText(), _);
    }

    private static double Convert(ConfigElement e, double _) {
      return Convert(e.GetText(), _);
    }

    private static string Convert(ConfigElement e, string _) {
      return Convert(e.GetText(), _);
    }

    private static bool Convert(ConfigElement e, bool _) {
      return Convert(e.GetText(), _);
    }

    private static T Convert<T>(ConfigElement e, T _) where T : IGeneratorObject, new() {
      if (e == null) {
        return default(T);
      }

      T t = new T();
      t.Read(e);

      IDelayInit delayInit = t as IDelayInit;
      if (delayInit != null) {
        GeneratorConfig.AddDelayAction(delayInit.OnDelayInit);
      }
      return t;
    }

    private static LuaInterface.LuaTable Convert(ConfigElement e, LuaInterface.LuaTable obj) {
      if (e == null) {
        return null;
      }
      if (obj == null) {
        Debug.LogError("LuaTable is null . -> check lua argments ?");
        return null;
      }
      // 调用元方法__call 也就是new
      LuaInterface.LuaTable table = obj.Invoke<LuaInterface.LuaTable, LuaInterface.LuaTable>("__call", obj);
      // 调用普通方法Read
      table.Call("Read", table, e);
      return table;
    }

    private static int Convert(string s, int _) {
      if (string.IsNullOrEmpty(s)) {
        return 0;
      }

      return int.Parse(s);
    }

    private static double Convert(string s, double _) {
      if (string.IsNullOrEmpty(s)) {
        return 0.0;
      }

      return double.Parse(s);
    }

    private static string Convert(string s, string _) {
      return s;
    }

    private static bool Convert(string s, bool _) {
      if (string.IsNullOrEmpty(s)) {
        return false;
      }
      return bool.Parse(s);
    }

    private static Stream GetContentStream(string fileName) {
      string path = Path.Combine(GeneratorConfig.ConfigDir, fileName + ".json");
      //Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
      //string path = GeneratorConfig.ConfigDir + fileName + ".json";
      ResourceManager manager = LuaHelper.GetResManager();
      TextAsset ta = manager.LoadAsset<TextAsset>(path.Replace('\\', '/'));
      Stream stream = new MemoryStream(ta.bytes);
      return stream;
    }

    private static ConfigElement LoadRootElement(Stream stream) {
      StreamReader reader = new StreamReader(stream, Encoding.UTF8);
      JToken token = JToken.Parse(reader.ReadToEnd());
      return new JsonConfigElement(token);
    }

    public sealed class JsonConfigElement : ConfigElement {
      private JToken element_;

      public JsonConfigElement(JToken element) {
        element_ = element;
      }

      public string GetAttribute(string name) {
        JObject obj = (JObject)element_;
        var attribute = obj.GetOrDefault(name);
        return attribute != null ? attribute.ToString() : null;
      }

      public string GetText() {
        return element_.ToString();
      }

      public IEnumerable<ConfigElement> GetElements() {
        JArray array = (JArray)element_;
        return array.Select(i => new JsonConfigElement(i));
      }

      public ConfigElement GetElement(string name) {
        JObject obj = (JObject)element_;
        var e = obj.GetOrDefault(name);
        return e != null ? new JsonConfigElement(e) : null;
      }
    }
  }
}
