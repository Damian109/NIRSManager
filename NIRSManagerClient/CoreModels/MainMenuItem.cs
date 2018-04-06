using NIRSCore;

namespace NIRSManagerClient.CoreModels
{
    /// <summary>
    /// В объекте данного класса хранится информация о названии пункта меню а также модели представления
    /// </summary>
    public sealed class MainMenuItem
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Модель представления
        /// </summary>
        public ViewModel NewViewModel { get; set; }

        public MainMenuItem(string name, ViewModel viewModel)
        {
            Name = name;
            NewViewModel = viewModel;
        }
    }
}
