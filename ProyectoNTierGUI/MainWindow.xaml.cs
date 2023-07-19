using ProyectoNTierGUI.Core;
using ProyectoNTierGUI.Page.Payroll.TransactionReason;
using ProyectoNTierGUI.Page.Payroll.Employee;
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
using ProyectoNTierGUI.Page.Accounting.AccountType;
using ProyectoNTierGUI.Page.Payroll.Payroll;
using ProyectoNTierGUI.Page.Accounting.Account;

namespace ProyectoNTierGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SidebarOnSelectedItem(object? sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            var parentItem = (sender as ListView);
            var selectedItem = parentItem?.SelectedItem as ListViewItem;

            if (selectedItem == null)
            {
                return;
            }

            foreach (ListView listView in FindVisualChildren<ListView>(this))
            {

                if (listView != null && listView != parentItem)
                {
                    listView.SelectedIndex = -1;
                }
            }

            string name = selectedItem.Name;

            // clear NavigationFrame
            NavigationFrame.Navigate(null);

            switch (name)
            {
                case "TransactionReason_Add":
                    NavigationFrame.Navigate(new TransactionReasonAddPage());
                    break;
                case "TransactionReason_Edit":
                    NavigationFrame.Navigate(new TransactionReasonEditPage());
                    break;
                case "TransactionReason_Remove":
                    NavigationFrame.Navigate(new TransactionReasonRemovePage());
                    break;
                case "TransactionReason_List":
                    NavigationFrame.Navigate(new TransactionReasonListPage());
                    break;
                case "Employee_Add":
                    NavigationFrame.Navigate(new EmployeeAddPage());
                    break;
                case "Employee_Edit":
                    NavigationFrame.Navigate(new EmployeeEditPage());
                    break;
                case "Employee_Remove":
                    NavigationFrame.Navigate(new EmployeeRemovePage());
                    break;
                case "Employee_List":
                    NavigationFrame.Navigate(new EmployeeListPage());
                    break;
                case "Accounting_AccountTypes":
                    NavigationFrame.Navigate(new AccountTypeAddPage());
                    break;
                case "Accounting_Accounts":
                    NavigationFrame.Navigate(new AccountsPage());
                    break;
                case "Payroll_Add":
                    NavigationFrame.Navigate(new PayrollAddPage());
                    break;
                case "Payroll_Report":
                    NavigationFrame.Navigate(new PayrollReportPage());
                    break;
                default: break;
            }
        }

        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
            }
        }
    }
}
