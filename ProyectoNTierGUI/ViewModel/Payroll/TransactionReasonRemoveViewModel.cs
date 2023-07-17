using Newtonsoft.Json;
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
    public partial class TransactionReasonRemoveViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<string> _codes = new ObservableCollection<string>();
        private string? _formMessage = null;
        private TransactionReasonService _transactionReasonService 
            = ContainerProvider.Resolve<TransactionReasonService>();

        public ObservableCollection<string> Codes
        {
            get
            {
                return _codes;
            }

            set
            {
                _codes = value;
                OnPropertyChanged(nameof(Codes));
            }
        }

        public string? FormMessage
        {
            get { return _formMessage; }
            set
            {
                _formMessage = value;
                OnPropertyChanged(nameof(FormMessage));
            }
        }

        public TransactionReasonRemoveViewModel()
        {
            var transactionReasons = _transactionReasonService.GetAll();

            foreach (var transactionReason in transactionReasons)
            {
                Codes.Add(transactionReason.Code);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RemoveTransactionReason(string code)
        {
            _transactionReasonService.Remove(code);
            FormMessage = "Motivo de egreso/ingreso eliminado";
        }
    }
}
