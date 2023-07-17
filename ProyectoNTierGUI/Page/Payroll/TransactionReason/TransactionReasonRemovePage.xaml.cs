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

namespace ProyectoNTierGUI.Page.Payroll.TransactionReason
{
    /// <summary>
    /// Lógica de interacción para TransactionReasonRemovePage.xaml
    /// </summary>
    public partial class TransactionReasonRemovePage : System.Windows.Controls.Page
    {
        public TransactionReasonRemovePage()
        {
            InitializeComponent();
        }

        public void Submit(object sender, RoutedEventArgs e)
        {
            var context = (TransactionReasonRemoveViewModel)DataContext;
            string code = CodeComboBox.Text.ToString();
            
            if (code == "" )
            {
                context.FormMessage = "Llena todos los campos";
                return;
            }

            context.RemoveTransactionReason(code);
            CodeComboBox.SelectedIndex = 0;
        }
    }
}
