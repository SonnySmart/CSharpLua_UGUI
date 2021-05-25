

using System;
using System.Collections;
using LuaInterface;

namespace LuaFramework {
  internal sealed class LuaIEnumerator : IEnumerator, IDisposable {
    private LuaTable table_;
    private LuaFunction current_;
    private LuaFunction moveNext_;
    private LuaFunction isIEnumeratorFn_;

    private LuaIEnumerator(LuaTable table) {
      table_ = table;
      current_ = table.GetLuaFunction("getCurrent");
      if (current_ == null) {
        throw new ArgumentNullException();
      }
      moveNext_ = table.GetLuaFunction("MoveNext");
      if (moveNext_ == null) {
        throw new ArgumentNullException();
      }
    }

    public static LuaIEnumerator Create(LuaTable table) {
      var ret = table.GetTable<LuaIEnumerator>("ref");
      if (ret == null) {
        ret = new LuaIEnumerator(table);
        table.SetTable("ref", ret);
      }
      return ret;
    }

    public void Push(IntPtr L) {
      table_.Push();
    }

    public object Current {
      get {
        object obj = current_.Invoke<LuaTable, object>(table_);
        var t = obj as LuaTable;
        if (t != null && IsLuaIEnumerator(t)) {
          return Create(t);
        }
        return obj;
      }
    }

    public void Dispose() {
      if (current_ != null) {
        current_.Dispose();
        current_ = null;
      }

      if (moveNext_ != null) {
        moveNext_.Dispose();
        moveNext_ = null;
      }

      if (table_ != null) {
        table_.Dispose();
        table_ = null;
      }

      if (isIEnumeratorFn_ != null) {
        isIEnumeratorFn_.Dispose();
        isIEnumeratorFn_ = null;
      }
    }

    public bool MoveNext() {
      bool hasNext = moveNext_.Invoke<LuaTable, bool>(table_);
      if (!hasNext) {
        Dispose();
      }
      return hasNext;
    }

    public void Reset() {
      throw new NotSupportedException();
    }

    internal bool IsLuaIEnumerator(LuaTable t) {
      if (isIEnumeratorFn_ == null) {
        LuaState lua = LuaHelper.GetLuaManager().GetMainState();
        isIEnumeratorFn_ = lua.GetFunction("System.IsIEnumerator");
        if (isIEnumeratorFn_ == null) {
          throw new InvalidProgramException();
        }
      }
      return isIEnumeratorFn_.Invoke<LuaTable, bool>(t);
    }
  }
}