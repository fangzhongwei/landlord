using System.Collections.Generic;
using App.Base;
using Assets.Scripts.App.Helper;
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

        if (allReady2GoPoints.Count == 0)
        {
            Debug.Log("no card selected.");
            ShowMessage(ErrorCode.PLAY_NO_CARD_SELECTED);
        }
        else
        {
            TypeWithPoints typeWithPoints = CardHelper.GetInstance().JudgeType(allReady2GoPoints);
            Debug.Log("type:" + typeWithPoints);
            GameObject obj;
            foreach (int point in allReady2GoPoints)
            {
//                obj = GameObject.FindGameObjectWithTag(CardHelper.GetInstance().GetTag(point));
//                obj.transform.localScale = Vector3.one * 0.4f;
            }
        }

    }
}
