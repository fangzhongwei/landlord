using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class CheckGameStatusResp
{
    [ProtoMember(1)]
    public string code = "";
    [ProtoMember(2)]
    public long memberId = 0;
    [ProtoMember(3)]
    public bool reconnect = false;
    [ProtoMember(4)]
    public GameTurnResp turn;
    [ProtoMember(5)]
    public string ext1 = "";
    [ProtoMember(6)]
    public string ext2 = "";
    [ProtoMember(7)]
    public string ext3 = "";
    [ProtoMember(8)]
    public string ext4 = "";
    [ProtoMember(9)]
    public string ext5 = "";
}
