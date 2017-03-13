using System.Collections.Generic;
using App.Base;
using App.VO;
using UnityEngine;

public class GameUIRender : BaseMonoBehaviour
{
    private GameObject playCardsObj;
    private GameObject passObj;
    private GameObject resetObj;
    private GameObject takeLandlordObj;
    private GameObject passLandlordObj;

    void Start()
    {
        AddDBManager();
        FindBaseUis();
//        MockUi();
    }

    void Awake()
    {
        playCardsObj = GameObject.FindGameObjectWithTag("playCards");
        passObj = GameObject.FindGameObjectWithTag("pass");
        resetObj = GameObject.FindGameObjectWithTag("reset");
        takeLandlordObj = GameObject.FindGameObjectWithTag("takeLandlord");
        passLandlordObj = GameObject.FindGameObjectWithTag("passLandlord");
        HideAllBtns();
    }

    private void MockUi()
    {
        SeatWatch watch = new SeatWatch();
        watch.cards = "103,104,105,106,107,108,109,110,111,112,113,114,115,204,205,206,207,208,209,210";
        watch.landlordCards = "303,304,305";
        watch.playStatus = Constants.GAME_STATUS_TURN_TO_PLAY;
        watch.proCardsInfo = "Exist:-:303,304,305:None";

        watch.gameId = 1L;
        watch.seatId = 1L;
        watch.seqInGame = 1;

        RenderWatch(watch);

        TouchManager.GetInstance().OnFocus(GameObject.FindGameObjectWithTag(CardHelper.GetInstance().GetTag(103)).transform);
        TouchManager.GetInstance().OnFocus(GameObject.FindGameObjectWithTag(CardHelper.GetInstance().GetTag(104)).transform);
        TouchManager.GetInstance().OnFocus(GameObject.FindGameObjectWithTag(CardHelper.GetInstance().GetTag(105)).transform);
        TouchManager.GetInstance().OnFocus(GameObject.FindGameObjectWithTag(CardHelper.GetInstance().GetTag(106)).transform);
        TouchManager.GetInstance().OnFocus(GameObject.FindGameObjectWithTag(CardHelper.GetInstance().GetTag(107)).transform);
        TouchManager.GetInstance().TouchEnded();


        Debug.Log("out at beginning:" + CardHelper.GetInstance().Join(PlayManager.GetInstance().AllPointsOutside()));

    }

    //0.56, 0.1, 0.82
    private void RenderWatch(SeatWatch watch)
    {
        AppContext.GetInstance().Watch = watch;
        var inHandCardIdArray = watch.cards.Split(Constants.COMMA_SEPERATOR);
        var inHandCapacity = inHandCardIdArray.Length;
        List<int> pointsInHand = new List<int>(inHandCapacity);
        for (int i = 0; i < inHandCapacity; i++)
        {
            pointsInHand.Add(int.Parse(inHandCardIdArray[i]));
        }
        RenderCardsInHand(pointsInHand);

        var proInfoArray = watch.proCardsInfo.Split(Constants.COLON_SEPERATOR);
        if (proInfoArray.Length != 4)
        {
            ShowMessage(ErrorCode.EC_GAME_INVALID_DATA);
            return;
        }
        string cardsTypeCode2Beat = proInfoArray[0];
        string cardsKeys2Beat = proInfoArray[1];
        string cards4Show = proInfoArray[2];
        string proPlayerAction = proInfoArray[3]; // None, Play, Pass

        List<int> pointsOutside = new List<int>();
        if (!"-".Equals(cards4Show))
        {
            var outsideCardIdArray = cards4Show.Split(Constants.COMMA_SEPERATOR);
            var outsideCapacity = outsideCardIdArray.Length;
            for (int i = 0; i < outsideCapacity; i++)
            {
                pointsOutside.Add(int.Parse(outsideCardIdArray[i]));
            }
        }
        RenderCardsOutside(pointsOutside);

        string playStatus = watch.playStatus;
        List<int> keysOutside = new List<int>();
        if (!"-".Equals(cardsKeys2Beat))
        {
            var outsideKeyArray = cardsKeys2Beat.Split(Constants.COMMA_SEPERATOR);
            foreach (string key in outsideKeyArray)
            {
                keysOutside.Add(int.Parse(key));
            }
        }
        CardHelper.GetInstance().SaveCurrentCardsType(cardsTypeCode2Beat, keysOutside);

        ShowBtns(playStatus);
    }

    private void ShowBtns(string playStatus)
    {
        if (Constants.GAME_STATUS_DECIDE_TO_BE_LANDLORD.Equals(playStatus))
        {
            //  takeLandlord， passLandlord
            //  set touch unabled
            TouchManager.GetInstance().doDetect = false;
            playCardsObj.SetActive(false);
            passObj.SetActive(false);
            resetObj.SetActive(false);
            takeLandlordObj.SetActive(true);
            passLandlordObj.SetActive(true);
        }
        else if (Constants.GAME_STATUS_TURN_TO_PLAY.Equals(playStatus))
        {
            //  playCards, , pass, reset
            //  set touch enabled
            TouchManager.GetInstance().doDetect = true;
            playCardsObj.SetActive(true);
            passObj.SetActive(true);
            resetObj.SetActive(true);
            takeLandlordObj.SetActive(false);
            passLandlordObj.SetActive(false);
        }
        else if (Constants.GAME_STATUS_WAITING_OTHER_PLAY.Equals(playStatus))
        {
            HideAllBtns();
        }
    }

