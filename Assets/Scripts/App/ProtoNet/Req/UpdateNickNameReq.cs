using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class UpdateNickNameReq
{
    [ProtoMember(1)]
    public string nickName = "";
    [ProtoMember(2)]
    public string ext1 = "";
    [ProtoMember(3)]
    public string ext2 = "";
    [ProtoMember(4)]
    public string ext3 = "";
    [ProtoMember(5)]
    public string ext4 = "";
    [ProtoMember(6)]
    public string ext5 = "";
}
