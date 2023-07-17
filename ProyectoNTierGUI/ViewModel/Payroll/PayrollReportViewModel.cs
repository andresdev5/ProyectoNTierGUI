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

    class EmployeeReportItem
    {
        public Employee Employee { get; set; }
        public double Total { get; set; }
    }

    class PayrollReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private TransactionReasonService _transactionReasonService;
        private ObservableCollection<TransactionReason> _transactionReasons = new();
        private ObservableCollection<EmployeeReportItem> _reportItems = new();
        private Dictionary<int, Collection<TransactionReason>> _transactionsPerEmployee = new();
        private Dictionary<Employee, EmployeeReportItem> _itemsPerEmployee = new();
        private DateTime? _startDate = null;
        private DateTime? _endDate = null;

        public DateTime? StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public ObservableCollection<EmployeeReportItem> EmployeeReportItems
        {
            get
            {
                return _reportItems;
            }
            set
            {
                _reportItems = value;
                OnPropertyChanged(nameof(EmployeeReportItems));
            }
        }

        public PayrollReportViewModel()
        {
            _transactionReasonService = ContainerProvider.Resolve<TransactionReasonService>();
            LoadData();
        }

        public void LoadData()
        {
            _transactionReasons = new ObservableCollection<TransactionReason>(_transactionReasonService.GetAll());
            _itemsPerEmployee = new Dictionary<Employee, EmployeeReportItem>();
            _transactionsPerEmployee = new Dictionary<int, Collection<TransactionReason>>();

            foreach (TransactionReason transactionReason in _transactionReasons)
            {
                if (transactionReason.Employee == null)
                {
                    continue;
                }

                // check if transaction date is between start and end date
                if ((StartDate != null && EndDate != null) && (transactionReason.CreatedAt < StartDate || transactionReason.CreatedAt > EndDate))
                {
                    continue;
                }

                if (_transactionsPerEmployee.ContainsKey(transactionReason.Employee.Id))
                {
                    _transactionsPerEmployee[transactionReason.Employee.Id].Add(transactionReason);
                }
                else
                {
                    _transactionsPerEmployee.Add(transactionReason.Employee.Id, new Collection<TransactionReason>() { transactionReason });
                }

                if (_itemsPerEmployee.ContainsKey(transactionReason.Employee))
                {
                    double amount = transactionReason.Amount;

                    if (transactionReason.Type == "EGRESO")
                    {
                        amount *= -1;
                    }

                    _itemsPerEmployee[transactionReason.Employee].Total += amount;
                }
                else
                {
                    _itemsPerEmployee.Add(transactionReason.Employee, new EmployeeReportItem() { Employee = transactionReason.Employee, Total = transactionReason.Amount });
                }
            }

            EmployeeReportItems = new ObservableCollection<EmployeeReportItem>();

            foreach (KeyValuePair<Employee, EmployeeReportItem> entry in _itemsPerEmployee)
            {
                var employee = entry.Key;
                var item = entry.Value;

                EmployeeReportItems.Add(item);
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
