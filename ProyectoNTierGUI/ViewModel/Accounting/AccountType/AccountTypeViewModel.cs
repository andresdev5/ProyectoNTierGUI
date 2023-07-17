using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.ViewModel.Accounting.AccountType
{
    using ProyectoNTierGUI.Core;
    using ProyectoNTierGUI.Model;
    using ProyectoNTierGUI.Service;

    class AccountTypeViewModel : INotifyPropertyChanged
    {
        private AccountingService _accountingService = ContainerProvider.Resolve<AccountingService>();
        private ObservableCollection<AccountType> _accountTypes = new();

        public ObservableCollection<AccountType> AccountTypes
        {
            get => _accountTypes;
            set
            {
                _accountTypes = value;
                OnPropertyChanged(nameof(AccountTypes));
            }
        }

        public AccountTypeViewModel()
        {
            AccountTypes = new ObservableCollection<AccountType>(_accountingService.getTypes());
        }

        public void AddType(string name)
        {
            _accountingService.AddType(new AccountType()
            {
                Name = name
            });
            AccountTypes = new ObservableCollection<AccountType>(_accountingService.getTypes());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
