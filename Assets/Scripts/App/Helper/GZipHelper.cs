using System;
using System.IO;
using Unity.IO.Compression;
using ConsoleApplication.Helper;

public class GZipHelper
{
    /// 将字节数组进行压缩后返回压缩的字节数组
    /// 需要压缩的数组
    /// 压缩后的数组
    public static byte[] compress(byte[] array)
    {
        MemoryStream stream = new MemoryStream();
        GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress);
        gZipStream.Write(array, 0, array.Length);
        gZipStream.Close();
        return stream.ToArray();
    }

    /// 解压字符数组
    ///
    /// 压缩的数组
    /// 解压后的数组
    public static byte[] Decompress(byte[] data)
    {
        MemoryStream stream = new MemoryStream();

        GZipStream gZipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);

        byte[] bytes = new byte[256];
        int n;
        while ((n = gZipStream.Read(bytes, 0, bytes.Length)) != 0)
        {
            stream.Write(bytes, 0, n);
        }
        gZipStream.Close();
        return stream.ToArray();
    }
}