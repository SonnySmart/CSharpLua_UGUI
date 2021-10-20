using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using LuaInterface;
using libx;

namespace LuaFramework {
  public sealed class MonoBehaviourConverter {
    private sealed class StructField {
      private string s_;
      public bool HasField { get; set; }

      public StructField(object v) {
        StringBuilder sb = new StringBuilder();
        sb.Append('{');
        Type t = v.GetType();
        bool isFirst = true;
        foreach (var field in t.GetFields(BindingFlags.Instance | BindingFlags.Public)) {
          object x = field.GetValue(v);
          if (x != null) {
            var y = Activator.CreateInstance(x.GetType());
            if (!x.EQ(y)) {
              if (isFirst) {
                isFirst = false;
              } else {
                sb.Append(',');
              }
              sb.Append(field.Name);
              sb.Append('=');
              sb.Append(SerializeFieldsInfo.NormalValueToString(x));
              HasField = true;
            }
          }
        }
        sb.Append(',');
        sb.Append(t.FullName);
        sb.Append('}');
        s_ = sb.ToString();
      }

      public override string ToString() {
        return s_;
      }
    }

    private sealed class SerializeFieldsInfo {
      internal abstract class ObjectField {
        public string Name;
        public abstract void FillTo(StringBuilder sb);
      }

      internal sealed class PoolIndexObjectField : ObjectField {
        public int PoolIndex;

        public override void FillTo(StringBuilder sb) {
          sb.Append(PoolIndex);
        }
      }

      internal sealed class ArrayObjectField : ObjectField {
        public bool IsArray;
        public Type ElementType;
        public List<int> PoolIndexs = new List<int>();

        public override void FillTo(StringBuilder sb) {
          string array = ToList(IsArray, ElementType, PoolIndexs, i => i.ToString());
          sb.Append(array);
        }
      }

      internal sealed class MonoBehaviourField {
        public int PoolIndex;
        public string ClassName;
      }

      public Dictionary<string, object> Normals = new Dictionary<string, object>();
      public List<UnityEngine.Object> ObjectsPool = new List<UnityEngine.Object>();
      public List<ObjectField> Objects = new List<ObjectField>();
      public List<MonoBehaviourField> MonoBehaviourFields = new List<MonoBehaviourField>();

      private void AppendNormals(StringBuilder sb) {
        sb.Append('{');
        bool isFirst = true;
        foreach (var normal in Normals) {
          if (isFirst) {
            isFirst = false;
          } else {
            sb.Append(',');
          }
          sb.Append(normal.Key);
          sb.Append('=');
          sb.Append(ValueToString(normal.Value));
        }
        sb.Append("}");
      }

      private void AppendObjects(StringBuilder sb) {
        if (Objects.Count > 0) {
          sb.Append('{');
          bool isFirst = true;
          foreach (var field in Objects) {
            if (isFirst) {
              isFirst = false;
            } else {
              sb.Append(',');
            }
            sb.Append(field.Name);
            sb.Append('=');
            field.FillTo(sb);
          }
          sb.Append('}');
        }
      }

      public string GetSerializeData() {
        bool isEmpty = Normals.Count == 0 && Objects.Count == 0;
        StringBuilder sb = new StringBuilder();
        if (!isEmpty) {
          sb.Append("return{");
          AppendNormals(sb);
          sb.Append(',');
          AppendObjects(sb);
          sb.Append('}');
        }
        return sb.ToString();
      }

      public UnityEngine.Object[] GetSerializeObjects() {
        return ObjectsPool.Count > 0 ? ObjectsPool.ToArray() : null;
      }

      private static string ToList(bool isArray, Type elementType, IList list, Func<object, string> transfore) {
        StringBuilder sb = new StringBuilder();
        sb.Append('{');
        if (list.Count > 0) {
          bool isFirst = true;
          foreach (var i in list) {
            if (isFirst) {
              isFirst = false;
            } else {
              sb.Append(',');
            }
            sb.Append(transfore(i));
          }
          sb.Append(',');
        }
        if (isArray) {
          sb.AppendFormat("System.Array({0})", elementType.FullName);
        } else {
          sb.AppendFormat("System.List({0})", elementType.FullName);
        }
        sb.Append('}');
        return sb.ToString();
      }

