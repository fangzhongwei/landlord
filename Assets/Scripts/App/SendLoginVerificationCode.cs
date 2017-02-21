using System;
using App.Base;
using App.Helper;
using Google.Protobuf;
using UnityEngine;

namespace App
{
    public class SendLoginVerificationCode : HttpMonoBehaviour
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
            inputMobile.value = "15881126718";
            buttonSend = GameObject.FindWithTag("send").GetComponent<UIButton>();
            labelSend = GameObject.FindWithTag("sendBtnLabel").GetComponent<UILabel>();
        }

        private float timer = 1.0f;
        private bool timing;
        private int seconds;

        // Update is called once per frame
        void Update() {
            if (timing)
            {
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

            string mobile = inputMobile.value;

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

            SendLoginVerificationCodeReq req = new SendLoginVerificationCodeReq
            {
                DeviceType = DeviceHelper.getDeviceType(),
                FingerPrint = SystemInfo.deviceUniqueIdentifier,
                Mobile = mobile,
                Resend = resend ? "1" : "0",
                LastChannel = lastChannel
            };

            Debug.Log("SendLoginVerificationCodeReq : " + req);

            HttpPost(Constants.API_ID_SEND_CODE, req.ToByteArray());
        }

        public override void Callback(byte[] data) { 
            SendLoginVerificationCodeResp response = null;
            try
            {
                response = SendLoginVerificationCodeResp.Parser.ParseFrom(data);
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
                            ShowMessage(ErrorCode.MSG_CODE_SENDED);
                            resend = true;
                            lastChannel = response.Channel;
                            seconds = 60;
                            timing = true;
                            break;
                        }
                    default:
                        {
                            ShowMessage(response.Code);
                            buttonSend.enabled = true;
                            break;
                        }
                }
            }
        }

        public override void HttpFinished()
        {
        }
    }
}