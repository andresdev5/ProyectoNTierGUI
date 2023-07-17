using Autofac;
using ProyectoNTierGUI.Core;
using ProyectoNTierGUI.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProyectoNTierGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ContainerBuilder _builder;

        public App() : base()
        {
            CommunicationHandler.Instance.Connect();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _builder = new ContainerBuilder();
            _builder.RegisterType<TransactionReasonService>().AsSelf();
            _builder.RegisterType<EmployeeService>().AsSelf();
            _builder.RegisterType<AccountingService>().AsSelf();
            _builder.RegisterType<PayrollService>().AsSelf();
            _builder.RegisterType<MainWindow>().AsSelf();
            _builder.RegisterType<LoginWindow>().AsSelf();

            var container = _builder.Build();
            ContainerProvider.Container = container;

            using (var scope = container.BeginLifetimeScope())
            {
                var window = scope.Resolve<MainWindow>();
                window.Show();
            }
        }
    }
}
