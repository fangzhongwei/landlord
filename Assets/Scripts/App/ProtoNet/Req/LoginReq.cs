using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class LoginReq
{
    [ProtoMember(1)]
    public int clientId = 0;
    [ProtoMember(2)]
    public string version = "";
    [ProtoMember(3)]
    public int deviceType  = 0;
    [ProtoMember(4)]
    public string fingerPrint = "";
    [ProtoMember(5)]
    public string mobile = "";
    [ProtoMember(6)]
    public string verificationCode = "";
    [ProtoMember(7)]
    public string ext1 = "";
    [ProtoMember(8)]
    public string ext2 = "";
    [ProtoMember(9)]
    public string ext3 = "";
    [ProtoMember(10)]
    public string ext4 = "";
    [ProtoMember(11)]
    public string ext5 = "";
}
