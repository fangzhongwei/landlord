using System.Collections.Generic;
using App.Base;
using UnityEngine;

public class PlayController : BaseMonoBehaviour
{
    void Start()
    {
        AddDBManager();
        FindBaseUis();
    }

    public void PlayCards()
    {
        List<int> allReady2GoPoints = PlayManager.GetInstance().AllReady2GoPoints();

        Debug.Log("out before play:" + CardHelper.GetInstance().Join(PlayManager.GetInstance().AllPointsOutside()));

        if (allReady2GoPoints.Count == 0)
        {
            Debug.Log("no card selected.");
            ShowMessage(ErrorCode.PLAY_NO_CARD_SELECTED);
        }
        else
        {
            Debug.Log("ready2GoPoints:" + CardHelper.GetInstance().Join(allReady2GoPoints));

            TypeWithPoints typeWithPoints = CardHelper.GetInstance().JudgeType(allReady2GoPoints);
            Debug.Log(string.Format("type:{0},p:{1},ps{2}", typeWithPoints.cardsType, typeWithPoints.p, Join(typeWithPoints.ps)));
            if (typeWithPoints.cardsType.Equals(CardsType.Invalid))
            {
                ShowMessage(ErrorCode.PLAY_INVALID_CARDS_TYPE);
            }
            else if (!CardHelper.GetInstance().CanPlay(typeWithPoints))
            {
                ShowMessage(ErrorCode.PLAY_NOT_BIG_ENOUGH);
            }
            else
            {
                List<int> goPoints = new List<int>(allReady2GoPoints);
                List<int> handPoints = new List<int>(PlayManager.GetInstance().AllPointsInHand());

                PlayCardsReq req = new PlayCardsReq();
                req.typeWithPoints = typeWithPoints;
                req.handPoints = handPoints;
                req.points = goPoints;

                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>().SendMessage("SendPlayCards", req);
            }
        }
    }

    public void Pass()
    {
        TypeWithPoints typeWithPoints = new TypeWithPoints();
        typeWithPoints.cardsType = CardsType.Pass;
        if (!CardHelper.GetInstance().CanPlay(typeWithPoints))
        {
            ShowMessage(ErrorCode.PLAY_INVALID_CARDS_TYPE);
        }
        else
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>().SendMessage("Pass");
        }
    }

    private string Join(List<int> list)
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

    public void ReSelect()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>().SendMessage("ResetHandCards");
    }
}
