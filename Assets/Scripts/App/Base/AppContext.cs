using App.VO;
using ConsoleApplication.Helper;

namespace App.Base
{
    public class AppContext
    {
        private static readonly AppContext instance = new AppContext();
        private static string Token;
        private static string Key;
        public SeatWatch Watch { get ; set ; }

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
    }
}