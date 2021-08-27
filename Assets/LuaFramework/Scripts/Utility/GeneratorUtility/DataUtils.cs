using System;
using System.IO;
using ProtoBuf;
using UnityEngine;
using LuaFramework;

namespace CSharpGeneratorForProton.Protobuf
{
    public class DataUtils
    {
        public static byte[] ObjectToBytes<T>(T instance)
        {
            try
            {
                byte[] array;
                if (instance == null)
                {
                    array = new byte[0];
                }
                else
                {
                    MemoryStream memoryStream = new MemoryStream();
                    Serializer.Serialize(memoryStream, instance);
                    array = new byte[memoryStream.Length];
                    memoryStream.Position = 0L;
                    memoryStream.Read(array, 0, array.Length);
                    memoryStream.Dispose();
                }

                return array;

            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                return new byte[0];
            }
        }

        public static T BytesToObject<T>(byte[] bytesData)
        {
            if (bytesData.Length == 0)
            {
                return default(T);
            }
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.Write(bytesData, 0, bytesData.Length);
                memoryStream.Position = 0L;
                T result = Serializer.Deserialize<T>(memoryStream);
                memoryStream.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                return default(T);
            }
        }
    }

    [ProtoContract]
    public class Test
    {
        [ProtoMember(1)]
        public string S { get; set; }

        [ProtoMember(2)]
        public string Type { get; set; }

        [ProtoMember(3)]
        public int I { get; set; }

        /// <summary>
        /// 默认构造函数必须有，否则反序列化会报 No parameterless constructor found for x 错误！
        /// </summary>
        public Test() { }

        public static Test Data => new Test
        {
            I = 222,
            S = "xxxxxx",
            Type = "打开的封口费"
        };
    }
}