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
using System.Windows.Shapes;

namespace ProyectoNTierGUI
{
    /// <summary>
    /// Lógica de interacción para LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public void Submit(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;

            if (username.Trim().Length == 0 )
            {
                ShowError("El nombre de usuario no puede estar vacío");
                return;
            }

            if (password.Trim().Length == 0)
            {
                ShowError("La contraseña no puede estar vacía");
                return;
            }

            string adminUsername = "admin";
            string adminPassword = "admin";

            if (username == adminUsername && password == adminPassword)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ShowError("El nombre de usuario o la contraseña son incorrectos");
            }
        }

        public void ShowError(string message)
        {
            ErrorMessageText.Text = message;
            ErrorMessageText.Visibility = Visibility.Visible;
        }
    }
}
