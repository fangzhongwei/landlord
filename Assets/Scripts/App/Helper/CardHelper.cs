using System;
using System.Collections.Generic;
using App.Base;
using UnityEngine;

public class CardHelper
{
    private static readonly CardHelper instance = new CardHelper();

    private readonly Dictionary<int, string> cardTagDic;

    private readonly CardCompare _cardCompare = new CardCompare();

    private TypeWithPoints twp;

    private CardHelper()
    {
        cardTagDic = new Dictionary<int, string>();

        cardTagDic.Add(103, "card_c3");
        cardTagDic.Add(104, "card_c4");
        cardTagDic.Add(105, "card_c5");
        cardTagDic.Add(106, "card_c6");
        cardTagDic.Add(107, "card_c7");
        cardTagDic.Add(108, "card_c8");
        cardTagDic.Add(109, "card_c9");
        cardTagDic.Add(110, "card_c10");
        cardTagDic.Add(111, "card_cj");
        cardTagDic.Add(112, "card_cq");
        cardTagDic.Add(113, "card_ck");
        cardTagDic.Add(114, "card_ca");
        cardTagDic.Add(115, "card_c2");

        cardTagDic.Add(203, "card_d3");
        cardTagDic.Add(204, "card_d4");
        cardTagDic.Add(205, "card_d5");
        cardTagDic.Add(206, "card_d6");
        cardTagDic.Add(207, "card_d7");
        cardTagDic.Add(208, "card_d8");
        cardTagDic.Add(209, "card_d9");
        cardTagDic.Add(210, "card_d10");
        cardTagDic.Add(211, "card_dj");
        cardTagDic.Add(212, "card_dq");
        cardTagDic.Add(213, "card_dk");
        cardTagDic.Add(214, "card_da");
        cardTagDic.Add(215, "card_d2");

        cardTagDic.Add(303, "card_h3");
        cardTagDic.Add(304, "card_h4");
        cardTagDic.Add(305, "card_h5");
        cardTagDic.Add(306, "card_h6");
        cardTagDic.Add(307, "card_h7");
        cardTagDic.Add(308, "card_h8");
        cardTagDic.Add(309, "card_h9");
        cardTagDic.Add(310, "card_h10");
        cardTagDic.Add(311, "card_hj");
        cardTagDic.Add(312, "card_hq");
        cardTagDic.Add(313, "card_hk");
        cardTagDic.Add(314, "card_ha");
        cardTagDic.Add(315, "card_h2");

        cardTagDic.Add(403, "card_s3");
        cardTagDic.Add(404, "card_s4");
        cardTagDic.Add(405, "card_s5");
        cardTagDic.Add(406, "card_s6");
        cardTagDic.Add(407, "card_s7");
        cardTagDic.Add(408, "card_s8");
        cardTagDic.Add(409, "card_s9");
        cardTagDic.Add(410, "card_s10");
        cardTagDic.Add(411, "card_sj");
        cardTagDic.Add(412, "card_sq");
        cardTagDic.Add(413, "card_sk");
        cardTagDic.Add(414, "card_sa");
        cardTagDic.Add(415, "card_s2");

        cardTagDic.Add(516, "card_jb");
        cardTagDic.Add(517, "card_jr");
    }

    public static CardHelper GetInstance()
    {
        return instance;
    }

    public string GetTag(int cardId)
    {
        return cardTagDic[cardId];
    }

    private TypeWithPoints tpInvalid = new TypeWithPoints {cardsType = CardsType.Invalid};

