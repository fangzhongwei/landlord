using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class Resource
{
    [ProtoMember(1)]
    public int version = 0;
    [ProtoMember(2)]
    public int resourceType = 0;
    [ProtoMember(3)]
    public string code = "";
    [ProtoMember(4)]
    public string lan = "";
    [ProtoMember(5)]
    public string desc = "";
    [ProtoMember(6)]
    public string ext1 = "";
    [ProtoMember(7)]
    public string ext2 = "";
    [ProtoMember(8)]
    public string ext3 = "";
    [ProtoMember(9)]
    public string ext4 = "";
    [ProtoMember(10)]
    public string ext5 = "";
}
