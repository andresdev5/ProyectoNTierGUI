using ProyectoNTierGUI.Model;
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

    class EmployeeAddViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private EmployeeService _employeeService = ContainerProvider.Resolve<EmployeeService>();
        private string? _formMessage;
        private Employee _mEmployee = new Employee()
        {
            IdCard = "",
            FullName = "",
            HireDate = DateTime.Now,
            Salary = 0
        };

        public string? FormMessage
        {
            get { return _formMessage; }
            set
            {
                _formMessage = value;
                OnPropertyChanged(nameof(FormMessage));
            }
        }

        public Employee? MEmployee
        {
            get { return _mEmployee; }
            set
            {
                _mEmployee = value;
                OnPropertyChanged(nameof(MEmployee));
            }
        }

        public void SaveEmployee()
        {
            if (MEmployee == null)
            {
                FormMessage = "Ingrese los campos";
                return;
            }

            if (MEmployee.FullName == null || MEmployee.FullName.Length == 0)
            {
                FormMessage = "El nombre del empleado es requerido";
                return;
            }

            if (MEmployee.Salary <= 0)
            {
                FormMessage = "El salario del empleado es requerido";
                return;
            }

            if (MEmployee.HireDate == null)
            {
                FormMessage = "La fecha de ingreso del empleado es requerida";
                return;
            }

            if (MEmployee.IdCard == null || MEmployee.IdCard.Length == 0)
            {
                FormMessage = "La cédula del empleado es requerida";
                return;
            }

            var id = _employeeService.SaveEmployee(MEmployee);

            if (id > 0)
            {
                FormMessage = $"Empleado guardado correctamente con id {id}";
            }
            else
            {
                FormMessage = "Error al guardar el empleado";
            }

            MEmployee = new Employee()
            {
                IdCard = "",
                FullName = "",
                HireDate = DateTime.Now,
                Salary = 0
            };
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
