using System;
using System.IO;
using App.Base;
using App.Helper;
using ProtoBuf;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : HttpMonoBehaviour
{
    private UIInput inputMobile;
    private UIInput inputCode;
    private UIButton buttonLogin;

    // Use this for initialization
    void Start()
    {
        FindBaseUis();
        AddDBManager();
        inputMobile = GameObject.FindWithTag("mobile").GetComponent<UIInput>();
        inputCode = GameObject.FindWithTag("code").GetComponent<UIInput>();
        buttonLogin = GameObject.FindWithTag("login").GetComponent<UIButton>();
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void OnbtlClick()
    {
        CleanMessage();
        buttonLogin.enabled = false;
        string mobile = inputMobile.value.Trim();
        string code = inputCode.value.Trim();

        if (mobile == null || "".Equals(mobile))
        {
            ShowMessage(ErrorCode.EC_UC_NO_MOBILE);
            buttonLogin.enabled = true;
            return;
        }

        if (!RegexHelper.isMobile(mobile))
        {
            ShowMessage(ErrorCode.EC_UC_INVALID_MOBILE);
            buttonLogin.enabled = true;
            return;
        }

        if (code == null || "".Equals(code))
        {
            ShowMessage(ErrorCode.EC_UC_NO_CODE);
            buttonLogin.enabled = true;
            return;
        }

        if (!RegexHelper.isValidCode(code))
        {
            ShowMessage(ErrorCode.EC_UC_INVALID_CODE);
            buttonLogin.enabled = true;
            return;
        }

        LoginReq req = new LoginReq
        {
            clientId = Constants.CLIENT_ID,
            deviceType = DeviceHelper.getDeviceType(),
            fingerPrint = SystemInfo.deviceUniqueIdentifier,
            mobile = mobile,
            verificationCode = code,
            version = Constants.VERSION
        };

        HttpPost(Constants.API_ID_LOGIN, ProtoHelper.Proto2Bytes(req));
    }

    public override void Callback(byte[] data)
    {
        LoginResp response = null;
        try
        {
            response = Serializer.Deserialize<LoginResp>(new MemoryStream(data));
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
                        DataHelper.GetInstance().SaveProfile(dbManager, response);
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
        buttonLogin.enabled = true;
    }
}
