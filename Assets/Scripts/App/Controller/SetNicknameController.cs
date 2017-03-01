
using System;
using System.IO;
using App.Base;
using ProtoBuf;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetNicknameController : HttpMonoBehaviour
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
            nickName = inputNicknameValue
        };

        HttpPost(Constants.API_UPDATE_NICKNAME, ProtoHelper.Proto2Bytes(req));
    }

    public override void Callback(byte[] data)
    {
        SimpleApiResponse response = null;

        try
        {
            response = Serializer.Deserialize<SimpleApiResponse>(new MemoryStream(data));
        }
        catch (Exception)
        {
            ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
        }

        if (response != null)
        {
            switch (response.code)
            {
                case "0":
                {
                    SceneManager.LoadScene("home");
                    break;
                }
                default:
                {
                    ShowMessage(response.code);
                    break;
                }
            }
        }
    }

    public override void HttpFinished()
    {
    }
}
