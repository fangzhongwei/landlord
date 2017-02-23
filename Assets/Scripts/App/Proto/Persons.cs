using System;
using ProtoBuf;

[Serializable]
[ProtoContract]
public class Persons
{
    [ProtoMember(1)]
    public string GroupName;

    [ProtoMember(2)]
    public Person[] GroupMembers;
}

[Serializable]
[ProtoContract]
public class Person
{
    [ProtoMember(1)]
    public string Name;

    [ProtoMember(2)]
    public string Age;
}