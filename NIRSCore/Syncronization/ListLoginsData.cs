namespace NIRSCore.Syncronization
{
    /// <summary>
    /// Объект представляет список всех доступных для обмена пользователей
    /// </summary>
    public sealed class ListLoginsData
    {
        public string[] ListLogins { get; set; }

        public ListLoginsData() => ListLogins = null;

        public ListLoginsData(string[] strings) => ListLogins = strings;
    }
}
