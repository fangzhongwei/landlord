using System.Collections.Generic;

public class PlayManager
{
    private static readonly PlayManager instance = new PlayManager();
    private readonly List<int> pointsInHand = new List<int>();
    private readonly List<int> pointsOutside = new List<int>();
    private readonly List<int> ready2GoPoints = new List<int>();

    private PlayManager()
    {
    }

    public static PlayManager GetInstance()
    {
        return instance;
    }

    // hand
    public void AddHandPoint(int point)
    {
        pointsInHand.Add(point);
    }

    public void RemoveHandPoint(int point)
    {
        pointsInHand.Remove(point);
    }

    public List<int> AllPointsInHand()
    {
        return pointsInHand;
    }

    public void ClearAllInHand()
    {
        pointsInHand.Clear();
    }

    //ready 2 go
    public void AddReady2GoPoint(int point)
    {
        ready2GoPoints.Add(point);
    }

    public void RemoveReady2GoPoint(int point)
    {
        ready2GoPoints.Remove(point);
    }

    public List<int> AllReady2GoPoints()
    {
        if (ready2GoPoints.Count > 0)
        {
            CardHelper.GetInstance().Sort(ready2GoPoints);
        }
        return ready2GoPoints;
    }

    public void ClearAlllReady2Go()
    {
        ready2GoPoints.Clear();
    }

    //outside
    public void SetOutsidePoints(List<int> points)
    {
        pointsOutside.Clear();
        pointsOutside.AddRange(points);
    }

    public List<int> AllPointsOutside()
    {
        return pointsOutside;
    }

    public void ClearAlllOutside()
    {
        pointsOutside.Clear();
    }
}
