using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class SocketRequest
{
    [ProtoMember(1)]
    public string p1 = "";
    [ProtoMember(2)]
    public string p2 = "";
    [ProtoMember(3)]
    public string p3 = "";
    [ProtoMember(4)]
    public string p4 = "";
    [ProtoMember(5)]
    public string p5 = "";
    [ProtoMember(6)]
    public string p6 = "";
    [ProtoMember(7)]
    public string p7 = "";
    [ProtoMember(8)]
    public string p8 = "";
    [ProtoMember(9)]
    public string p9 = "";
}
