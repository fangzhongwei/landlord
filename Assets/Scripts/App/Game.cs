using System;
using System.Collections.Generic;
using App.Base;
using App.Helper;
using App.VO;
using Assets.Scripts.App.Helper;
using UnityEngine;

public class Game : WebSocketMonoBehaviour {

    public float timer = 1.0f;

    // Use this for initialization
	void Start ()
	{
	    FindBaseUis();
        AddDBManager();
	    SeatWatch watch = new SeatWatch();
	    watch.cards = "311,211,412,312,212,112,413,313,213,113,414,314,214,114,415,315,215,115,516,517";
	    //watch.cards = "411,311,211,412,312,212,112,413,313,213,113";
	    //.cards = "411,311,211,412,312";
	    //watch.cards = "516,517";
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
        string action = socketResponse.P2;
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
        //    16: bool turnToPlay = false,
        //    17: string fingerPrint = "",
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
        watch.gameId = long.Parse(socketResponse.P3);
        watch.gameType = int.Parse(socketResponse.P4);
        watch.deviceType = int.Parse(socketResponse.P5);
        watch.cards = socketResponse.P6;
        watch.landlordCards = socketResponse.P7;
        watch.baseAmount = int.Parse(socketResponse.P8);
        watch.multiples = int.Parse(socketResponse.P9);
        watch.previousNickname = socketResponse.P10;
        watch.previousCardsCount = int.Parse(socketResponse.P11);
        watch.nextNickname = socketResponse.P12;
        watch.nextCardsCount = int.Parse(socketResponse.P13);
        watch.choosingLandlord = bool.Parse(socketResponse.P14);
        watch.landlord = bool.Parse(socketResponse.P15);
        watch.turnToPlay = bool.Parse(socketResponse.P16);
        watch.fingerPrint = socketResponse.P17;

        return watch;
    }

    private void RenderWatch(SeatWatch watch)
    {
        var cardIdArray = watch.cards.Split(Constants.CARDS_SEPERATOR);
        var length = cardIdArray.Length;
        float mid = (float)length / 2;
        GameObject cardObj;
        for (int i = 0; i < length; i++)
        {
            cardObj = GameObject.FindGameObjectWithTag(CardHelper.GetInstance().GetTag(int.Parse(cardIdArray[i])));

            cardObj.transform.position = new Vector3((i - mid) * 50.0f, 0, 0);
            cardObj.transform.localScale = new Vector3(2, 2, 2);
            cardObj.AddComponent<CardAttr>().idx = i;
            cardObj.AddComponent<CardAttr>().inHand = true;
            cardObj.AddComponent<CardAttr>().ready2go = false;

//            base.OnHover()
//            DraggablePanel
//            UISprite
        }
    }


}