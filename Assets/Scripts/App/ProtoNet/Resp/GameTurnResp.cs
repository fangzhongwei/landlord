using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class GameTurnResp
{
    [ProtoMember(1)]
    public long gameId = 0;
    [ProtoMember(2)]
    public int gameType = 0;
    [ProtoMember(3)]
    public int deviceType = 0;
    [ProtoMember(4)]
    public string cards = "";
    [ProtoMember(5)]
    public string landlordCards = "";
    [ProtoMember(6)]
    public int baseAmount = 0;
    [ProtoMember(7)]
    public int multiples = 0;
    [ProtoMember(8)]
    public string previousNickname = "";
    [ProtoMember(9)]
    public int previousCardsCount = 0;
    [ProtoMember(10)]
    public string nextNickname = "";
    [ProtoMember(11)]
    public int nextCardsCount = 0;
    [ProtoMember(12)]
    public bool choosingLandlord = false;
    [ProtoMember(13)]
    public bool landlord = false;
    [ProtoMember(14)]
    public bool turnToPlay = false;
    [ProtoMember(15)]
    public string ext1 = "";
    [ProtoMember(16)]
    public string ext2 = "";
    [ProtoMember(17)]
    public string ext3 = "";
    [ProtoMember(18)]
    public string ext4 = "";
    [ProtoMember(19)]
    public string ext5 = "";
}
