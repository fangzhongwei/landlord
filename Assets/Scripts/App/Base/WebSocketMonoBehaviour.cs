using System;
using System.Threading;
using App.Helper;
using BestHTTP.WebSocket;
using ConsoleApplication.Helper;
using SimpleSQL;
using UnityEngine;

namespace App.Base
{
    public abstract class WebSocketMonoBehaviour : MonoBehaviour
    {
        private WebSocket webSocket;
        protected UILabel labelMessage;

        protected SimpleSQLManager dbManager;
        protected void AddDBManager()
        {
            gameObject.AddComponent<SimpleSQLManager>();
            dbManager = gameObject.GetComponent<SimpleSQLManager>();
            dbManager.databaseFile = Resources.Load(Constants.RESOURCES_DB_FILE_PATH) as TextAsset;
            dbManager.overwriteIfExists = false;
        }

        // Use this for initialization
        protected void FindBaseUis()
        {
            labelMessage = GameObject.FindWithTag("message").GetComponent<UILabel>();
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

        protected void StartWebSocket(string uri)
        {
            webSocket = new WebSocket(new Uri(uri));
            webSocket.OnOpen += OnWebSocketOpen;
            webSocket.OnMessage += OnMessageReceived;
            webSocket.OnBinary += OnBinaryMessageReceived;
            webSocket.OnClosed += OnWebSocketClosed;
            webSocket.OnError += OnError;
            webSocket.OnErrorDesc += OnErrorDesc;

            webSocket.Open();
        }

        protected void Close()
        {
            try
            {
                webSocket.Close();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        protected void SendString(string str)
        {
            webSocket.Send(str);
        }

        protected void Send(byte[] buffer)
        {
            //fill up the buffer with data
            webSocket.Send(buffer);
        }

        private void OnWebSocketOpen(WebSocket webSocket)
        {
            Debug.Log("WebSocket Open!");
        }

        private void OnMessageReceived(WebSocket webSocket, string message)
        {
            Debug.Log("Text Message received from server: " + message);
            labelMessage.text = message;
        }

        private void OnBinaryMessageReceived(WebSocket webSocket, byte[] buffer)
        {
            Debug.Log("Binary Message received from server. Length: " + buffer.Length);
            labelMessage.text = "Length:" + buffer.Length;
            SocketResponse socketResponse;
            try
            {
                buffer = GZipHelper.Decompress(DESHelper.DecodeBytes(buffer, AppContext.GetInstance().getDesKey()));
                socketResponse = SocketResponse.Parser.ParseFrom(buffer);
            }
            catch (Exception)
            {
                ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
                DataHelper.GetInstance().CleanProfile(dbManager);
                return;
            }

            if (socketResponse != null)
            {
                String code = socketResponse.P1;
                if (!"0".Equals(code))
                {
                    ShowMessage(code);
                }
                else
                {
                    HandleSocketResponse(socketResponse);
                }
            }
        }

        abstract public void HandleSocketResponse(SocketResponse socketResponse);

        private void OnWebSocketClosed(WebSocket webSocket, UInt16 code, string message)
        {
            Debug.Log("WebSocket Closed!");
            Thread.Sleep(1000);

        }

        private void OnError(WebSocket ws, Exception ex)
        {
            string errorMsg = string.Empty;
            if (ws.InternalRequest.Response != null)
                errorMsg = string.Format("Status Code from Server: {0} and Message: {1}",
                    ws.InternalRequest.Response.StatusCode,
                    ws.InternalRequest.Response.Message);

            Debug.Log("An error occured: " + (ex != null ? ex.Message : "Unknown: " + errorMsg));
        }

        void OnErrorDesc(WebSocket ws, string error)
        {
            Debug.Log("Error: " + error);
        }

        //Ping
        //Pong
        //Streaming OnIncompleteFrame

    }
}