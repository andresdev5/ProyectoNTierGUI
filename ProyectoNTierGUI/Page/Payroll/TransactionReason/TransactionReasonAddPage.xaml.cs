using ProyectoNTierGUI.ViewModel.Payroll;
using ProyectoNTierGUI.Model;
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
    using ProyectoNTierGUI.Model;

    /// <summary>
    /// Lógica de interacción para TransactionReasonAddPage.xaml
    /// </summary>
    public partial class TransactionReasonAddPage : System.Windows.Controls.Page
    {
        public TransactionReasonAddPage()
        {
            InitializeComponent();
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            var context = (TransactionReasonAddViewModel)DataContext;
            string type = TypeComboBox.Text.ToString();
            string reason = ReasonText.Text.ToString();
            string amountText = AmountText.Text.ToString();
            double amount = Double.TryParse(amountText, out amount) ? amount : 0;
            Employee? employee = EmployeeComboBox.SelectedItem as Employee;

            if (type == "" || reason.Trim() == "" || amountText.Trim() == "" || employee == null)
            {
                context.FormMessage = "Llena todos los campos";
                return;
            }

            var transactionReason = new TransactionReason()
            {
                Type = type,
                Reason = reason,
                Amount = amount,
                Employee = employee
            };

            context.CreateTransactionReason(transactionReason);
            context.FormReasonValue = "";
        }
    }
}