    private void HideAllBtns()
    {
        //  set touch unabled
        TouchManager.GetInstance().doDetect = false;
        playCardsObj.SetActive(false);
        passObj.SetActive(false);
        resetObj.SetActive(false);
        takeLandlordObj.SetActive(false);
        passLandlordObj.SetActive(false);
    }

    public void RenderCardsInHand(List<int> list)
    {
        List<int> pointsInHand = new List<int>(list);
        Debug.Log("render hand:" + CardHelper.GetInstance().Join(pointsInHand));
        AllCardsInHandBack();
        var length = pointsInHand.Count;
        float mid = (float) length / 2;
        GameObject cardObj;
        string objTag;
        Vector3 lp;
        int point;
        for (int i = 0; i < length; i++)
        {
            point = pointsInHand[i];
            objTag = CardHelper.GetInstance().GetTag(point);
            cardObj = GameObject.FindGameObjectWithTag(objTag);
            cardObj.transform.localScale = Vector3.one;

            PlayManager.GetInstance().AddHandPoint(point);

            lp = new Vector3((i - mid) * 0.15f, 0, -8.0f + (length - i - 1) * 0.001f);
            cardObj.transform.localPosition = lp;
            SetCardGoAttr(cardObj, true, point, i, false);
        }
    }

    private void AllCardsInHandBack()
    {
        GameObject obj;
        foreach (int point in PlayManager.GetInstance().AllPointsInHand())
        {
            obj = GameObject.FindGameObjectWithTag(CardHelper.GetInstance().GetTag(point));
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = new Vector3(0, 2000f, 0);
            SetCardGoAttr(obj, false, 0, 0, false);
        }
        PlayManager.GetInstance().ClearAllInHand();
    }

    public void RenderCardsOutside(List<int> list)
    {
        List<int> pointsOutside = new List<int>(list);
        Debug.Log("render outside:" + CardHelper.GetInstance().Join(pointsOutside));
        AllCardsOutsideBack();
        var length = pointsOutside.Count;
        float mid = (float) length / 2;
        GameObject cardObj;
        string objTag;
        Vector3 lp;
        int point;

        PlayManager.GetInstance().SetOutsidePoints(pointsOutside);

        for (int i = 0; i < length; i++)
        {
            point = pointsOutside[i];
            objTag = CardHelper.GetInstance().GetTag(point);
            Debug.Log("out tag:" + objTag);
            cardObj = GameObject.FindGameObjectWithTag(objTag);
            cardObj.transform.localScale = Vector3.one * 0.5f;

            lp = new Vector3((i - mid) * 0.5f, 1f, -8.0f + (length - i - 1) * 0.001f);
            cardObj.transform.localPosition = lp;
            SetCardGoAttr(cardObj, false, point, i, false);
        }
    }

    private void AllCardsOutsideBack()
    {
        GameObject obj;
        Debug.Log("backs");
        foreach (int point in PlayManager.GetInstance().AllPointsOutside())
        {
            Debug.Log("back:" + point);
            obj = GameObject.FindGameObjectWithTag(CardHelper.GetInstance().GetTag(point));
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = new Vector3(0, 2000f, 0);
            SetCardGoAttr(obj, false, 0, 0, false);
        }
        PlayManager.GetInstance().ClearAlllOutside();
    }

    private void SetCardGoAttr(GameObject obj, bool inHand, int point, int idx, bool ready2go)
    {
        obj.GetComponent<CardAttr>().inHand = inHand;
        obj.GetComponent<CardAttr>().point = point;
        obj.GetComponent<CardAttr>().idx = idx;
        obj.GetComponent<CardAttr>().ready2go = ready2go;
    }

    public void LetItGo()
    {
        List<int> allReady2GoPoints = PlayManager.GetInstance().AllReady2GoPoints();
        Debug.Log("LetItGo:" + CardHelper.GetInstance().Join(allReady2GoPoints));
        foreach (int ready2GoPoint in allReady2GoPoints)
        {
            PlayManager.GetInstance().RemoveHandPoint(ready2GoPoint);
        }

        Debug.Log("hand:" + CardHelper.GetInstance().Join(PlayManager.GetInstance().AllPointsInHand()));
        Debug.Log("outside:" + CardHelper.GetInstance().Join(PlayManager.GetInstance().AllPointsOutside()));

        RenderCardsInHand(PlayManager.GetInstance().AllPointsInHand());
        RenderCardsOutside(allReady2GoPoints);
        PlayManager.GetInstance().ClearAlllReady2Go();
    }
}
