using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class DepositRequestResp
{
    [ProtoMember(1)]
    public string code = "";
    [ProtoMember(2)]
    public string paymentVoucherNo = "";
    [ProtoMember(3)]
    public int tradeStatus = 0;
    [ProtoMember(4)]
    public string extUrl = "";
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
