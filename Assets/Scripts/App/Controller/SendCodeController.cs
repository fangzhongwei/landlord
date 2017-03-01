using System;
using System.IO;
using App.Base;
using App.Helper;
using ProtoBuf;
using UnityEngine;

public class SendCodeController : HttpMonoBehaviour
{
    private UIInput inputMobile;
    private UIButton buttonSend;
    protected UILabel labelSend;
    private bool resend;
    private int lastChannel;

    // Use this for initialization
    void Start()
    {
        AddDBManager();
        FindBaseUis();
        resend = false;
        lastChannel = 0;
        inputMobile = GameObject.FindWithTag("mobile").GetComponent<UIInput>();
        inputMobile.value = "17381906228";
        inputMobile.value = "17381906228";
        buttonSend = GameObject.FindWithTag("send").GetComponent<UIButton>();
        labelSend = GameObject.FindWithTag("sendBtnLabel").GetComponent<UILabel>();
    }

    private float timer = 1.0f;
    private bool timing;
    private int seconds;

    // Update is called once per frame
    void LateUpdate() {
        if (timing)
        {
            buttonSend.enabled = false;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                seconds -= 1;
                labelSend.text = string.Format("重新发送({0}秒)", seconds);
                if (seconds == 0)
                {
                    timing = false;
                    labelSend.text ="获取验证码";
                    buttonSend.enabled = true;
                }
                timer = 1.0f;
            }
        }
    }

    public void OnbtlClick()
    {
        CleanMessage();
        buttonSend.enabled = false;

        string mobile = inputMobile.value.Trim();

        if (mobile == null || "".Equals(mobile))
        {
            ShowMessage(ErrorCode.EC_UC_NO_MOBILE);
            buttonSend.enabled = true;
            return;
        }

        if (!RegexHelper.isMobile(mobile))
        {
            ShowMessage(ErrorCode.EC_UC_INVALID_MOBILE);
            buttonSend.enabled = true;
            return;
        }

        string s = resend ? "1" : "0";
//            SendLoginVerificationCodeReq req = new SendLoginVerificationCodeReq
//            {
//                DeviceType = DeviceHelper.getDeviceType(),
//                FingerPrint = SystemInfo.deviceUniqueIdentifier,
//                Mobile = mobile,
//                Resend = s,
//                LastChannel = lastChannel
//            };

        SendCode sc = new SendCode()
        {
            deviceType = DeviceHelper.getDeviceType(),
            fingerPrint = SystemInfo.deviceUniqueIdentifier,
            mobile = mobile,
            resend = s,
            lastChannel = lastChannel
        };

        HttpPost(Constants.API_ID_SEND_CODE, ProtoHelper.Proto2Bytes(sc));
    }

    public override void Callback(byte[] data) {
        SendCodeResp response = null;
        try
        {
            response = Serializer.Deserialize<SendCodeResp>(new MemoryStream(data));
        }
        catch (Exception)
        {
            Debug.LogError("SendCodeResp parse error");
            ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
        }

        if (response != null)
        {
            switch (response.code)
            {
                case "0":
                    {
                        ShowMessage(ErrorCode.MSG_CODE_SENDED);
                        resend = true;
                        lastChannel = response.channel;
                        seconds = 60;
                        timing = true;
                        break;
                    }
                default:
                    {
                        ShowMessage(response.code);
                        buttonSend.enabled = true;
                        break;
                    }
            }
        }
    }

    public override void HttpFinished()
    {
        buttonSend.enabled = true;
    }
}
