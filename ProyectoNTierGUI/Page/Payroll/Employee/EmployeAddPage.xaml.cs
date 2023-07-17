using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoNTierGUI.Page.Payroll.Employee
{
    using ProyectoNTierGUI.Model;
    using ProyectoNTierGUI.ViewModel.Employee;

    /// <summary>
    /// Lógica de interacción para EmployeAddPage.xaml
    /// </summary>
    public partial class EmployeeAddPage : System.Windows.Controls.Page
    {
        public EmployeeAddPage()
        {
            InitializeComponent();
        }

        public void Submit(object? sender, RoutedEventArgs e)
        {
            var context = (EmployeeAddViewModel)DataContext;
            context.SaveEmployee();
        }
    }
}
