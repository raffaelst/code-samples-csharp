namespace Dividendos.API.Model.Messages
{
    /// <summary>
    /// AuthMessage
    /// </summary>
    public static class AuthMessage
    {
        private static readonly string _invalidLogin = "Usuário Inválido";
        private static readonly string _invalidRefreshToken = "Refresh Token inválido";
        private static readonly string _expiredRefreshToken = "Refresh Token expirado";
        private static readonly string _invalidPassword = "A senha inserida está incorreta";
        private static readonly string _loginBlocked = "Seu usuário foi bloqueado por excesso de tentativas de login. Clique em esqueci minha senha e realize o procedimento solicitado para voltar a ter acesso.";
        private static readonly string _loginSecondAttempts = "Na próxima tentativa errada o usuário será bloqueado";

        /// <summary>
        /// InvalidLogin
        /// </summary>
        public static string InvalidLogin => _invalidLogin;
        /// <summary>
        /// InvalidRefreshToken
        /// </summary>
        public static string InvalidRefreshToken => _invalidRefreshToken;
        /// <summary>
        /// ExpiredRefreshToken
        /// </summary>
        public static string ExpiredRefreshToken => _expiredRefreshToken;

        /// <summary>
        /// LoginBlocked
        /// </summary>
        public static string LoginBlocked => _loginBlocked;

        /// <summary>
        /// LoginSecondAttempts
        /// </summary>
        public static string LoginSecondAttempts => _loginSecondAttempts;

        /// <summary>
        /// InvalidPassword
        /// </summary>
        public static string InvalidPassword => _invalidPassword;
    }
}
