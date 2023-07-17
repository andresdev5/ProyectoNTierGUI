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
    /// Lógica de interacción para EmployeeEditPage.xaml
    /// </summary>
    public partial class EmployeeEditPage : System.Windows.Controls.Page
    {
        public EmployeeEditPage()
        {
            InitializeComponent();
        }

        public void Submit(object sender, RoutedEventArgs e)
        {
            var context = (EmployeeEditViewModel)DataContext;
            var id = IdComboBox.Text;
            context.UpdateEmployee(int.Parse(id));
        }

        public void IdComboChanged(object sender, SelectionChangedEventArgs e)
        {
            var context = (EmployeeEditViewModel)DataContext;

            if (IdComboBox.SelectedIndex == -1)
            {
                return;
            }

            int id = (int)IdComboBox.SelectedItem;

            context.LoadEmployee(id);
        }
    }
}
