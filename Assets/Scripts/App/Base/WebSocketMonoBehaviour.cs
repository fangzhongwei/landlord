using System;
using System.IO;
using System.Threading;
using App.Helper;
using BestHTTP.WebSocket;
using ConsoleApplication.Helper;
using ProtoBuf;
using UnityEngine;

namespace App.Base
{
    public abstract class WebSocketMonoBehaviour : BaseMonoBehaviour
    {
        private WebSocket webSocket;

        protected  void StartWebSocket(string uri)
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

        public void SendBytes(byte[] buffer)
        {
            webSocket.Send(DESHelper.EncodeBytes(GZipHelper.compress(buffer), AppContext.GetInstance().getDesKey()));
        }

        private void OnWebSocketOpen(WebSocket webSocket)
        {
            Debug.Log("WebSocket open! now login.");
            SocketRequest req = new SocketRequest();
            req.p1 = GUIDHelper.generate();
            req.p2 = "login";

            req.p3 = DataHelper.GetInstance().LoadToken(dbManager);
            req.p4 = SystemInfo.deviceUniqueIdentifier;

            webSocket.Send(DESHelper.EncodeBytes(GZipHelper.compress(ProtoHelper.Proto2Bytes(req)), AppContext.GetInstance().getDesKey()));
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
                socketResponse = Serializer.Deserialize<SocketResponse>(new MemoryStream(buffer));
            }
            catch (Exception)
            {
                Debug.LogError("SocketResponse parse error");
                ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
                DataHelper.GetInstance().CleanProfile(dbManager);
                return;
            }

            if (socketResponse != null)
            {
                String code = socketResponse.p1;
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
    }
}