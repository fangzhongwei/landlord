namespace App.Base
{
    public class ErrorCode
    {

        public const string EC_UC_NO_MOBILE = "EC_UC_NO_MOBILE";
        public const string EC_UC_NO_CODE = "EC_UC_NO_CODE";
        public const string EC_UC_NO_NICKNAME = "EC_UC_NO_NICKNAME";

        public const string EC_UC_INVALID_MOBILE = "EC_UC_INVALID_MOBILE";
        public const string EC_UC_INVALID_CODE = "EC_UC_INVALID_CODE";
        public const string EC_UC_NICKNAME_TOO_LONG = "EC_UC_NICKNAME_TOO_LONG";

        public const string EC_NETWORK_UNREACHED = "EC_NETWORK_UNREACHED";
        public const string EC_NETWORK_TIMEOUT = "EC_NETWORK_TIMEOUT";
        public const string EC_SERVER_ERROR = "EC_SERVER_ERROR";
        public const string EC_PARSE_DATA_ERROR = "EC_PARSE_DATA_ERROR";

        public const string MSG_CODE_SENDED = "MSG_CODE_SENDED";

        public const string EC_SSO_SESSION_EXPIRED = "EC_SSO_SESSION_EXPIRED";
        public const string EC_SSO_SESSION_REPELLED = "EC_SSO_SESSION_REPELLED";

        public const string EC_GAME_INVALID_DATA = "EC_GAME_INVALID_DATA";

    }
}