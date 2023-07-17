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
    using ProyectoNTierGUI.Model;

    public partial class TransactionReasonEditViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<string> _codes = new ObservableCollection<string>();
        private ObservableCollection<Employee> _employees = new();

        private string? _formMessage = null;

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

        public ObservableCollection<Employee> Employees
        {
            get
            {
                return _employees;
            }
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
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

        private TransactionReasonService _transactionReasonService;
        private EmployeeService _employeeService;

        public TransactionReasonEditViewModel()
        {
            _transactionReasonService = ContainerProvider.Resolve<TransactionReasonService>();
            _employeeService = ContainerProvider.Resolve<EmployeeService>();
            Employees = new ObservableCollection<Employee>(_employeeService.GetAll());

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

        public void UpdateTransactionReason(TransactionReason transactionReason)
        {
            _transactionReasonService.Update(transactionReason);
            FormMessage = "Actualizado correctamente";
        }

        public TransactionReason? GetTransactionReason(String code)
        {
            var transactions = _transactionReasonService.GetAll();

            foreach (var transaction in transactions)
            {
                if (transaction.Code == code)
                {
                    return transaction;
                }
            }

            return null;
        }
    }
}