    public TypeWithPoints JudgeType(List<int> list)
    {
        Debug.Log("JudgeType, points:" + Join(list));
        //plain list
        List<int> points = new List<int>(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            points.Add(list[i] % 100);
        }
        Debug.Log("Plain points:" + Join(points));
        points.Sort();
        Debug.Log("Points after sort:" + Join(points));
        TypeWithPoints tp = new TypeWithPoints();

        if (isSingle(points))
        {
            tp.cardsType = CardsType.Single;
            tp.p = points[0];
            return tp;
        }

        if (isDouble(points))
        {
            tp.cardsType = CardsType.Doub;
            tp.p = points[0];
            return tp;
        }

        if (points.Count == 2 && points[0] == 16 && points[1] == 17)
        {
            tp.cardsType = CardsType.DoubJoker;
            return tp;
        }

        if (points.Count == 4 && points[0] == points[3])
        {
            tp.cardsType = CardsType.DoubJoker;
            tp.p = points[0];
            return tp;
        }

        if (isSeq(points))
        {
            tp.cardsType = CardsType.Seq;
            tp.ps = points;
            return tp;
        }

        List<int> doubSeq = judgeDoubSeq(points);
        if (doubSeq != null)
        {
            tp.cardsType = CardsType.DoubSeq;
            tp.ps = doubSeq;
            return tp;
        }

        List<int> threeSeq = judgeThreeSeq(points);
        if (threeSeq != null)
        {
            tp.cardsType = CardsType.ThreeSeq;
            tp.ps = threeSeq;
            return tp;
        }

        List<int> threeWithOneSeq = judgeThreeWithOneSeq(points);
        if (threeWithOneSeq != null)
        {
            tp.cardsType = CardsType.ThreeWithOneSeq;
            tp.ps = threeWithOneSeq;
            return tp;
        }

        List<int> threeWithTwoSeq = judgeThreeWithTwoSeq(points);
        if (threeWithTwoSeq != null)
        {
            tp.cardsType = CardsType.ThreeWithTwoSeq;
            tp.ps = threeWithTwoSeq;
            return tp;
        }

        List<int> fourWith2SingleSeq = judgeFourWith2SingleSeq(points);
        if (fourWith2SingleSeq != null)
        {
            tp.cardsType = CardsType.FourWith2SingleSeq;
            tp.ps = fourWith2SingleSeq;
            return tp;
        }

        List<int> fourWith2DoubSeq = judgeFourWith2DoubSeq(points);
        if (fourWith2DoubSeq != null)
        {
            tp.cardsType = CardsType.FourWith2DoubSeq;
            tp.ps = fourWith2DoubSeq;
            return tp;
        }

        return tpInvalid;
    }

    private bool isSingle(List<int> points)
    {
        return points.Count == 1;
    }

    private bool isDouble(List<int> points)
    {
        return points[0] == points[1];
    }

