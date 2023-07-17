using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.ViewModel.Employee
{
    using ProyectoNTierGUI.Core;
    using ProyectoNTierGUI.Model;
    using ProyectoNTierGUI.Service;
    using System.Collections.ObjectModel;

    public class EmployeeEditViewModel : INotifyPropertyChanged
    {
        private EmployeeService _employeeService = ContainerProvider.Resolve<EmployeeService>();
        private Employee _memployee = new()
        {
            Id = -1,
            IdCard = null,
            FullName = null,
            HireDate = DateTime.Now,
            Salary = 0,
        };
        private ObservableCollection<Employee> _employees { get; set; } = new();
        private ObservableCollection<int> _ids { get; set; } = new();
        private string? _formMessage = null;

        public Employee MEmployee
        {
            get { return _memployee; }
            set
            {
                _memployee = value;
                OnPropertyChanged(nameof(MEmployee));
            }
        }

        public ObservableCollection<int> Ids
        {
            get { return _ids; }
            set
            {
                _ids = value;
                OnPropertyChanged(nameof(Ids));
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
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

        public EmployeeEditViewModel() {
            var employees = _employeeService.GetAll();

            foreach (var employee in employees)
            {
                Employees.Add(employee);
                Ids.Add(employee.Id);
            }
        }

        public void UpdateEmployee(int id)
        {
            var employee = Employees.Where(e => e.Id == id).FirstOrDefault();

            if (employee == null)
            {
                return;
            }

            _employeeService.Update(employee);
            FormMessage = "Empleado actualizado correctamente";
        }

        public void LoadEmployee(int id)
        {
            var employee = Employees.Where(e => e.Id == id).FirstOrDefault();
            if (employee == null)
            {
                return;
            }
            MEmployee = employee;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
