using System.Text.RegularExpressions;

namespace App.Helper
{
    public class RegexHelper
    {
        //private static Regex mobileReg=new Regex(@"^[1]([3][0-9]{1}|([4][7]{1})|([5][0-3|5-9]{1})|([8][0-9]{1}))[0-9]{8}$");
        private static Regex mobileReg=new Regex(@"^[1][0-9]{10}$");
        private static Regex codeReg=new Regex(@"^[0-9]{4,6}$");

        public static bool isMobile(string str)
        {
            return mobileReg.Match(str).Success;
        }

        public static bool isValidCode(string str)
        {
            return codeReg.Match(str).Success;
        }

    }
}