      internal static string NormalValueToString(object v) {
        if (v is string) {
          return "\"" + v + "\"";
        }

        return v.ToString();
      }

      private static string ValueToString(object v) {
        if (v is IList) {
          var list = (IList)v;
          bool isArray = list is Array;
          var elementType = isArray ? v.GetType().GetElementType() : v.GetType().GetIListElementType();
          return ToList(isArray, elementType, list, NormalValueToString);
        }

        return NormalValueToString(v);
      }
    }

    private sealed class MonoBehaviourFieldLazy {
      public string ClassName;
      public SerializeFieldsInfo SerializeInfo;
      public LuaBehaviour BridgeMonoBehaviour;

      public void Bind() {
        foreach (var field in SerializeInfo.MonoBehaviourFields) {
          var gameObject = (GameObject)SerializeInfo.ObjectsPool[field.PoolIndex];
          var bridges = gameObject.GetComponents<LuaBehaviour>();
          var item = bridges.Single(i => i.LuaClass == field.ClassName);
          SerializeInfo.ObjectsPool[field.PoolIndex] = item;
        }
        BridgeMonoBehaviour.Bind(ClassName, SerializeInfo.GetSerializeData(), SerializeInfo.GetSerializeObjects());
      }
    }
    // 开发模式路径
    private static readonly string compiledScriptsManifestDevPath = "Lua/Compiled/manifest.lua.bytes";
    // AssetBundle模式路径
    private static readonly string compiledScriptsManifestPath = "Compiled/manifest";
    private static MonoBehaviourConverter default_;

    private HashSet<string> userDefinedNames_;
    private LuaState luaState_;

    public MonoBehaviourConverter() {
      Load();
    }

    private void Load() {
      luaState_ = LuaHelper.GetLuaManager().GetMainState();
      //luaState_ = LuaClient.GetMainState();
      if (luaState_ == null) {
        throw new InvalidProgramException("not found MainState");
      }
      LoadClassNames();
    }

    public static MonoBehaviourConverter Default {
      get {
        if (default_ == null) {
          default_ = new MonoBehaviourConverter();
        }
        return default_;
      }
    }

    private void PauseEdit() {
      // if (Application.isPlaying) {
      //   UnityEngine.Debug.Break();
      // }
    }

    private string GetUserDefineContent() {
      string content = "";
      if (Assets.development) {
        string assetPath = string.Format("{0}/{1}", Application.dataPath, compiledScriptsManifestDevPath);
        if (!File.Exists(assetPath)) {
          PauseEdit();
          throw new InvalidOperationException("please compiled scripts first");
        }
        content = File.ReadAllText(assetPath);
      }
      else {
        content = LuaHelper.GetLuaManager().ReadFile(compiledScriptsManifestPath);
        if (content == null) {
          PauseEdit();
          throw new InvalidOperationException("please compiled scripts first");
        }
      }
      return content;
    }

    private void LoadClassNames() {
      const string kBeginToken = "types = {";
      const string kEndToken = "}";

      string content = GetUserDefineContent();
      
      if (string.IsNullOrEmpty(content)) {
        PauseEdit();
        throw new InvalidOperationException("please compiled scripts first");
      }

      //string content = File.ReadAllText(compiledScriptsManifestPath_);
      int begin = content.IndexOf(kBeginToken);
      if (begin == -1) {
        throw new InvalidProgramException();
      }
      begin += kBeginToken.Length;
      int end = content.IndexOf(kEndToken, begin);
      if (end == -1) {
        throw new InvalidProgramException();
      }

      var userDefines = content.Substring(begin, end - begin).Split(',').Select(i => i.Trim().Trim('"')).ToArray();
      userDefinedNames_ = new HashSet<string>(userDefines);
    }

    private bool IsUserMonoBehaviourExists(GameObject prefab) {
      var childrens = GetChildrenTransform(prefab.transform);
      foreach (var gameObject in childrens) {
        var monoBehaviours = gameObject.GetComponents<LuaBehaviour>();
        foreach (var monoBehaviour in monoBehaviours) {
          if (IsUserDefine(monoBehaviour)) {
            return true;
          }
        }
      }
      return false;
    }

