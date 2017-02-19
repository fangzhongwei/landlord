using ConsoleApplication.Helper;
using UnityEngine;

namespace App.Base
{
    public class AppContext
    {
        private static readonly AppContext instance = new AppContext();
        private static string Token;
        private static string Key;
        private static string Lan;
        private AppContext()
        {
        }

        public static AppContext GetInstance()
        {
            return instance;
        }

        public string getDesKey()
        {
            if (string.IsNullOrEmpty(Key))
            {
                Key = DESHelper.Decode(Constants.DEFAULT_KEY, "ABCD1234");
            }
            return Key;
        }

        public string GetToken()
        {
            return Token;
        }

        public void SetToken(string token)
        {
            Token = token;
        }

        public string GetLan()
        {
            return Application.systemLanguage.ToString();
        }

        public void SetLan(string lan)
        {
            Lan = lan;
        }
    }
}