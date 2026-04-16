namespace YoutubeClone.Shared.Constants
{
    public static class ResponseConstants
    {
        // Usuarios
        public const string USER_NOT_EXIST = "El usuario no existe";

        // Canales
        public const string CHANNEL_NOT_EXIST = "El canal no existe";

        // Token
        public const string AUTH_TOKEN_NOT_FOUND = "El token no es correcto o expiró";

        public static string ErrorUnexpected(string traceId)
        {
            return $"Ha ocurrido un error inesperado: Contacte con soporte, mencionando el siguiente código de error: {traceId}";
        }

        public static string ConfigurationPropertyNotFound(string property)
        {
            return $"Falta la propiedad '{property}' por establecer en la configuración del aplicativo.";
        }
    }
}
