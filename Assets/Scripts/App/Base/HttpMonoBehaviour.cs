using System;
using System.IO;
using App.Helper;
using BestHTTP;
using ConsoleApplication.Helper;
using ProtoBuf;
using SimpleSQL;
using UnityEngine;

namespace App.Base
{
    public abstract class HttpMonoBehaviour : MonoBehaviour
    {
        protected UILabel labelMessage;

        protected SimpleSQLManager dbManager;
        protected void AddDBManager()
        {
            dbManager = GameObject.FindGameObjectWithTag("appdbmanager").GetComponent<SimpleSQLManager>();
        }

        // Use this for initialization
        protected void FindBaseUis()
        {
            labelMessage = GameObject.FindWithTag("message").GetComponent<UILabel>();
        }

        protected void CleanMessage()
        {
            labelMessage.text = "";
        }

        protected string LocalToken()
        {
            return DataHelper.GetInstance().LoadToken(dbManager);
        }

        protected void ShowMessage(string code)
        {
            if (ErrorCode.EC_SSO_SESSION_EXPIRED.Equals(code) || ErrorCode.EC_SSO_TOKEN_DEVICE_MISMATCH.Equals(code))
            {
                DataHelper.GetInstance().CleanProfile(dbManager);
                return;
            }
            if (code.Equals(ErrorCode.EC_SSO_SESSION_REPELLED))
            {
                //SceneManager.LoadScene("login");
                return;
            }
            labelMessage.text = DataHelper.GetInstance().GetDescByCode(dbManager, code, DataHelper.GetInstance().LoadLan(dbManager));
        }

        public void HttpPost(int actionId, byte[] data)
        {
            //Debug.Log(string.Format("http post:[actionId:{0},dataLenght:{1}]", actionId, data == null ? 0 : data.Length));
            byte[] encodeBytes = new byte[0];
            if (data != null && data.Length > 0)
            {
                encodeBytes = DESHelper.EncodeBytes(GZipHelper.compress(data), AppContext.GetInstance().getDesKey());
            }

            HTTPRequest request = new HTTPRequest(new Uri(Constants.COMMON_DISPATCH_URL), HTTPMethods.Post, OnRequestFinished);
            request.SetHeader("TI", GUIDHelper.generate());
            request.SetHeader("AI", actionId.ToString());
            request.SetHeader("TK", ignoreSession(actionId) ? Constants.DEFAULT_TOKEN: DataHelper.GetInstance().LoadToken(dbManager));
            request.SetHeader("FP", SystemInfo.deviceUniqueIdentifier);
            request.ConnectTimeout = TimeSpan.FromSeconds(30);
            request.RawData = encodeBytes;
            request.Send();
        }

        private bool ignoreSession(int apiId)
        {
            return apiId == Constants.API_ID_LOGIN || Constants.API_ID_SEND_CODE == apiId ||
                   Constants.API_LOAD_ALL_RESOURCES == apiId || Constants.API_PULL_RESOURCES == apiId;
        }

        void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            HttpFinished();
            switch (req.State)
            {
                // The request finished without any problem.
                case HTTPRequestStates.Finished:
                    if (resp.StatusCode == 200)
                    {
                        bool isNormal = false;
                        if (resp.Headers.ContainsKey("normal"))
                        {
                            isNormal = "true".Equals(resp.Headers["normal"][0]);
                        }
                        byte[] protoBytes = GZipHelper.Decompress(DESHelper.DecodeBytes(resp.Data, AppContext.GetInstance().getDesKey()));
                        //Debug.Log("isNormal:" + isNormal);
                        if (!isNormal)
                        {
                            SimpleApiResponse response = null;
                            try
                            {
                                response = Serializer.Deserialize<SimpleApiResponse>(new MemoryStream(protoBytes));
                                Debug.Log("error response:" + response);
                            }
                            catch (Exception)
                            {
                                ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
                            }
                            if (response != null)
                            {
                                ShowMessage(response.code);
                            }
                        }
                        else
                        {
                            Callback(protoBytes);
                        }
                    }
                    else
                    {
                        ShowMessage(ErrorCode.EC_SERVER_ERROR);
                    }
                    break;

                // The request finished with an unexpected error.
                // The request's Exception property may contain more information about the error.
                case HTTPRequestStates.Error:
                    Debug.LogError("Request Finished with Error! " +
                                   (req.Exception != null ?
                                       (req.Exception.Message + "\n" + req.Exception.StackTrace) :
                                       "No Exception"));
                    ShowMessage(ErrorCode.EC_NETWORK_UNREACHED);
                    break;

                // The request aborted, initiated by the user.
                case HTTPRequestStates.Aborted:
                    Debug.LogWarning("Request Aborted!");
                    ShowMessage(ErrorCode.EC_NETWORK_UNREACHED);
                    break;

                // Ceonnecting to the server timed out.
                case HTTPRequestStates.ConnectionTimedOut:
                    Debug.LogError("Connection Timed Out!");
                    ShowMessage(ErrorCode.EC_NETWORK_TIMEOUT);
                    break;

                // The request didn't finished in the given time.
                case HTTPRequestStates.TimedOut:
                    Debug.LogError("Processing the request Timed Out!");
                    ShowMessage(ErrorCode.EC_NETWORK_TIMEOUT);
                    break;
                default:
                    Debug.LogError("Connection Error!");
                    ShowMessage(ErrorCode.EC_NETWORK_TIMEOUT);
                    break;
            }
        }

        abstract public void Callback(byte[] data);
        abstract public void HttpFinished();
    }
}