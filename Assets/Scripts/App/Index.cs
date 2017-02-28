using System;
using App.Base;
using App.Helper;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Index : HttpMonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	    AddDBManager();
	    DataHelper.GetInstance().CreateTables(dbManager);
	    FindBaseUis();
        PullResource();
    }

    void PullResource()
    {
        ConfigRow loadConfig = DataHelper.GetInstance().LoadConfig(dbManager);
        if (loadConfig.ResourceVersion == 0)
        {

            SimpleReq req = new SimpleReq
            {
                Param0 = loadConfig.Lan
            };
            HttpPost(Constants.API_LOAD_ALL_RESOURCES, req.ToByteArray());
        }
        else
        {
            PullResourceReq req = new PullResourceReq
            {
                Version = loadConfig.ResourceVersion,
                Lan = loadConfig.Lan
            };
            HttpPost(Constants.API_PULL_RESOURCES, req.ToByteArray());
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Callback(byte[] data) {
        ResourceResp response = null;
        try
        {
            response = ResourceResp.Parser.ParseFrom(data);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
        }

        if (response != null)
        {
            switch (response.Code)
            {
                case "0":
                {
                    DataHelper.GetInstance().saveResource(dbManager, response);
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

    public override void HttpFinished() {

    }
}
