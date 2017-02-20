using System;
using App.Base;
using App.Helper;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App
{
    public class Login : HttpMonoBehaviour
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
            string mobile = inputMobile.value;
            string code = inputCode.value;

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
                ClientId = Constants.CLIENT_ID,
                DeviceType = DeviceHelper.getDeviceType(),
                FingerPrint = SystemInfo.deviceUniqueIdentifier,
                Mobile = mobile,
                VerificationCode = code,
                Version = Constants.VERSION
            };

            HttpPost(Constants.API_ID_LOGIN, req.ToByteArray());
        }

        public override void Callback(byte[] data)
        {
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

        public override void HttpFinished()
        {
            buttonLogin.enabled = true;
        }
    }
}