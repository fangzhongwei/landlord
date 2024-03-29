﻿using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class Price
{
    [ProtoMember(1)]
    public string code = "";
    [ProtoMember(2)]
    public int diamondAmount = 0;
    [ProtoMember(3)]
    public string price = "";
    [ProtoMember(4)]
    public string ext1 = "";
    [ProtoMember(5)]
    public string ext2 = "";
    [ProtoMember(6)]
    public string ext3 = "";
    [ProtoMember(7)]
    public string ext4 = "";
    [ProtoMember(8)]
    public string ext5 = "";
}
