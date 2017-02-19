using System;
using System.Collections;
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

        // Update is called once per frame
        void Update()
        {
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
                Resend = resend,
                LastChannel = lastChannel
            };

            Debug.Log("SendLoginVerificationCodeReq : " + req);

            HttpPost(Constants.COMMON_DISPATCH_URL, GUIDHelper.generate(), Constants.DEFAULT_TOKEN,
                Constants.API_ID_SEND_CODE, req.ToByteArray());
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
                            StartCoroutine(Timer());
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

        IEnumerator Timer()
        {
            int remainingSeconds = 60;
            labelSend.text = string.Format("重新发送(${0}秒)", remainingSeconds);
            while (true) {
                yield return new WaitForSeconds(1.0f);
                remainingSeconds -= 1;
                if (remainingSeconds <= 0)
                {
                    labelSend.text = "获取验证码";
                    buttonSend.enabled = true;
                    CleanMessage();
                    break;
                }
                labelSend.text = string.Format("重新发送(${0}秒)", remainingSeconds);
            }
        }

        public override void HttpFinished()
        {
            buttonSend.enabled = true;
        }
    }
}