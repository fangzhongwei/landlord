using System.Collections.Generic;

public class PlayCardsReq
{
    public TypeWithPoints typeWithPoints { get; set; }
    public List<int> handPoints { get; set; }
    public List<int> points { get; set; }

    public string Keys()
    {
        string ks = "";
        if (typeWithPoints.p != 0)
        {
            ks += typeWithPoints.p + ",";
        }
        if (typeWithPoints.ps != null && typeWithPoints.ps.Count != 0)
        {
            foreach (int point in typeWithPoints.ps)
            {
                ks += point + ",";
            }
        }
        return ks.Equals("") ? "-" : ks.Substring(0, ks.Length - 1);
    }
}
