using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NIRSCore
{
    /// <summary>
    /// Абстрактный класс, являющийся предком для всех моделей представления
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие, накоторое будут реагировать элементы управления формы
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызываем событие PropertyChanged
        /// </summary>
        /// <param name="prop"></param>
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        /// <summary>
        /// Название формы
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Конструктор абстрактного класса
        /// </summary>
        /// <param name="name">Название формы</param>
        public ViewModel(string name) => Name = name;
    }
}
