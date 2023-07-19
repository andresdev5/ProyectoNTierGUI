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

        private void DataGridPrinting(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                ReportGrid.Measure(pageSize);
                ReportGrid.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                Printdlg.PrintVisual(ReportGrid, Title);
            }
        }
    }
}
