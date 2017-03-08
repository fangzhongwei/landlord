using System.Collections.Generic;

public class PlayCardsReq
{
    public TypeWithPoints typeWithPoints { get; set; }
    public List<int> points { get; set; }

    public string Keys()
    {
        string ks = "";
        if (typeWithPoints.p != 0)
        {
            ks += typeWithPoints.p + ",";
        }
        if (points != null && points.Count != 0)
        {
            foreach (int point in points)
            {
                ks += point + ",";
            }
        }
        return ks.Substring(0, ks.Length - 1);
    }
}
