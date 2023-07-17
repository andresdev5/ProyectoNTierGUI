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

namespace ProyectoNTierGUI.ViewModel.Employee
{
    using ProyectoNTierGUI.Model;

    class EmployeeListViewModel : INotifyPropertyChanged
    {
        private EmployeeService _employeeService = ContainerProvider.Resolve<EmployeeService>();
        private ObservableCollection<Employee> _employees { get; set; } = new();

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public EmployeeListViewModel()
        {
            Employees = new ObservableCollection<Employee>(_employeeService.GetAll());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
