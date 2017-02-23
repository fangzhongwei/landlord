using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleSQL;
using UnityEngine;

namespace App.Base
{
    public class SimpleSQLManager2 : MonoBehaviour
    {
        public string overrideBasePath = "";
        public string workingName = "";
        protected SQLiteConnection _db;
        public TextAsset databaseFile;
        public bool changeWorkingName;
        public bool overwriteIfExists;
        public bool debugTrace;
        public DatabaseCreatedDelegate databaseCreated;

        public TimeSpan BusyTimeout
        {
            get
            {
                Initialize(false);
                return _db.BusyTimeout;
            }
            set
            {
                Initialize(false);
                _db.BusyTimeout = value;
            }
        }

        public bool IsInTransaction
        {
            get
            {
                Initialize(false);
                return _db.IsInTransaction;
            }
        }

        public IEnumerable<TableMapping> TableMappings
        {
            get
            {
                Initialize(false);
                return _db.TableMappings;
            }
        }

        private void Awake()
        {
            LoadRuntimeLibrary();
            //Initialize(false);
        }

        private void LoadRuntimeLibrary()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                    string path1_1 = Application.dataPath + "/../";
                    try
                    {
                        if (File.Exists(Path.Combine(path1_1, "sqlite3.dll")))
                            File.Delete(Path.Combine(path1_1, "sqlite3.dll"));
                    }
                    catch
                    {
                    }
                    if (is64Bit())
                    {
                        RuntimeHelper.CreateFileFromEmbeddedResource("SimpleSQL.Resources.sqlite3.dll_64.resource", Path.Combine(path1_1, "sqlite3.dll"));
                        break;
                    }
                    RuntimeHelper.CreateFileFromEmbeddedResource("SimpleSQL.Resources.sqlite3.dll_32.resource", Path.Combine(path1_1, "sqlite3.dll"));
                    break;
                case RuntimePlatform.OSXPlayer:
                    break;
                case RuntimePlatform.IPhonePlayer:
                    break;
                case RuntimePlatform.Android:
                    break;
                default:
                    string path1_2 = Application.dataPath + "/../";
                    if (is64Bit())
                    {
                        RuntimeHelper.CreateFileFromEmbeddedResource("SimpleSQL.Resources.sqlite3.dll_64.resource", Path.Combine(path1_2, "sqlite3.dll"));
                        break;
                    }
                    RuntimeHelper.CreateFileFromEmbeddedResource("SimpleSQL.Resources.sqlite3.dll_32.resource", Path.Combine(path1_2, "sqlite3.dll"));
                    break;
            }
        }

        private bool is64Bit()
        {
            string environmentVariable = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            return !string.IsNullOrEmpty(environmentVariable) && !(environmentVariable.Substring(0, 3) == "x86");
        }

        public void Initialize(bool forceReinitialization)
        {
            if (_db != null && !forceReinitialization)
                return;
            if (changeWorkingName && workingName.Trim() == "")
            {
                Debug.LogError((object) ("If you want to change the database's working name, then you will need to supply a new working name in the SimpleSQLManager [" + gameObject.name + "]"));
            }
            else
            {
                Close();
                Dispose();
                string str = Path.Combine(string.IsNullOrEmpty(overrideBasePath) ? Application.persistentDataPath : overrideBasePath, changeWorkingName ? workingName.Trim() : databaseFile.name + ".bytes");
                bool flag1 = File.Exists(str);
                bool flag2 = true;
                if (overwriteIfExists)
                {
                    if (flag1)
                        goto label_6;
                }
                if (flag1)
                    goto label_12;
                label_6:
                try
                {
                    if (flag1)
                        File.Delete(str);
                    File.WriteAllBytes(str, databaseFile.bytes);
                    if (databaseCreated != null)
                        databaseCreated(str);
                }
                catch
                {
                    flag2 = false;
                    Debug.LogError((object) ("Failed to open database at the working path: " + str));
                }
                label_12:
                if (!flag2)
                    return;
                CreateConnection(str);
                _db.Trace = debugTrace;
            }
        }

        protected virtual void CreateConnection(string documentsPath)
        {
            _db = new SQLiteConnection(documentsPath);
        }

        private static byte[] StreamToBytes(Stream input)
        {
            using (MemoryStream memoryStream = new MemoryStream(input.CanSeek ? (int) input.Length : 0))
            {
                byte[] buffer = new byte[4096];
                int count;
                do
                {
                    count = input.Read(buffer, 0, buffer.Length);
                    memoryStream.Write(buffer, 0, count);
                }
                while (count != 0);
                return memoryStream.ToArray();
            }
        }

        public void Close()
        {
            if (_db == null)
                return;
            if (debugTrace)
                Debug.Log((object) (name + ": closing connection"));
            _db.Close();
        }

        public void Dispose()
        {
            if (_db == null)
                return;
            if (debugTrace)
                Debug.Log((object) (name + ": disposing connection"));
            _db.Dispose();
            _db = (SQLiteConnection) null;
        }

        private void OnApplicationQuit()
        {
            Close();
            Dispose();
        }

        public TableMapping GetMapping(Type type)
        {
            Initialize(false);
            return _db.GetMapping(type);
        }

        public int CreateTable<T>()
        {
            Initialize(false);
            return _db.CreateTable<T>();
        }

        public SQLiteCommand CreateCommand(string cmdText, params object[] ps)
        {
            Initialize(false);
            return _db.CreateCommand(cmdText, ps);
        }

        public int Execute(string query, params object[] args)
        {
            Initialize(false);
            return _db.Execute(query, args);
        }

        public List<T> Query<T>(string query, params object[] args) where T : new()
        {
            Initialize(false);
            return _db.Query<T>(query, args);
        }

        public List<object> Query(TableMapping map, string query, params object[] args)
        {
            Initialize(false);
            return _db.Query(map, query, args);
        }

        public SimpleDataTable QueryGeneric(string query, params object[] args)
        {
            return CreateCommand(query, args).ExecuteQueryGeneric();
        }

        public T QueryFirstRecord<T>(out bool recordExists, string query, params object[] args) where T : new()
        {
            Initialize(false);
            List<T> objList = _db.Query<T>(query, args);
            if (objList.Count > 0)
            {
                recordExists = true;
                return objList[0];
            }
            recordExists = false;
            return default (T);
        }

        public object QueryFirstRecord(out bool recordExists, TableMapping map, string query, params object[] args)
        {
            Initialize(false);
            List<object> objectList = _db.Query(map, query, args);
            if (objectList.Count > 0)
            {
                recordExists = true;
                return objectList[0];
            }
            recordExists = false;
            return (object) null;
        }

        public TableQuery<T> Table<T>() where T : new()
        {
            Initialize(false);
            return _db.Table<T>();
        }

        public T Get<T>(object pk) where T : new()
        {
            Initialize(false);
            return _db.Get<T>(pk);
        }

        public void BeginTransaction()
        {
            Initialize(false);
            _db.BeginTransaction();
        }

        public void Rollback()
        {
            Initialize(false);
            _db.Rollback();
        }

        public void Commit()
        {
            Initialize(false);
            _db.Commit();
        }

        public void RunInTransaction(Action action)
        {
            Initialize(false);
            _db.RunInTransaction(action);
        }

        public int InsertAll(IEnumerable objects, out long lastRowID)
        {
            Initialize(false);
            return _db.InsertAll(objects, out lastRowID);
        }

        public int InsertAll(IEnumerable objects)
        {
            long lastRowID = -1;
            return InsertAll(objects, out lastRowID);
        }

        public int Insert(object obj, out long rowID)
        {
            Initialize(false);
            return _db.Insert(obj, out rowID);
        }

        public int Insert(object obj)
        {
            long rowID = -1;
            return Insert(obj, out rowID);
        }

        public int Insert(object obj, Type objType, out long rowID)
        {
            Initialize(false);
            return _db.Insert(obj, objType, out rowID);
        }

        public int Insert(object obj, Type objType)
        {
            long rowID = -1;
            return Insert(obj, objType, out rowID);
        }

        public int Insert(object obj, string extra, out long rowID)
        {
            Initialize(false);
            return _db.Insert(obj, extra, out rowID);
        }

        public int Insert(object obj, string extra)
        {
            long rowID = -1;
            return Insert(obj, extra, out rowID);
        }

        public int Insert(object obj, string extra, Type objType, out long rowID)
        {
            Initialize(false);
            return _db.Insert(obj, extra, objType, out rowID);
        }

        public int Insert(object obj, string extra, Type objType)
        {
            long rowID = -1;
            return Insert(obj, extra, objType, out rowID);
        }

        public int UpdateTable(object obj)
        {
            Initialize(false);
            return _db.Update(obj);
        }

        public int UpdateTable(object obj, Type objType)
        {
            Initialize(false);
            return _db.Update(obj, objType);
        }

        public int Delete<T>(T obj)
        {
            Initialize(false);
            return _db.Delete<T>(obj);
        }
    }
}