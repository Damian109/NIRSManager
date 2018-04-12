namespace NIRSManagerClient.HelpfulModels
{
    /// <summary>
    /// Модель для моели представления настроек соединения. Хранит все возможные на компьютере СУБД
    /// </summary>
    public sealed class DatabaseSelection
    {
        /// <summary>
        /// Название СУБД
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Провайдер СУБД для связи с программой
        /// </summary>
        public string ProviderName { get; set; }

        public DatabaseSelection()
        {
            Name = ProviderName = string.Empty;
        }

        public DatabaseSelection(string name, string providerName)
        {
            Name = name;
            ProviderName = providerName;
        }
    }
}
