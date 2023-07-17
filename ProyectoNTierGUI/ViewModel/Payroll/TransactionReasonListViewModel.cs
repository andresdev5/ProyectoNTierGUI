using ProyectoNTierGUI.Core;
using ProyectoNTierGUI.Model;
using ProyectoNTierGUI.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.ViewModel.Payroll
{
    class TransactionReasonListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<TransactionReason> _items = new();

        public ObservableCollection<TransactionReason> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private TransactionReasonService _transactionReasonService;

        public TransactionReasonListViewModel()
        {
            _transactionReasonService = ContainerProvider.Resolve<TransactionReasonService>();
            
            var transactionReasons = _transactionReasonService.GetAll();

            foreach (var transactionReason in transactionReasons)
            {
                Items.Add(transactionReason);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
