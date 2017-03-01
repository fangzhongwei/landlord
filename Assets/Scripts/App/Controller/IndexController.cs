using System;
using System.IO;
using App.Base;
using App.Helper;
using ProtoBuf;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IndexController : HttpMonoBehaviour
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
                param0 = loadConfig.Lan
            };
            HttpPost(Constants.API_LOAD_ALL_RESOURCES, ProtoHelper.Proto2Bytes(req));
        }
        else
        {
            PullResourceReq req = new PullResourceReq
            {
                version = loadConfig.ResourceVersion,
                lan = loadConfig.Lan
            };
            HttpPost(Constants.API_PULL_RESOURCES, ProtoHelper.Proto2Bytes(req));
        }
    }

    public override void Callback(byte[] data) {
        ResourceResp response = null;
        try
        {
            response = Serializer.Deserialize<ResourceResp>(new MemoryStream(data));
        }
        catch (Exception)
        {
            Debug.LogError("ResourceResp parse error");
            ShowMessage(ErrorCode.EC_PARSE_DATA_ERROR);
        }

        if (response != null)
        {
            switch (response.code)
            {
                case "0":
                {
                    DataHelper.GetInstance().saveResource(dbManager, response);
                    SceneManager.LoadScene("home");
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

    public override void HttpFinished() {

    }
}
