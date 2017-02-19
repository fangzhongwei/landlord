using UnityEngine;

namespace App.Helper
{
    public class DeviceHelper
    {
        public static int getDeviceType()
        {
            string operatingSystem = SystemInfo.operatingSystem.ToLower();
            if (operatingSystem.Contains("ios"))
            {
                return 1;
            }
            if (operatingSystem.Contains("android"))
            {
                return 2;
            }
            if (operatingSystem.Contains("windows"))
            {
                return 3;
            }
            if (operatingSystem.Contains("ios"))
            {
                return 4;
            }
            return 0;
        }
    }
}