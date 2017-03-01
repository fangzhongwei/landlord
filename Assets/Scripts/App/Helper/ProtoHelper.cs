
using System.IO;
using ProtoBuf;

public class ProtoHelper
{
    public static byte[] Proto2Bytes<T>(T instance)
    {
        byte[] data;
        using(var ms = new MemoryStream()) {
            Serializer.Serialize(ms, instance);
            data = ms.ToArray();
        }
        return data;
    }

    public static MemoryStream Bytes2MemoryStream(byte[] bytes)
    {
//        SimpleApiResponse response = Serializer.Deserialize<SimpleApiResponse>(new MemoryStream(bytes));
        return new MemoryStream(bytes);;
    }
}