    public bool Do(ref GameObject prefab) {
      if (IsUserMonoBehaviourExists(prefab)) {

        List<MonoBehaviourFieldLazy> monoBehaviourFieldLazys = new List<MonoBehaviourFieldLazy>();
        var childrens = GetChildrenTransform(prefab.transform);
        foreach (var t in childrens) {
          Convert(t.gameObject, monoBehaviourFieldLazys);
        }

        foreach (var i in monoBehaviourFieldLazys) {
          i.Bind();
        }

        return true;
      }
      return false;
    }

    private List<Transform> GetChildrenTransform(Transform parent) {
      List<Transform> list = new List<Transform> { parent };
      GetChildrenTransform(list, parent);
      return list;
    }

    private void GetChildrenTransform(List<Transform> list, Transform parent) {
      int count = parent.childCount;
      for (int i = 0; i < count; i++) {
        Transform child = parent.GetChild(i);
        list.Add(child);
        GetChildrenTransform(list, child);
      }
    }

    private bool IsUserDefine(Type type) {
      return userDefinedNames_.Contains(type.FullName);
    }

    private bool IsUserDefine(LuaBehaviour behaviour) {
      return userDefinedNames_.Contains(behaviour.LuaClass);
    }

    private void Convert(GameObject gameObject, List<MonoBehaviourFieldLazy> lazys) {
      var monoBehaviours = gameObject.GetComponents<LuaBehaviour>();
      foreach (var monoBehaviour in monoBehaviours) {
        if (IsUserDefine(monoBehaviour)) {
          Convert(monoBehaviour, lazys);
        }
      }
    }

    private bool IsSerializedField(FieldInfo field) {
      if (field.IsPublic) {
        return !field.IsDefined(typeof(HideInInspector), false);
      } else {
        return field.IsDefined(typeof(SerializeField), false);
      }
    }

    private void Convert(LuaBehaviour monoBehaviour, List<MonoBehaviourFieldLazy> lazys) {
      Type type = monoBehaviour.GetType();
      string className = type.Name;
      using (var luaClass = luaState_.GetTable(className)) {
        if (luaClass == null) {
          PauseEdit();
          throw new InvalidOperationException($"{className} is not found in lua env");
        }

        SerializeFieldsInfo info = new SerializeFieldsInfo();
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (FieldInfo field in fields) {
          if (IsSerializedField(field)) {
            Convert(monoBehaviour, luaClass, field, info);
          }
        }

        GameObject gameObject = monoBehaviour.gameObject;
        UnityEngine.Object.DestroyImmediate(monoBehaviour, true);
        var bridgeMonoBehaviour = gameObject.AddComponent<LuaBehaviour>();
        if (info.MonoBehaviourFields.Count == 0) {
          bridgeMonoBehaviour.Bind(className, info.GetSerializeData(), info.GetSerializeObjects());
        } else {
          lazys.Add(new MonoBehaviourFieldLazy() {
            ClassName = className,
            SerializeInfo = info,
            BridgeMonoBehaviour = bridgeMonoBehaviour,
          });
        }
      }
    }

    private static bool IsSameRootGameObject(GameObject x, GameObject y) {
      //string pathX = AssetDatabase.GetAssetPath(x);
      //string pathY = AssetDatabase.GetAssetPath(y);
      //return pathX == pathY;
      return x == y;
    }

    private static bool IsNormalType(Type type, out TypeCode typeCode) {
      typeCode = Type.GetTypeCode(type);
      return typeCode == TypeCode.String || (typeCode >= TypeCode.Boolean && typeCode <= TypeCode.Double);
    }

    private static bool IsNormalType(Type type) {
      TypeCode typeCode;
      return IsNormalType(type, out typeCode);
    }

