using App.Base;
using App.Helper;
using App.VO;
using UnityEngine;

public class GameController : WebSocketMonoBehaviour
{
    public float timer = 1.0f;

    // Use this for initialization
    void Start()
    {
        FindBaseUis();
        AddDBManager();
        StartWebSocket(Constants.WS_ADDRESS);
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
            GetComponent<GameUIRender>().SendMessage("RenderWatch", watch);
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

    public void ResetHandCards()
    {
        PlayManager.GetInstance().ClearAlllReady2Go();
        Debug.Log("ready2GoPoints after reset:" + CardHelper.GetInstance().Join(PlayManager.GetInstance().AllReady2GoPoints()));
        GetComponent<GameUIRender>().SendMessage("RenderCardsInHand", PlayManager.GetInstance().AllPointsInHand());
    }

    public void TakeLanlord()
    {
        HideAll();
        DoTake(true);
    }

    public void GiveupLanlord()
    {
        HideAll();
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
        HideAll();
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

        GetComponent<GameUIRender>().SendMessage("LetItGo");
    }

    public void Pass()
    {
        HideAll();
        PlayCardsReq req = new PlayCardsReq();
        req.typeWithPoints = new TypeWithPoints();
        req.typeWithPoints.cardsType = CardsType.Pass;
        SendPlayCards(req);
    }

    private void HideAll()
    {
        GetComponent<GameUIRender>().SendMessage("HideAllBtns");
    }
}