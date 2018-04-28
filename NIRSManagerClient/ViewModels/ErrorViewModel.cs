using NIRSCore;

namespace NIRSManagerClient.ViewModels
{
    /// <summary>
    /// Модель представления - возникла неисправимая ошибка
    /// </summary>
    public sealed class ErrorViewModel : ViewModel
    {
        /// <summary>
        /// Заголовок окна ошибки
        /// </summary>
        public string Head { get; private set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Команда загрузки другого окна при выходе
        /// </summary>
        public RelayCommand CommandBack { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="head">Заголовок ошибки</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="command">Команда перехода</param>
        public ErrorViewModel(string head, string message, RelayCommand command) : base("Главное окно")
        {
            Head = head;
            Message = message;
            CommandBack = command;
        }
    }
}
