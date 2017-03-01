using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class DepositReq
{
    [ProtoMember(1)]
    public string channelCode = "";
    [ProtoMember(2)]
    public string priceCode = "";
    [ProtoMember(3)]
    public string price = "";
    [ProtoMember(4)]
    public int diamondAmount = 0;
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
