using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class SimpleApiResponse
{
    [ProtoMember(1)]
    public string code = "";
    [ProtoMember(1)]
    public string ext1 = "";
    [ProtoMember(1)]
    public string ext2 = "";
    [ProtoMember(1)]
    public string ext3 = "";
    [ProtoMember(1)]
    public string ext4 = "";
    [ProtoMember(1)]
    public string ext5 = "";
}
