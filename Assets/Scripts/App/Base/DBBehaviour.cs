using SimpleSQL;
using UnityEngine;

namespace App.Base
{
    public abstract class DBBehaviour : MonoBehaviour
    {
        protected SimpleSQLManager dbManager;
        protected void AddDBManager()
        {
            gameObject.AddComponent<SimpleSQLManager>();
            dbManager = gameObject.GetComponent<SimpleSQLManager>();
            dbManager.databaseFile = Resources.Load(Constants.RESOURCES_DB_FILE_PATH) as TextAsset;
            dbManager.overwriteIfExists = false;
        }
    }
}