using ProtoBuf;
using System;
using System.IO;

namespace Framework
{
    public static partial class Helper
    {
        public static byte[] ProtoSerialize<T>(T t) where T : class
        {
            using MemoryStream ms = new();
            Serializer.Serialize(ms, t);
            return ms.ToArray();
        }

        public static T ProtoDeserialize<T>(byte[] bytes)
        {
            return Serializer.Deserialize<T>((ReadOnlyMemory<byte>)bytes);
        }
    }
}
