using System;
using System.Collections.Generic;
using App.Helper;
using ConsoleApplication.Helper;
using UnityEngine;
using BestHTTP;
using SimpleSQL;

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

        protected void ShowMessage(string code)
        {
            if (code.Equals(ErrorCode.EC_SSO_SESSION_EXPIRED))
            {
                DataHelper.GetInstance().CleanProfile(dbManager);
                return;
            }
            if (code.Equals(ErrorCode.EC_SSO_SESSION_REPELLED))
            {
                //SceneManager.LoadScene("login");
                return;
            }
            labelMessage.text = DataHelper.GetInstance().GetDescByCode(dbManager, code, AppContext.GetInstance().GetLan());
        }

        public void HttpPost(string uri, string traceId, string token, int actionId, byte[] data)
        {
            Debug.Log(string.Format("http post:[uri:{0},traceId:{1},token:{2},actionId:{3},dataLenght:{4}]", uri, traceId, token, actionId, data.Length));
            byte[] encodeBytes = new byte[0];
            if (data != null && data.Length > 0)
            {
                encodeBytes = DESHelper.EncodeBytes(GZipHelper.compress(data), AppContext.GetInstance().getDesKey());
            }
            HTTPRequest request = new HTTPRequest(new Uri(uri), HTTPMethods.Post, OnRequestFinished);
            request.SetHeader("TI", traceId);
            request.SetHeader("AI", actionId.ToString());
            request.SetHeader("TK", token);
            request.SetHeader("X-Real-Ip", "127.0.0.1");
            request.ConnectTimeout = TimeSpan.FromSeconds(30);
            request.RawData = encodeBytes;
            request.Send();
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
                        Debug.Log("isNormal:" + isNormal);
                        if (!isNormal)
                        {
                            SimpleApiResponse response = null;
                            try
                            {
                                response = SimpleApiResponse.Parser.ParseFrom(protoBytes);
                                Debug.Log("error response:" + response);
                            }
                            catch (Exception)
                            {
                                ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
                            }
                            if (response != null)
                            {
                                ShowMessage(response.Code);
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