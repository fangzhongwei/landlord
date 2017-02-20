using System;
using System.Collections;
using System.Collections.Generic;
using App.Base;
using App.Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : HttpMonoBehaviour
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
        SimpleApiResponse.Parser.ParseFrom(data);
        SimpleApiResponse response = null;
        try
        {
            response = SimpleApiResponse.Parser.ParseFrom(data);
        }
        catch (Exception)
        {
            ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
        }

        if (response != null)
        {
            Debug.Log("logout response:" + response);
            switch (response.Code)
            {
                case "0":
                {
                    DataHelper.GetInstance().CleanProfile(dbManager);
                    SceneManager.LoadScene("login");
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
        btnLogout.enabled = true;

    }
}
