using App.Helper;
using SimpleSQL;
using UnityEngine;

namespace App.Base
{
    public abstract class BaseMonoBehaviour : MonoBehaviour
    {
        protected SimpleSQLManager dbManager;
        protected UILabel labelMessage;

        protected void AddDBManager()
        {
            dbManager = GameObject.FindGameObjectWithTag("appdbmanager").GetComponent<SimpleSQLManager>();
        }

        // Use this for initialization
        protected void FindBaseUis()
        {
            labelMessage = GameObject.FindWithTag("message").GetComponent<UILabel>();
        }

        protected string LocalToken()
        {
            return DataHelper.GetInstance().LoadToken(dbManager);
        }

        protected void CleanMessage()
        {
            labelMessage.text = "";
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
    }
}


