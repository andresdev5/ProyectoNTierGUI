using ProyectoNTierGUI.ViewModel.Employee;
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
    /// <summary>
    /// Lógica de interacción para EmployeeRemovePage.xaml
    /// </summary>
    public partial class EmployeeRemovePage : System.Windows.Controls.Page
    {
        public EmployeeRemovePage()
        {
            InitializeComponent();
        }

        public void Submit(object sender, RoutedEventArgs e)
        {
            var context = (EmployeeRemoveViewModel)DataContext;

            if (IdComboBox.SelectedIndex == -1)
            {
                return;
            }

            int id = (int)IdComboBox.SelectedItem;
            context.RemoveEmployee(id);
        }
    }
}
