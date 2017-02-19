
using System;
using App.Base;
using App.Helper;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetNickname : HttpMonoBehaviour
{
    private UIInput inputNickname;
    private UIButton buttonSubmit;

	// Use this for initialization
	void Start () {
	    AddDBManager();
		FindBaseUis();
	    inputNickname = GameObject.FindGameObjectWithTag("inputNickname").GetComponent<UIInput>();
	    buttonSubmit = GameObject.FindGameObjectWithTag("submitNickname").GetComponent<UIButton>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        buttonSubmit.enabled = false;
        string inputNicknameValue = inputNickname.value;
        if (inputNicknameValue == null || "".Equals(inputNicknameValue))
        {
            ShowMessage(ErrorCode.EC_UC_NO_NICKNAME);
            buttonSubmit.enabled = true;
            return;
        }
        if (inputNicknameValue.Length > 16)
        {
            ShowMessage(ErrorCode.EC_UC_NICKNAME_TOO_LONG);
            buttonSubmit.enabled = true;
            return;
        }

        UpdateNickNameReq req = new UpdateNickNameReq
        {
            NickName = inputNicknameValue
        };

        HttpPost(Constants.COMMON_DISPATCH_URL, GUIDHelper.generate(), DataHelper.GetInstance().LoadToken(dbManager),
            Constants.API_UPDATE_NICKNAME, req.ToByteArray());
    }

    public override void Callback(byte[] data)
    {
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
                    SceneManager.LoadScene("home");
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

    private void OnDestroy()
    {
        if (dbManager != null)
        {
            dbManager.Close();
            dbManager = null;
        }
    }

    public override void HttpFinished()
    {
    }
}
