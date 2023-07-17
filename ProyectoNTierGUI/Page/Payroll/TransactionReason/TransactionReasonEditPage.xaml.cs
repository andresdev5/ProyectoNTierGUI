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
    using ProyectoNTierGUI.Model;

    /// <summary>
    /// Lógica de interacción para TransactionReasonEditPage.xaml
    /// </summary>
    public partial class TransactionReasonEditPage : System.Windows.Controls.Page
    {
        public TransactionReasonEditPage()
        {
            InitializeComponent();
        }

        public void Submit(object sender, RoutedEventArgs e)
        {
            var context = (TransactionReasonEditViewModel)DataContext;
            string? code = CodeComboBox.SelectedValue.ToString();

            if (code == null)
            {
                return;
            }

            string type = TypeComboBox.Text.ToString();
            string reason = ReasonTextBox.Text.ToString();
            string amountText = AmountText.Text.ToString();
            Employee? employee = EmployeeComboBox.SelectedItem as Employee;
            
            if (code == "" || type == "" || reason.Trim() == "" || amountText.Trim() == "" || employee == null)
            {
                context.FormMessage = "Llena todos los campos";
                return;
            }


            double amount = 0;

            try { amount = Double.Parse(amountText); }
            catch (Exception) { }

            var transactionReason = new Model.TransactionReason()
            {
                Code = code,
                Type = type,
                Reason = reason,
                Amount = amount,
                Employee = employee
            };

            context.UpdateTransactionReason(transactionReason);
            CodeComboBox.SelectedIndex = 0;
            TypeComboBox.SelectedIndex = 0;
            ReasonTextBox.Text = "";
        }

        public void OnCodeComboBoxChanged(object? sender, SelectionChangedEventArgs e)
        {
            var context = (TransactionReasonEditViewModel)DataContext;
            string? code = CodeComboBox.SelectedValue.ToString();

            if (code == null)
            {
                return;
            }

            var transactionReason = context.GetTransactionReason(code);
            
            if (transactionReason == null)
            {
                return;
            }

            CodeComboBox.SelectedItem = transactionReason.Code;
            TypeComboBox.Text = transactionReason.Type;
            ReasonTextBox.Text = transactionReason.Reason;
            AmountText.Text = transactionReason.Amount.ToString();

            var comboItems = EmployeeComboBox.Items;

            foreach (var item in comboItems)
            {
                var employee = (Employee)item;
                if (employee.Id == transactionReason?.Employee?.Id)
                {
                    EmployeeComboBox.SelectedItem = employee;
                    break;
                }
            }
        }
    }
}
