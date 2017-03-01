using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class PullResourceReq {
    [ProtoMember(1)]
    public int version = 0;
    [ProtoMember(2)]
    public string lan = "";
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

