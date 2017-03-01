using System.Collections.Generic;
using App.Base;
using App.Helper;
using App.VO;
using Assets.Scripts.App.Helper;
using UnityEngine;

public class Controller : WebSocketMonoBehaviour {

    public float timer = 1.0f;

    // Use this for initialization
	void Start ()
	{
//	    FindBaseUis();
//        AddDBManager();
	    SeatWatch watch = new SeatWatch();
	    watch.cards = "311,211,412,312,212,112,413,313,213,113,414,314,214,114,415,315,215,115,516,517";
	    //watch.cards = "411,311,211,412,312,212,112,413,313,213,113";
	    //.cards = "411,311,211,412,312";
//	    watch.cards = "516,517";
//	    watch.cards = "517";
	    watch.cards = "103,104,105,106,107,108,109,110,111,112,113,114,115,203,204,205,206,207,208,209";
	    RenderWatch(watch);
	    //StartWebSocket("ws://127.0.0.1:9000/greeter");
	}

    private List<GameObject> cubes = new List<GameObject>();

	// Update is called once per frame
	void Update () {
	    // timer -= Time.deltaTime;
	    //if (timer <= 0) {
	       //Debug.Log(string.Format("Timer1 is up !!! time=${0}", Time.time));
	        //Send(Time.time.ToString().GetASCIIBytes());
	        // timer = 1.0f;
	    // }

	    foreach (GameObject c in cubes.ToArray())
	    {
	        if (c != null)
	        {
	            Vector3 translation = Vector3.forward * 1;
	            Vector3 transformDirection = Camera.main.transform.TransformDirection(translation);
	            c.transform.position += (transformDirection);
	        }
	    }
	}

    public void DoSomething()
    {
        Debug.Log("you found me.");
    }

    public override void HandleSocketResponse(SocketResponse socketResponse)
    {
        string action = socketResponse.p2;
        switch (action)
        {
            case "seatWatch":
                SeatWatch(socketResponse);
                break;
        }
    }

    void SeatWatch(SocketResponse socketResponse)
    {
        //    1: string code,
        //    2: string action,
        //    3: i64 gameId = 0,
        //    4: i32 gameType = 0,
        //    5: i32 deviceType = 0,
        //    6: string cards = "",
        //    7: string landlordCards = "",
        //    8: i32 baseAmount = 0,
        //    9: i32 multiples = 0,
        //    10: string previousNickname = "",
        //    11: i32 previousCardsCount = 0,
        //    12: string nextNickname = "",
        //    13: i32 nextCardsCount = 0,
        //    14: bool choosingLandlord = false,
        //    15: bool landlord = false,
        //    16: bool turnToplay = false,
        //    17: string fingerprint = "",
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
        SeatWatch watch = new SeatWatch();
        watch.gameId = long.Parse(socketResponse.p3);
        watch.gameType = int.Parse(socketResponse.p4);
        watch.deviceType = int.Parse(socketResponse.p5);
        watch.cards = socketResponse.p6;
        watch.landlordCards = socketResponse.p7;
        watch.baseAmount = int.Parse(socketResponse.p8);
        watch.multiples = int.Parse(socketResponse.p9);
        watch.previousNickname = socketResponse.p10;
        watch.previousCardsCount = int.Parse(socketResponse.p11);
        watch.nextNickname = socketResponse.p12;
        watch.nextCardsCount = int.Parse(socketResponse.p13);
        watch.choosingLandlord = bool.Parse(socketResponse.p14);
        watch.landlord = bool.Parse(socketResponse.p15);
        watch.turnToPlay = bool.Parse(socketResponse.p16);
        watch.fingerPrint = socketResponse.p17;

        return watch;
    }

    private void RenderWatch(SeatWatch watch)
    {
        var cardIdArray = watch.cards.Split(Constants.CARDS_SEPERATOR);
        var length = cardIdArray.Length;
        float mid = (float)length / 2;
        GameObject cardObj;
        string objTag;
        Vector3 lp;
        for (int i = 0; i < length; i++)
        {
            objTag = CardHelper.GetInstance().GetTag(int.Parse(cardIdArray[i]));
            cardObj = GameObject.FindGameObjectWithTag(objTag);

            lp = new Vector3((i - mid) * 0.15f, 0, - 8.0f + (length - i - 1) * 0.001f);
            cardObj.transform.localPosition = lp;
            cardObj.GetComponent<CardAttr>().idx = i;
            cardObj.GetComponent<CardAttr>().ready2go = false;
            Debug.Log(string.Format("{0}, postion:{1}", cardObj.tag, cardObj.transform.position));
        }
    }
}