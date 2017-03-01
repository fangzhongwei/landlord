using System;
using System.IO;
using App.Base;
using App.Helper;
using ProtoBuf;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutController : HttpMonoBehaviour
{
    private UIButton btnLogout;

	// Use this for initialization
	void Start () {
		AddDBManager();
	    FindBaseUis();
	    btnLogout = GameObject.FindGameObjectWithTag("logout").GetComponent<UIButton>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        btnLogout.enabled = false;
        HttpPost(Constants.API_ID_LOGOUT, null);
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
            Debug.Log("logout response:" + response);
            switch (response.code)
            {
                case "0":
                {
                    DataHelper.GetInstance().CleanProfile(dbManager);
                    SceneManager.LoadScene("login");
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
        btnLogout.enabled = true;

    }
}
