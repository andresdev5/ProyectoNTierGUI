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

namespace ProyectoNTierGUI.Page.Payroll.Payroll
{
    using ProyectoNTierGUI.Model;

    /// <summary>
    /// Lógica de interacción para PayrollAddPage.xaml
    /// </summary>
    public partial class PayrollAddPage : System.Windows.Controls.Page
    {

        public PayrollAddPage()
        {
            InitializeComponent();
        }

        public void OnEmployeeComboBoxChanged(object? sender, SelectionChangedEventArgs e)
        {
            var context = (DataContext as ViewModel.Payroll.PayrollAddViewModel);
            Employee? employee = EmployeeComboBox.SelectedItem as Employee;

            if (context == null)
            {
                return;
            }

            context.SelectEmployee(employee);
        }

        public void Submit(object? sender, RoutedEventArgs e)
        {
            var context = (DataContext as ViewModel.Payroll.PayrollAddViewModel);
            
            if (context == null)
            {
                return;
            }

            context.Save();
        }
    }
}