    private bool isSeq(List<int> points)
    {
        if (points.Count < 5 || points.Count > 12)
        {
            return false;
        }
        if (points[points.Count - 1] > 14)
        {
            return false;
        }
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i + 1] - points[i] != 1)
            {
                return false;
            }
        }
        return true;
    }

    private List<int> judgeDoubSeq(List<int> points)
    {
        if (points.Count < 6 || points.Count > 20)
        {
            return null;
        }
        if (points.Count % 2 != 0)
        {
            return null;
        }
        if (points[points.Count - 1] > 14)
        {
            return null;
        }
        List<int> kp = new List<int>(points.Count / 2);
        for (int i = 3; i < points.Count; i += 2)
        {
            if (points[i - 3] != points[i - 2] || points[i - 2] - points[i - 1] != 1 ||
                points[i - 1] != points[i])
            {
                return null;
            }
            if (i == 3)
            {
                kp.Add(points[1]);
            }
            kp.Add(points[i]);
        }
        return kp;
    }

    private List<int> judgeThreeSeq(List<int> points)
    {
        if (points.Count % 3 != 0)
        {
            return null;
        }
        List<int> ps = new List<int>();
        for (int i = 2; i < points.Count; i += 3)
        {
            if (points[i - 2] != points[i - 1] || points[i - 1] != points[i])
            {
                return null;
            }
            ps.Add(points[i]);
        }

        if (points.Count / 3 != ps.Count)
        {
            return null;
        }

        for (int i = 0; i < ps.Count - 1; i++)
        {
            if (ps[i] + 1 != ps[i + 1])
            {
                return null;
            }
        }
        return ps;
    }

    private List<int> judgeThreeWithOneSeq(List<int> points)
    {
        if (points.Count % 4 != 0)
        {
            return null;
        }
        List<int> ps = new List<int>();
        for (int i = 2; i < points.Count; i++)
        {
            if (points[i - 2] == points[i])
            {
                if (i == points.Count - 1 && points[i - 3] != points[i - 2])
                {
                    ps.Add(points[i]);
                }
                else if (i == 2 && points[i] != points[i + 1])
                {
                    ps.Add(points[i]);
                }
                else
                {
                    if (points[i - 3] != points[i - 2] && points[i] != points[i + 1])
                    {
                        ps.Add(points[i]);
                    }
                }
            }
        }
        if (points.Count / 4 != ps.Count)
        {
            return null;
        }
        for (int i = 0; i < ps.Count - 1; i++)
        {
            if (ps[i] + 1 != ps[i + 1])
            {
                return null;
            }
        }
        return ps;
    }

    private List<int> judgeThreeWithTwoSeq(List<int> points)
    {
        if (points.Count % 5 != 0)
        {
            return null;
        }
        List<int> ps3 = new List<int>();
        for (int i = 2; i < points.Count; i++)
        {
            if (points[i - 2] == points[i])
            {
                if (i == points.Count - 1 && points[i - 3] != points[i - 2])
                {
                    ps3.Add(points[i]);
                }
                else if (i == 2 && points[i] != points[i + 1])
                {
                    ps3.Add(points[i]);
                }
                else
                {
                    if (points[i - 3] != points[i - 2] && points[i] != points[i + 1])
                    {
                        ps3.Add(points[i]);
                    }
                }
            }
        }

        if (points.Count / 5 != ps3.Count)
        {
            return null;
        }

        for (int i = 0; i < ps3.Count - 1; i++)
        {
            if (ps3[i] + 1 != ps3[i + 1])
            {
                return null;
            }
        }

        List<int> ps2 = new List<int>();
        for (int i = 1; i < points.Count; i++)
        {
            if (points[i - 1] == points[i])
            {
                if (i == points.Count - 1 && points[i - 2] != points[i - 1])
                {
                    ps2.Add(points[i]);
                }
                else if (i == 1 && points[i] != points[i + 1])
                {
                    ps2.Add(points[i]);
                }
                else
                {
                    if (points[i - 2] != points[i - 1] && points[i] != points[i + 1])
                    {
                        ps2.Add(points[i]);
                    }
                }
            }
        }

        if (points.Count / 5 != ps2.Count)
        {
            return null;
        }

        return ps3;
    }

    private List<int> judgeFourWith2SingleSeq(List<int> points)
    {
        if (points.Count % 6 != 0)
        {
            return null;
        }
        List<int> ps = new List<int>();
        for (int i = 3; i < points.Count; i++)
        {
            if (points[i - 3] == points[i])
            {
                ps.Add(points[i]);
            }
        }
        if (points.Count / 6 != ps.Count)
        {
            return null;
        }
        for (int i = 0; i < ps.Count - 1; i++)
        {
            if (ps[i] + 1 != ps[i + 1])
            {
                return null;
            }
        }
        return ps;
    }

    private List<int> judgeFourWith2DoubSeq(List<int> points)
    {
        if (points.Count % 8 != 0)
        {
            return null;
        }
        List<int> ps4 = new List<int>();
        for (int i = 3; i < points.Count; i++)
        {
            if (points[i - 3] == points[i])
            {
                ps4.Add(points[i]);
            }
        }
        if (points.Count / 8 != ps4.Count)
        {
            return null;
        }
        for (int i = 0; i < ps4.Count - 1; i++)
        {
            if (ps4[i] + 1 != ps4[i + 1])
            {
                return null;
            }
        }

        List<int> ps2 = new List<int>();
        for (int i = 1; i < points.Count; i++)
        {
            if (points[i - 1] == points[i])
            {
                if (i == points.Count - 1 && points[i - 2] != points[i - 1])
                {
                    ps2.Add(points[i]);
                }
                else if (i == 1 && points[i] != points[i + 1])
                {
                    ps2.Add(points[i]);
                }
                else
                {
                    if (points[i - 2] != points[i - 1] && points[i] != points[i + 1])
                    {
                        ps2.Add(points[i]);
                    }
                }
            }
        }

        if (points.Count / 4 != ps2.Count)
        {
            return null;
        }
        return ps4;
    }

    public void Sort(List<int> points)
    {
        points.Sort(_cardCompare);
    }

    public string Join(List<int> list)
    {
        if (list == null || list.Count == 0)
        {
            return "";
        }
        string j = "";

        foreach (int i in list)
        {
            j += i + ",";
        }
        return j.Substring(0, j.Length - 1);
    }

    public void SaveCurrentCardsType(string cardTypeCode, List<int> keysOutside)
    {
        twp = new TypeWithPoints();
        CardsType ct = (CardsType) Enum.Parse(typeof(CardsType), cardTypeCode);
        twp.cardsType = ct;
        switch (ct)
        {
            case CardsType.Single:
                twp.p = keysOutside[0];
                break;
            case CardsType.Doub:
                twp.p = keysOutside[0];
                break;
            case CardsType.Four:
                twp.p = keysOutside[0];
                break;
            default:
                twp.ps = keysOutside;
                break;
        }
    }

    public bool CanPlay(TypeWithPoints typeWithPoints)
    {
        if (twp == null)
        {
            twp = new TypeWithPoints();
            twp.cardsType = CardsType.Any;
        }
        CardsType originalType = twp.cardsType;
        CardsType readyType = typeWithPoints.cardsType;

        switch (originalType)
        {
            case CardsType.Any:
                return true;
            case CardsType.Single:
                if (readyType == CardsType.DoubJoker || readyType == CardsType.Four)
                {
                    return true;
                }
                if (readyType == CardsType.Single)
                {
                    return typeWithPoints.p > twp.p;
                }
                return false;
            case CardsType.Doub:
                if (readyType == CardsType.DoubJoker || readyType == CardsType.Four)
                {
                    return true;
                }
                if (readyType == CardsType.Doub)
                {
                    return typeWithPoints.p > twp.p;
                }
                return false;
            case CardsType.Four:
                if (readyType == CardsType.DoubJoker)
                {
                    return true;
                }
                if (readyType == CardsType.Four)
                {
                    return typeWithPoints.p > twp.p;
                }
                return false;
            case CardsType.DoubJoker:
                return false;
            default:
                if (readyType == CardsType.DoubJoker || readyType == CardsType.Four)
                {
                    return true;
                }
                if (readyType == originalType)
                {
                    List<int> oldList = typeWithPoints.ps;
                    List<int> newList = twp.ps;
                    return oldList.Count == newList.Count && oldList[newList.Count - 1] < newList[newList.Count - 1];
                }
                return false;
        }
    }
}

public class CardCompare : IComparer<int>
{
    public int Compare(int point1, int point2)
    {
        int p1 = point1;
        int p2 = point2;
        if (p1 > p2)
        {
            return 1;
        }
        if (p1 < p2)
        {
            return -1;
        }
        if (point1 > point2)
        {
            return 1;
        }
        if (point1 < point2)
        {
            return -1;
        }
        return 0;
    }
}

public class TypeWithPoints
{
    public CardsType cardsType { get; set; }
    public int p { get; set; }
    public List<int> ps { get; set; }
}