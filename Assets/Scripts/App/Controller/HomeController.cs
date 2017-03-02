using System;
using System.IO;
using App.Base;
using App.Helper;
using ProtoBuf;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : HttpMonoBehaviour
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
            clientId = Constants.CLIENT_ID,
            version = Constants.VERSION,
            deviceType = DeviceHelper.getDeviceType(),
            fingerPrint = SystemInfo.deviceUniqueIdentifier,
            token = token
        };

        dataType = 1;
        HttpPost(Constants.API_ID_LOGIN_BY_TOKEN, ProtoHelper.Proto2Bytes(req));
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
            case 3:
                CheckGameStatusCallback(data);
                break;
        }
    }

    private void LoginByTokenCallback(byte[] data)
    {
        dataType = 0;
        LoginResp response = null;
        try
        {
            response = Serializer.Deserialize<LoginResp>(new MemoryStream(data));
        }
        catch (Exception)
        {
            Debug.LogError("LoginResponse parse error");
            ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
        }

        if (response != null)
        {
            switch (response.code)
            {
                case "0":
                {
                    DataHelper.GetInstance().SaveProfile(dbManager, response);
                    if (response.status != 99 && "".Equals(response.nickName))
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
                    Debug.LogError("LoginByTokenCallback:" + response.code);
                    ShowMessage(response.code);
                    break;
                }
            }
        }
    }

    private void QueryDiamonAmount()
    {
        HttpPost(Constants.API_QUERY_DIAMOND_AMOUNT, null);
    }

    private void CheckGameStatus()
    {
        HttpPost(Constants.API_CHECK_GAME_STATUS, null);
    }

    private void QueryDiamonAmountCallback(byte[] data)
    {
        dataType = 0;
        SimpleApiResponse response = null;
        try
        {
            response = Serializer.Deserialize<SimpleApiResponse>(new MemoryStream(data));
        }
        catch (Exception)
        {
            Debug.LogError("SimpleApiResponse parse error");
            ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
        }

        if (response != null)
        {
            switch (response.code)
            {
                case "0":
                {
                    string diamondAmount = response.ext1;
                    ShowBalance(DataHelper.GetInstance().LoadSession(dbManager).Mobile + "," + diamondAmount);
                    dataType = 3;
                    CheckGameStatus();
                    break;
                }
                default:
                {
                    Debug.LogError("QueryDiamonAmountCallback:" + response.code);
                    ShowMessage(response.code);
                    break;
                }
            }
        }
    }

    private void CheckGameStatusCallback(byte[] data)
    {
        dataType = 0;
        CheckGameStatusResp response = null;
        try
        {
            response = Serializer.Deserialize<CheckGameStatusResp>(new MemoryStream(data));
        }
        catch (Exception)
        {
            Debug.LogError("SimpleApiResponse parse error");
            ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
        }

        if (response != null)
        {
            switch (response.code)
            {
                case "0":
                {
                    if (response.reconnect)
                    {
                        SceneManager.LoadScene("play");
                    }
                    break;
                }
                default:
                {
                    Debug.LogError("CheckGameStatusCallback:" + response.code);
                    ShowMessage(response.code);
                    break;
                }
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("play");
    }

    private void ShowBalance(string diamondAmount)
    {
        labelDiamondAmount.text = diamondAmount;
    }

    public override void HttpFinished()
    {
    }
}
