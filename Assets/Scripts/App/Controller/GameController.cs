using System.Collections.Generic;
using App.Base;
using App.Helper;
using App.VO;
using UnityEngine;

public class GameController : WebSocketMonoBehaviour
{
    public float timer = 1.0f;

    private GameObject playCardsObj;
    private GameObject passObj;
    private GameObject resetObj;
    private GameObject takeLandlordObj;
    private GameObject passLandlordObj;

    // Use this for initialization
    void Start()
    {
        FindBaseUis();
        AddDBManager();

        playCardsObj = GameObject.FindGameObjectWithTag("playCards");
        passObj = GameObject.FindGameObjectWithTag("pass");
        resetObj = GameObject.FindGameObjectWithTag("reset");
        takeLandlordObj = GameObject.FindGameObjectWithTag("takeLandlord");
        passLandlordObj = GameObject.FindGameObjectWithTag("passLandlord");

        // MockUi();
        HideAllBtns();
        StartWebSocket(Constants.WS_ADDRESS);
    }

    private void MockUi()
    {
        SeatWatch watch = new SeatWatch();
        watch.cards = "103,104,105,106,107,108,109,110,111,112,113,114,115,204,205,206,207,208,209,210";
        watch.landlordCards = "303,304,305";
        watch.proCardsInfo = "Exist:-:303,304,305:None";
        RenderWatch(watch);
    }

    public void StartPlay()
    {
        SocketRequest req = new SocketRequest();
        req.p1 = GUIDHelper.generate();
        req.p2 = "join";
        req.p3 = DataHelper.GetInstance().LoadToken(dbManager);
        req.p4 = SystemInfo.deviceUniqueIdentifier;
        req.p5 = "10";
        SendBytes(ProtoHelper.Proto2Bytes(req));
    }

    public override void HandleSocketResponse(SocketResponse socketResponse)
    {
        string action = socketResponse.p2;
        switch (action)
        {
            case "seatWatch":
                RenderResponse(socketResponse);
                break;
        }
    }

    void RenderResponse(SocketResponse socketResponse)
    {
        SeatWatch watch = Convert(socketResponse);
        if (!ValidateSeatWatch(watch))
        {
            ShowMessage(ErrorCode.EC_GAME_INVALID_DATA);
        }
        else
        {
            RenderWatch(watch);
        }
    }

    private bool ValidateSeatWatch(SeatWatch watch)
    {
        if (watch.deviceType != DeviceHelper.getDeviceType() || watch.fingerPrint != SystemInfo.deviceUniqueIdentifier)
        {
            return false;
        }
        return true;
    }

    private SeatWatch Convert(SocketResponse socketResponse)
    {
        //    1: string code,
        //    2: string action,
        //    3: i64 gameId = 0,
        //    4: i32 gameType = 0,
        //    5: i32 deviceType = 0,
        //    6: string cards = "",
        //    7: string landlordCards = "",
        //    8: string proCardsInfo = "",
        //    9: i32 baseAmount = 0,
        //    10: i32 multiples = 0,
        //    11: string previousNickname = "",
        //    12: i32 previousCardsCount = 0,
        //    13: string nextNickname = "",
        //    14: i32 nextCardsCount = 0,
        //    15: string playStatus = "",
        //    16: bool landlord = false,
        //    17: string fingerPrint = "",
        //    18: long memberId = 0,
        //    19: long seatId = 0,
        SeatWatch watch = new SeatWatch();
        watch.gameId = long.Parse(socketResponse.p3);
        watch.gameType = int.Parse(socketResponse.p4);
        watch.deviceType = int.Parse(socketResponse.p5);
        watch.cards = socketResponse.p6;
        watch.landlordCards = socketResponse.p7;
        watch.proCardsInfo = socketResponse.p8;
        watch.baseAmount = int.Parse(socketResponse.p9);
        watch.multiples = int.Parse(socketResponse.p10);
        watch.previousNickname = socketResponse.p11;
        watch.previousCardsCount = int.Parse(socketResponse.p12);
        watch.nextNickname = socketResponse.p13;
        watch.nextCardsCount = int.Parse(socketResponse.p14);
        watch.playStatus = socketResponse.p15;
        watch.landlord = bool.Parse(socketResponse.p16);
        watch.fingerPrint = socketResponse.p17;
        watch.seqInGame = int.Parse(socketResponse.p18);
        watch.seatId = long.Parse(socketResponse.p19);

        return watch;
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
            RenderCardsOutside(pointsOutside);
        }

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
        foreach (int point in PlayManager.GetInstance().AllPointsOutside())
        {
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
        Debug.Log("ready to go:" + CardHelper.GetInstance().Join(allReady2GoPoints));
        foreach (int ready2GoPoint in allReady2GoPoints)
        {
            PlayManager.GetInstance().RemoveHandPoint(ready2GoPoint);
        }
        PlayManager.GetInstance().SetOutsidePoints(allReady2GoPoints);

        Debug.Log("hand:" + CardHelper.GetInstance().Join(PlayManager.GetInstance().AllPointsInHand()));
        Debug.Log("outside:" + CardHelper.GetInstance().Join(PlayManager.GetInstance().AllPointsOutside()));

        RenderCardsInHand(PlayManager.GetInstance().AllPointsInHand());
        RenderCardsOutside(PlayManager.GetInstance().AllPointsOutside());
    }

    public void ResetHandCards()
    {
        RenderCardsInHand(PlayManager.GetInstance().AllPointsInHand());
    }

    public void TakeLanlord()
    {
        HideAllBtns();
        DoTake(true);
    }

    public void GiveupLanlord()
    {
        HideAllBtns();
        DoTake(false);
    }

    private void DoTake(bool take)
    {
        SocketRequest sr = new SocketRequest();
        sr.p1 = GUIDHelper.generate();
        sr.p2 = "takeLandlord";
        sr.p3 = LocalToken();
        sr.p4 = SystemInfo.deviceUniqueIdentifier;
        sr.p5 = AppContext.GetInstance().Watch.gameId.ToString();
        sr.p6 = AppContext.GetInstance().Watch.seatId.ToString();
        sr.p7 = take.ToString();
        SendBytes(ProtoHelper.Proto2Bytes(sr));
    }

    public void SendPlayCards(PlayCardsReq req)
    {
        HideAllBtns();
        SocketRequest sr = new SocketRequest();
        sr.p1 = GUIDHelper.generate();
        sr.p2 = "playCards";
        sr.p3 = LocalToken();
        sr.p4 = SystemInfo.deviceUniqueIdentifier;
        sr.p5 = AppContext.GetInstance().Watch.gameId.ToString();
        sr.p6 = AppContext.GetInstance().Watch.seatId.ToString();
        sr.p7 = AppContext.GetInstance().Watch.seqInGame.ToString();
        sr.p8 = req.typeWithPoints.cardsType.ToString();
        sr.p9 = req.Keys();
        sr.p10 = CardHelper.GetInstance().Join(req.points);
        sr.p11 = CardHelper.GetInstance().Join(req.handPoints);
        SendBytes(ProtoHelper.Proto2Bytes(sr));
    }

    public void Pass()
    {
        HideAllBtns();
        PlayCardsReq req = new PlayCardsReq();
        req.typeWithPoints = new TypeWithPoints();
        req.typeWithPoints.cardsType = CardsType.Pass;
        SendPlayCards(req);
    }
}