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

namespace ProyectoNTierGUI.Page.Accounting.AccountType
{
    /// <summary>
    /// Lógica de interacción para AccountTypeAddPage.xaml
    /// </summary>
    public partial class AccountTypeAddPage : System.Windows.Controls.Page
    {
        public AccountTypeAddPage()
        {
            InitializeComponent();
        }

        public void Submit(object sender, RoutedEventArgs e)
        {
            var name = NameTextBox.Text;
            var vm = (ViewModel.Accounting.AccountType.AccountTypeViewModel)DataContext;
            vm.AddType(name);
        }
    }
}
