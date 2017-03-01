using System.Collections.Generic;
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
            SimpleDataTable dtConfigRow = dbManager.QueryGeneric("SELECT COUNT(*) CT FROM sqlite_master where type='table' and name='ConfigRow'");
            List<SimpleDataRow> simpleDataConfigRowRows = dtConfigRow.rows;

            if (int.Parse(simpleDataConfigRowRows[0]["CT"].ToString()) == 0)
            {
                dbManager.CreateTable<ConfigRow>();
                Debug.Log("create table ConfigRow.");
            }
            SimpleDataTable dtResourceRow = dbManager.QueryGeneric("SELECT COUNT(*) CT FROM sqlite_master where type='table' and name='ResourceRow'");
            List<SimpleDataRow> simpleDataResourceRowRows = dtResourceRow.rows;
            if (int.Parse(simpleDataResourceRowRows[0]["CT"].ToString()) == 0)
            {
                dbManager.CreateTable<ResourceRow>();
                Debug.Log("create table ResourceRow.");
            }
            SimpleDataTable dtSessionRow = dbManager.QueryGeneric("SELECT COUNT(*) CT FROM sqlite_master where type='table' and name='SessionRow'");
            List<SimpleDataRow> simpleDataSessionRowRows = dtSessionRow.rows;
            if (int.Parse(simpleDataSessionRowRows[0]["CT"].ToString()) == 0)
            {
                dbManager.CreateTable<SessionRow>();
                Debug.Log("create table SessionRow.");
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
            //SystemLanguage systemLanguage = Application.systemLanguage;
            //configRow.Lan = systemLanguage.ToString();
            configRow.Lan = "Chinese";
            dbManager.Insert(configRow);
            return configRow;
        }

        public void SaveProfile(SimpleSQLManager dbManager, LoginResp response)
        {
            dbManager.BeginTransaction();
            dbManager.Execute("DELETE FROM SessionRow WHERE Id = 1");
            SessionRow row = new SessionRow();
            row.Id = 1;
            row.Token = response.token;
            row.Mobile = response.mobile;
            row.Status = response.status;
            row.NickName = response.nickName;
            dbManager.Insert(row);
            dbManager.Commit();
        }

        public string GetDescByCode(SimpleSQLManager dbManager, string code, string lan)
        {
            SimpleDataTable dt = dbManager.QueryGeneric(string.Format("SELECT Desc FROM ResourceRow WHERE Code = '{0}' AND Lan = '{1}'", code, lan));
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

            if (response.latestVersion > configRow.ResourceVersion)
            {
                dbManager.BeginTransaction();

                dbManager.Execute("UPDATE ConfigRow SET ResourceVersion = ? WHERE Id = ?", response.latestVersion, 1);

                Resource r;
                Resource[] list = response.list;
                for (int i = 0; i < list.Length; i++)
                {
                    r = list[i];
                    dbManager.Execute("DELETE FROM ResourceRow WHERE Code = ? AND Lan = ?", r.code, r.lan);
                    dbManager.Insert(new ResourceRow
                    {
                        Code = r.code,
                        Lan = r.lan,
                        Desc = r.desc
                    });
                }

                dbManager.Commit();
                Debug.Log(list.Length + " ResourceRow records updated.");
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

        public string LoadLan(SimpleSQLManager dbManager)
        {
            SimpleDataTable dt = dbManager.QueryGeneric("SELECT Lan FROM ConfigRow WHERE Id = 1");
            List<SimpleDataRow> simpleDataRows = dt.rows;
            if (dt == null || simpleDataRows == null || simpleDataRows.Count == 0)
            {
                SaveDefaultConfig(dbManager);
                return "Chinese";
            }
            return simpleDataRows[0]["Lan"].ToString();
        }

        public void CleanProfile(SimpleSQLManager dbManager)
        {
            dbManager.Execute("DELETE FROM SessionRow WHERE Id = 1");
            SceneManager.LoadScene("login");
        }

    }
}