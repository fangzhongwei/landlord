using System;

namespace App.Helper
{
    public class GUIDHelper
    {
        public static string generate()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}