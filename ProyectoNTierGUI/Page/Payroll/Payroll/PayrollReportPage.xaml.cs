using ProyectoNTierGUI.ViewModel.Payroll;
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
    /// <summary>
    /// Lógica de interacción para PayrollReportPage.xaml
    /// </summary>
    public partial class PayrollReportPage : System.Windows.Controls.Page
    {
        public PayrollReportPage()
        {
            InitializeComponent();
        }

        public void OnDateChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PayrollReportViewModel viewModel)
            {
                ((PayrollReportViewModel)DataContext).LoadData();
            }
        }
    }
}
