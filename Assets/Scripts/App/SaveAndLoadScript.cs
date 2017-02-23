
using System.IO;
using ProtoBuf;
using UnityEngine;public class SaveAndLoadScript: MonoBehaviour
{
    public string FilePath;
    public Persons MyGroup;
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 100), "Load File1" ))
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                Debug.LogError("Path is empty.");
                return;
            }
            if (!File.Exists(FilePath))
            {
                Debug.LogError("Path is empty.");
                return;
            }
            MyGroup = Serializer.Deserialize<Persons>(new FileStream(FilePath, FileMode.Open, FileAccess.Read));
        }

        if (GUI.Button(new Rect(10, 120, 200, 100), "Save File1" ))
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                Debug.LogError("Path is empty.");
                return;
            }
//            if (MyGroup == null)
//            {
//                Debug.LogError("Value is null.");
//                return;
//            }
//            using (FileStream Stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
//            {
//                Serializer.Serialize(Stream, MyGroup);
//                Stream.Flush();
//            }

            SC sc = new SC()
            {
                deviceType = 1,
                fingerPrint = "aaaabbb12234234234",
                mobile = "15812345678",
                resend = "1",
                lastChannel = 5,
                ext1 = "ext1",
                ext2 = "ext2",
                ext3 = "ext3",
                ext4 = "ext4",
                ext5 = "ext5"
            };
            byte[] data;
            using(var ms = new MemoryStream()) {
                Serializer.Serialize<SC>(ms, sc);
                data = ms.ToArray();
            }

            var a = data.Length;



            using (FileStream Stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                Serializer.Serialize(Stream, sc);
                Stream.Flush();
            }
        }
    }
}