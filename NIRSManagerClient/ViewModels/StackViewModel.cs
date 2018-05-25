using NIRSCore;
using System.Threading.Tasks;
using NIRSCore.StackOperations;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NIRSManagerClient.ViewModels
{
    public sealed class StackViewModel : ViewModel
    {
        //Список операций
        private List<Operation> _operations;

        /// <summary>
        /// Список операций
        /// </summary>
        public ObservableCollection<Operation> Operations
        {
            get
            {
                ObservableCollection<Operation> operations = new ObservableCollection<Operation>();
                if (_operations != null)
                    foreach (var elem in _operations)
                        operations.Add(elem);
                return operations;
            }
        }

        //Асинхронное получение списка операций
        private async void GetOperationsAsync() => await Task.Run(() =>
        {
            _operations = NirsSystem.StackOperations.Operations;
            OnPropertyChanged("Operations");
        });

        /// <summary>
        /// Конструктор
        /// </summary>
        public StackViewModel() : base("Стек операций") => GetOperationsAsync();
    }
}
