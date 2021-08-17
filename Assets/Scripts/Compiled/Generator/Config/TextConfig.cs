//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CSharpGeneratorForProton.Json {
  
  
  public class TextConfig : IGeneratorObject {
    
    // 文本1
    public string Text_1 { get; private set; }
    
    // 文本2
    public string Text_2 { get; private set; }
    
    // 文本3
    public string Text_3 { get; private set; }
    
    // 缓存静态实例减少开销
    private static TextConfig _obj;
    
    public void Read(ConfigElement element) {
      this.Text_1 = GeneratorUtility.Get(element, "Text_1", this.Text_1);
      this.Text_2 = GeneratorUtility.Get(element, "Text_2", this.Text_2);
      this.Text_3 = GeneratorUtility.Get(element, "Text_3", this.Text_3);
      this.OnInit();
    }
    
    protected virtual void OnInit() {
    }
    
    public static TextConfig Load() {
      return _obj = (_obj == null ? Load<TextConfig>() : _obj);
    }
    
    public static T Load<T>()
      where T : TextConfig, new () {
      return GeneratorUtility.Load<T>("TextConfig");
    }
  }
}