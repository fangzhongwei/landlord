using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class LoginResp
{
    [ProtoMember(1)]
    public string code = "";
    [ProtoMember(2)]
    public string token = "";
    [ProtoMember(3)]
    public string mobile = "";
    [ProtoMember(4)]
    public int status = 0;
    [ProtoMember(5)]
    public string nickName = "";
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
