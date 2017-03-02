﻿namespace App.Base
{
    public class Constants
    {
        public const string COMMON_DISPATCH_URL = "http://172.16.7.114:8080/v1.0-route";
//        public const string COMMON_DISPATCH_URL = "http://43.254.3.198/v1.0-route";
        public const string DEFAULT_TOKEN = "0";
        public const string VERSION = "1.0.0";
        public const string DEFAULT_KEY = "19B313BB9FD9F9A8A1D0E82590DD77B9";
        public const string WS_ADDRESS =  "ws://172.16.7.114:9000/greeter";
//        public const string WS_ADDRESS =  "ws://43.254.3.198:9000/greeter";

        public const int CLIENT_ID = 1;

        public const int API_ID_SEND_CODE = 1001;
        public const int API_ID_LOGIN = 1002;
        public const int API_ID_LOGIN_BY_TOKEN = 1003;
        public const int API_ID_LOGOUT = 1004;
        public const int API_UPDATE_NICKNAME = 1005;
        public const int API_LOAD_ALL_RESOURCES = 1006;
        public const int API_PULL_RESOURCES = 1007;

        public const int API_QUERY_DIAMOND_AMOUNT = 2001;
        public const int API_GET_PRICE_LIST = 2002;
        public const int API_GET_CHANNEL_LIST = 2003;
        public const int API_DEPOSIT_REQUEST = 2004;
        public const int API_QUERY_DEPOSIT = 2005;
        public const int API_CHECK_GAME_STATUS = 2006;

        public static char[] CARDS_SEPERATOR = new char[1]{','};

    }
}