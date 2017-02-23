﻿using System;
using App.Base;
using App.Helper;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : HttpMonoBehaviour
{
    private int dataType;
    private UILabel labelDiamondAmount;

	// Use this for initialization
	void Start ()
	{
	    AddDBManager();
	    FindBaseUis();
	    labelDiamondAmount = GameObject.FindGameObjectWithTag("diamondAmount").GetComponent<UILabel>();
	    LoginByToken();
	}

    void LoginByToken()
    {
        string token = LocalToken();
        if ("-".Equals(token))
        {
            SceneManager.LoadScene("login");
            return;
        }

        LoginByTokenReq req = new LoginByTokenReq
        {
            ClientId = Constants.CLIENT_ID,
            Version = Constants.VERSION,
            DeviceType = DeviceHelper.getDeviceType(),
            FingerPrint = SystemInfo.deviceUniqueIdentifier,
            Token = token
        };

        dataType = 1;
        HttpPost(Constants.API_ID_LOGIN_BY_TOKEN, req.ToByteArray());
    }

    // Update is called once per frame
	void Update () {
		
	}

    public  override void Callback(byte[] data)
    {
        switch (dataType)
        {
            case 1:
                LoginByTokenCallback(data);
                break;
            case 2:
                QueryDiamonAmountCallback(data);
                break;
        }
    }

    private void LoginByTokenCallback(byte[] data)
    {
        dataType = 0;
        LoginResp response = null;
        try
        {
            response = LoginResp.Parser.ParseFrom(data);
        }
        catch (Exception)
        {
            ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
        }

        if (response != null)
        {
            switch (response.Code)
            {
                case "0":
                {
                    DataHelper.GetInstance().SaveProfile(dbManager, response);
                    if (response.Status != 99 && "".Equals(response.NickName))
                    {
                        SceneManager.LoadScene("nickname");
                        break;
                    }
                    dataType = 2;
                    QueryDiamonAmount();
                    break;
                }
                default:
                {
                    ShowMessage(response.Code);
                    break;
                }
            }
        }
    }

    private void QueryDiamonAmount()
    {
        HttpPost(Constants.API_QUERY_DIAMOND_AMOUNT, null);
    }

    private void QueryDiamonAmountCallback(byte[] data)
    {
        dataType = 0;
        SimpleApiResponse response = null;
        try
        {
            response = SimpleApiResponse.Parser.ParseFrom(data);
        }
        catch (Exception)
        {
            ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
        }

        if (response != null)
        {
            switch (response.Code)
            {
                case "0":
                {
                    string diamondAmount = response.Ext1;
                    ShowBalance(diamondAmount);
                    break;
                }
                default:
                {
                    ShowMessage(response.Code);
                    break;
                }
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("game");
    }

    private void ShowBalance(string diamondAmount)
    {
        labelDiamondAmount.text = diamondAmount;
    }

    public override void HttpFinished()
    {
    }
}
