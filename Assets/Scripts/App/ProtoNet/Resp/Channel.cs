using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class Channel
{
    [ProtoMember(1)]
    public string code = "";
    [ProtoMember(2)]
    public string name = "";
    [ProtoMember(3)]
    public string ext1 = "";
    [ProtoMember(4)]
    public string ext2 = "";
    [ProtoMember(5)]
    public string ext3 = "";
    [ProtoMember(6)]
    public string ext4 = "";
    [ProtoMember(7)]
    public string ext5 = "";
}
