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
	    GameObject.FindWithTag("message").GetComponent<UILabel>().text = "Index start.";
	    AddDBManager();
	    GameObject.FindWithTag("message").GetComponent<UILabel>().text = "AddDBManager:" + dbManager;
	    try
	    {
	        DataHelper.GetInstance().CreateTables(dbManager);
	    }
	    catch (Exception e)
	    {
	        GameObject.FindWithTag("message").GetComponent<UILabel>().text = "Ex:" + e.StackTrace;
	        throw;
	    }
	    GameObject.FindWithTag("message").GetComponent<UILabel>().text = "CreateTables";
	    FindBaseUis();
	    GameObject.FindWithTag("message").GetComponent<UILabel>().text = "FindBaseUis";
	    ConfigRow loadConfig = DataHelper.GetInstance().LoadConfig(dbManager);
	    GameObject.FindWithTag("message").GetComponent<UILabel>().text = "loadConfig:" + loadConfig;

	    if (loadConfig.ResourceVersion == 0)
	    {
	        SimpleReq req = new SimpleReq
	        {
	            Param0 = loadConfig.Lan
	        };
	        HttpPost(Constants.API_LOAD_ALL_RESOURCES, req.ToByteArray());
	        labelMessage.text = "API_LOAD_ALL_RESOURCES";
	    }
	    else
	    {
	        PullResourceReq req = new PullResourceReq
	        {
	            Version = loadConfig.ResourceVersion,
	            Lan = loadConfig.Lan
	        };
	        HttpPost(Constants.API_PULL_RESOURCES, req.ToByteArray());
	        labelMessage.text = "API_PULL_RESOURCES";
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
