using ProyectoNTierGUI.Core;
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
    public class EmployeeRemoveViewModel : INotifyPropertyChanged
    {
        private EmployeeService _employeeService = ContainerProvider.Resolve<EmployeeService>();
        private ObservableCollection<int> _ids { get; set; } = new();
        private string? _formMessage = null;

        public string? FormMessage
        {
            get { return _formMessage; }
            set
            {
                _formMessage = value;
                OnPropertyChanged(nameof(FormMessage));
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

        public EmployeeRemoveViewModel()
        {
            var employees = new ObservableCollection<Model.Employee>(_employeeService.GetAll());

            foreach (var employee in employees)
            {
                Ids.Add(employee.Id);
            }
        }

        public void RemoveEmployee(int id)
        {
            _employeeService.Remove(id);
            FormMessage = "Empleado eliminado";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