    private int ConvertUnityEngineGameObject(MonoBehaviour monoBehaviour, object fieldValue, SerializeFieldsInfo info) {
      int poolIndex = info.ObjectsPool.Count;
      var obj = (UnityEngine.Object)fieldValue;
      if (obj is GameObject) {
        var gameObject = (GameObject)obj;
        if (!IsSameRootGameObject(monoBehaviour.gameObject, gameObject)) {
          bool hasChanged = Do(ref gameObject);
          if (hasChanged) {
            obj = gameObject;
          }
        }
      } else if (obj is MonoBehaviour) {
        var mb = (MonoBehaviour)obj;
        var gameObject = mb.gameObject;
        bool isSameRoot = IsSameRootGameObject(monoBehaviour.gameObject, gameObject);
        if (!isSameRoot) {
          bool hasChanged = Do(ref gameObject);
          if (hasChanged) {
            obj = gameObject;
          }
        } else {
          obj = gameObject;
        }
        if (IsUserDefine(mb.GetType())) {
          if (isSameRoot) {
            info.MonoBehaviourFields.Add(new SerializeFieldsInfo.MonoBehaviourField() {
              PoolIndex = poolIndex,
              ClassName = mb.GetType().FullName,
            });
          } else {
            var bridges = gameObject.GetComponents<LuaBehaviour>();
            var mbNew = bridges.Single(i => i.LuaClass == mb.GetType().FullName);
            Contract.Assert(mbNew != null);
            obj = mbNew;
          }
        } else {
          var mbNew = gameObject.GetComponent(mb.GetType());
          obj = mbNew;
        }
      }

      info.ObjectsPool.Add(obj);
      return poolIndex;
    }

    private void Convert(MonoBehaviour monoBehaviour, LuaTable luaClass, FieldInfo field, SerializeFieldsInfo info) {
      Type fieldType = field.FieldType;
      object fieldValue = field.GetValue(monoBehaviour);
      TypeCode fieldTypeCode;

      if (IsNormalType(fieldType, out fieldTypeCode)) {
        object x = fieldValue;
        object y = luaClass.RawGet<string, object>(field.Name);
        if (y is double) {
          if (fieldTypeCode == TypeCode.Char) {
            x = (double)(char)x;
          } else {
            x = System.Convert.ToDouble(x);
          }
        }
        if (!x.EQ(y)) {
          info.Normals.Add(field.Name, x);
        }
        return;
      }

      if (fieldType.IsEnum) {
        int x = (int)(ValueType)fieldValue;
        int y = luaClass.RawGet<string, int>(field.Name);
        if (x != y) {
          info.Normals.Add(field.Name, x);
        }
        return;
      }

      if (fieldType.IsUnityEngineStruct()) {
        var structField = new StructField(fieldValue);
        if (structField.HasField) {
          info.Normals.Add(field.Name, structField);
        }
        return;
      }

      if (fieldValue == null) {
        return;
      }

      if (fieldType.IsUnityEngineObject()) {
        int poolIndex = ConvertUnityEngineGameObject(monoBehaviour, fieldValue, info);
        info.Objects.Add(new SerializeFieldsInfo.PoolIndexObjectField() {
          Name = field.Name,
          PoolIndex = poolIndex,
        });
        return;
      }

      var elementTypeOfIList = fieldType.GetIListElementType();
      if (elementTypeOfIList != null) {
        if (IsNormalType(elementTypeOfIList)) {
          info.Normals.Add(field.Name, fieldValue);
          return;
        } else if (elementTypeOfIList.IsUnityEngineObject()) {
          SerializeFieldsInfo.ArrayObjectField array = new SerializeFieldsInfo.ArrayObjectField() {
            Name = field.Name,
            IsArray = fieldType.IsArray,
            ElementType = elementTypeOfIList
          };
          IList list = (IList)fieldValue;
          foreach (object v in list) {
            int poolIndex = ConvertUnityEngineGameObject(monoBehaviour, v, info);
            array.PoolIndexs.Add(poolIndex);
          }
          info.Objects.Add(array);
          return;
        }
      }

      PauseEdit();
      throw new NotSupportedException($"{monoBehaviour.GetType()}'s field[{field.Name}] type[{fieldType}] not support serialized");
    }
  }

  public static partial class Extensions {
    public static Type GetIListElementType(this Type type) {
      var listType = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));
      return listType != null ? listType.GetGenericArguments().First() : null;
    }

    public static bool IsUnityEngineObject(this Type type) {
      return typeof(UnityEngine.Object).IsAssignableFrom(type);
    }

    public static bool EQ(this object x, object y) {
      return EqualityComparer<object>.Default.Equals(x, y);
    }
    
    public static bool IsUnityEngineStruct(this Type type) {
      return type == typeof(Vector2)
        || type == typeof(Vector3)
        || type == typeof(Vector4);
    }
  }
}