namespace NIRSCore.HelpfulEnumsStructs
{
    /// <summary>
    /// Перечисление, описывающее статус авторизации
    /// </summary>
    public enum AuthorizationStatus
    {
        AuthLogin,     //Необходимо ввести логин
        AuthPassword,  //Необходимо ввести пароль
        AuthOK,        //Все данные введены
        AuthError      //Авторизация не удалась
    }
}
