namespace NIRSCore.HelpfulEnumsStructs
{
    /// <summary>
    /// Перечисление, описывающее статус регистрации
    /// </summary>
    public enum RegistrationStatus
    {
        RegLogin,     //Ввести логин
        RegPassword,  //Ввести пароль
        RegOK,        //Данные корректны
        RegError,     //Такой пользователь существует
        RegServerErr, //Сервер недоступен
        RegGood       //Регистрация успешна
    }
}
