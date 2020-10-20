namespace WebApi.Helpers
{
    public class AppSettings
    {
        public static string SecretJWT = "D42B1171FC499D145D136CBD19B6F";
        public static int TIME_EXPIRES = 7; //7 day


        public static string DB_HOST = "127.0.0.1";
        public static string DB_USER = "root";
        public static string DB_PW = "msf";
        public static string DB_PORT = "3306";  // default port mysql 3306
        public static string DB_NAME = "library";
        public static string DB_CONN_STRING = $"server={DB_HOST};database={DB_NAME};user={DB_USER};password={DB_PW};port={DB_PORT}";
    }
}

