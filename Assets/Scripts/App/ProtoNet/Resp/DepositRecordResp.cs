using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class DepositRecordResp
{
    [ProtoMember(1)]
    public string code = "";
    [ProtoMember(2)]
    public string paymentVoucherNo = "";
    [ProtoMember(3)]
    public long accountId = 0;
    [ProtoMember(4)]
    public long memberId = 0;
    [ProtoMember(5)]
    public int tradeType = 0;
    [ProtoMember(6)]
    public int tradeStatus = 0;
    [ProtoMember(7)]
    public int diamondAcount = 0;
    [ProtoMember(8)]
    public string amount = "";
    [ProtoMember(9)]
    public string gmtCreate = "";
    [ProtoMember(10)]
    public string gmtUpdate = "";
    [ProtoMember(11)]
    public string ext1 = "";
    [ProtoMember(12)]
    public string ext2 = "";
    [ProtoMember(13)]
    public string ext3 = "";
    [ProtoMember(14)]
    public string ext4 = "";
    [ProtoMember(15)]
    public string ext5 = "";
}
