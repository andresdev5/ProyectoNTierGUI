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

    public partial class TransactionReasonAddViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string? _formMessage = null;
        private string? _formReasonValue = null;
        private string? _formAmountValue = "0.0";
        private ObservableCollection<Employee> _employees = new();

        public string? FormMessage
        {
            get { return _formMessage; }
            set
            {
                _formMessage = value;
                OnPropertyChanged(nameof(FormMessage));
            }
        }

        public string? FormReasonValue
        {
            get { return _formReasonValue; }
            set
            {
                _formReasonValue = value;
                OnPropertyChanged(nameof(FormReasonValue));
            }
        }

        public string? FormAmountValue
        {
            get { return _formAmountValue; }
            set
            {
                _formAmountValue = value;
                OnPropertyChanged(nameof(FormAmountValue));
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

        private TransactionReasonService _transactionReasonService;
        private EmployeeService _employeeService;

        public TransactionReasonAddViewModel()
        {
            _transactionReasonService = ContainerProvider.Resolve<TransactionReasonService>();
            _employeeService = ContainerProvider.Resolve<EmployeeService>();
            Employees = new ObservableCollection<Employee>(_employeeService.GetAll());
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CreateTransactionReason(TransactionReason transactionReason)
        {
            var id = _transactionReasonService.Add(transactionReason);
            FormMessage = $"Agregado Motivo de egreso/ingreso correctamente con id {id}";
        }
    }
}
