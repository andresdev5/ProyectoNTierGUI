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
    using ProyectoNTierGUI.Core;
    using ProyectoNTierGUI.Model;

    public class PayrollAddViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private TransactionReasonService _transactionReasonService;
        private PayrollService _payrollService;
        private EmployeeService _employeeService;
        private ObservableCollection<Employee> _employees = new();
        private ObservableCollection<TransactionReason> _transactionReasons = new();
        private Dictionary<int,  Collection<TransactionReason>> _transactionsPerEmployee = new();
        private ObservableCollection<TransactionReason> _transactionReasonsEmployee = new();
        private Employee? _selectedEmployee = null;
        private AccountingEntry _formAccountingEntry = new();
        private string? _formMessage = null;

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

        public ObservableCollection<TransactionReason> TransactionReasonsEmployee
        {
            get
            {
                return _transactionReasonsEmployee;
            }
            set
            {
                _transactionReasonsEmployee = value;
                OnPropertyChanged(nameof(TransactionReasonsEmployee));
            }
        }

        public Employee? SelectedEmployee
        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public AccountingEntry FormAccountingEntry
        {
            get
            {
                return _formAccountingEntry;
            }
            set
            {
                _formAccountingEntry = value;
                OnPropertyChanged(nameof(FormAccountingEntry));
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

        public PayrollAddViewModel()
        {
            _payrollService = ContainerProvider.Resolve<PayrollService>();
            _transactionReasonService = ContainerProvider.Resolve<TransactionReasonService>();
            _employeeService = ContainerProvider.Resolve<EmployeeService>();

            _transactionReasons = new ObservableCollection<TransactionReason>(_transactionReasonService.GetAll());
            Employees = new ObservableCollection<Employee>(_employeeService.GetAll());
            
            foreach (var transactionReason in _transactionReasons)
            {
                if (transactionReason.Employee == null)
                {
                    continue;
                }

                if (transactionReason.IsChecked)
                {
                    continue;
                }

                if (!_transactionsPerEmployee.ContainsKey(transactionReason.Employee.Id))
                {
                    _transactionsPerEmployee.Add(transactionReason.Employee.Id, new Collection<TransactionReason>());
                }

                _transactionsPerEmployee[transactionReason.Employee.Id].Add(transactionReason);
            }
        }

        public void SelectEmployee(Employee? employee)
        {
            if (employee == null)
            {
                SelectedEmployee = null;
                TransactionReasonsEmployee = new ObservableCollection<TransactionReason>();
                return;
            }

            if (!_transactionsPerEmployee.ContainsKey(employee.Id))
            {
                SelectedEmployee = null;
                TransactionReasonsEmployee = new ObservableCollection<TransactionReason>();
                return;
            }

            var transactions = _transactionsPerEmployee[employee.Id];
            SelectedEmployee = employee;
            TransactionReasonsEmployee = new ObservableCollection<TransactionReason>(transactions);
        }

        public void Save()
        {
            if (SelectedEmployee == null)
            {
                FormMessage = "No se ha seleccionado un empleado";
                return;
            }

            if (TransactionReasonsEmployee.Count == 0)
            {
                FormMessage = "El detalle esta vacio";
                return;
            }

            Collection<AccountingEntryItem> items = new();

            foreach (var transactionReason in TransactionReasonsEmployee)
            {
                items.Add(new AccountingEntryItem
                {
                    Credit = transactionReason.Type == "INGRESO" ? transactionReason.Amount : 0,
                    Debit = transactionReason.Type == "EGRESO" ? transactionReason.Amount : 0,
                    TransactionReason = transactionReason
                });
            }

            FormAccountingEntry.Employee = SelectedEmployee;
            FormAccountingEntry.Details = items;
            int rowsAffected = _payrollService.SaveAccountingEntry(FormAccountingEntry);

            SelectedEmployee = null;
            TransactionReasonsEmployee = new ObservableCollection<TransactionReason>();
            FormMessage = rowsAffected > 0 ? "Se ha guardado correctamente" : "Hubo un problema al generar el asiento contable";
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
