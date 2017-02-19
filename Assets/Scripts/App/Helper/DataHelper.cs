using System.Collections.Generic;
using App.Base;
using Google.Protobuf.Collections;
using SimpleSQL;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Helper
{
    public class DataHelper
    {

        private static readonly DataHelper instance = new DataHelper();

        private DataHelper()
        {
        }

        public static DataHelper GetInstance()
        {
            return instance;
        }

        public void CreateTables(SimpleSQLManager dbManager)
        {

            SimpleDataTable dtConfigRow = dbManager.QueryGeneric("SELECT COUNT(*) FROM sqlite_master where type='table' and name='ConfigRow'");
            List<SimpleDataRow> simpleDataConfigRowRows = dtConfigRow.rows;
            if (dtConfigRow == null || simpleDataConfigRowRows == null || simpleDataConfigRowRows.Count == 0)
            {
                dbManager.CreateTable<ConfigRow>();
            }
            SimpleDataTable dtResourceRow = dbManager.QueryGeneric("SELECT COUNT(*) FROM sqlite_master where type='table' and name='ResourceRow'");
            List<SimpleDataRow> simpleDataResourceRowRows = dtResourceRow.rows;
            if (dtResourceRow == null || simpleDataResourceRowRows == null || simpleDataResourceRowRows.Count == 0)
            {
                dbManager.CreateTable<ResourceRow>();
            }
            SimpleDataTable dtSessionRow = dbManager.QueryGeneric("SELECT COUNT(*) FROM sqlite_master where type='table' and name='SessionRow'");
            List<SimpleDataRow> simpleDataSessionRowRows = dtSessionRow.rows;
            if (dtSessionRow == null || simpleDataSessionRowRows == null || simpleDataSessionRowRows.Count == 0)
            {
                dbManager.CreateTable<SessionRow>();
            }

        }

        public ConfigRow LoadConfig(SimpleSQLManager dbManager)
        {
            ConfigRow configRow;
            List<ConfigRow> configRows = dbManager.Query<ConfigRow>("SELECT * FROM ConfigRow WHERE Id = 1");

            if (configRows == null || configRows.Count == 0)
            {
                configRow = SaveDefaultConfig(dbManager);
            }
            else
            {
                configRow = configRows[0];
            }
            return configRow;
        }

        public ConfigRow SaveDefaultConfig(SimpleSQLManager dbManager)
        {
            ConfigRow configRow = new ConfigRow();
            configRow.Id = 1;
            configRow.ResourceVersion = 0;
            // todo get systemLanguage
            SystemLanguage systemLanguage = Application.systemLanguage;
            //configRow.Lan = "zh";
            configRow.Lan = systemLanguage.ToString();
            dbManager.Insert(configRow);
            AppContext.GetInstance().SetLan(configRow.Lan);
            return configRow;
        }

        public void SaveProfile(SimpleSQLManager dbManager, LoginResp response)
        {
            dbManager.BeginTransaction();
            dbManager.Execute("DELETE FROM SessionRow WHERE Id = 1");
            SessionRow row = new SessionRow();
            row.Id = 1;
            row.Token = response.Token;
            row.Mobile = response.Mobile;
            row.Status = response.Status;
            row.NickName = response.NickName;
            dbManager.Insert(row);
            dbManager.Commit();
        }

        public string GetDescByCode(SimpleSQLManager dbManager, string code, string lan)
        {
            string format = string.Format("SELECT Desc FROM ResourceRow WHERE Code = '{0}' AND Lan = '{1}'", code, lan);
            Debug.Log("sql:" + format);
            SimpleDataTable dt = dbManager.QueryGeneric(format);
            List<SimpleDataRow> simpleDataRows = dt.rows;
            if (dt == null || simpleDataRows == null || simpleDataRows.Count == 0)
            {
                return "-";
            }
            return simpleDataRows[0]["Desc"].ToString();
        }

        public void saveResource(SimpleSQLManager dbManager, ResourceResp response)
        {
            ConfigRow configRow = LoadConfig(dbManager);
            if (response.LatestVersion > configRow.ResourceVersion)
            {
                dbManager.BeginTransaction();

                dbManager.Execute("UPDATE ConfigRow SET ResourceVersion = ? WHERE Id = ?", response.LatestVersion, 1);

                Resource r;
                RepeatedField<Resource> list = response.List;
                for (int i = 0; i < list.Count; i++)
                {
                    r = list[i];
                    dbManager.Execute("DELETE FROM ResourceRow WHERE Code = ? AND Lan = ?", r.Code, r.Lan);
                    dbManager.Insert(new ResourceRow
                    {
                        Code = r.Code,
                        Lan = r.Lan,
                        Desc = r.Desc
                    });
                }

                dbManager.Commit();
                Debug.Log(list.Count + " ResourceRow records updated.");
            }
        }

        public string LoadToken(SimpleSQLManager dbManager)
        {
            SimpleDataTable dt = dbManager.QueryGeneric("SELECT Token FROM SessionRow WHERE Id = 1");
            List<SimpleDataRow> simpleDataRows = dt.rows;
            if (dt == null || simpleDataRows == null || simpleDataRows.Count == 0)
            {
                return "-";
            }
            return simpleDataRows[0]["Token"].ToString();
        }

        public void CleanProfile(SimpleSQLManager dbManager)
        {
            dbManager.Execute("DELETE FROM SessionRow WHERE Id = 1");
            SceneManager.LoadScene("login");
        }

    }
}