using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.ViewModel.Accounting
{
    using GalaSoft.MvvmLight.Command;
    using ProyectoNTierGUI.Core;
    using ProyectoNTierGUI.Model;
    using ProyectoNTierGUI.Service;
    using System.Windows.Input;

    class AccountTypeViewModel : INotifyPropertyChanged
    {
        private AccountingService _accountingService = ContainerProvider.Resolve<AccountingService>();
        private ObservableCollection<AccountType> _accountTypes = new();
        public RelayCommand<AccountType> DeleteCommand { get; set; }
        public RelayCommand<AccountType> UpdateCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        private string _searchText = "";
        private AccountType _accountType = new()
        {
            Name = ""
        };

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public AccountType AccountType
        {
            get => _accountType;
            set
            {
                _accountType = value;
                OnPropertyChanged(nameof(AccountType));
            }
        }

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
            DeleteCommand = new RelayCommand<AccountType>(DeleteAccountType);
            UpdateCommand = new RelayCommand<AccountType>(UpdateAccountType);
            SearchCommand = new RelayCommand(SearchAccountType);
        }


        public void AddType(string name)
        {
            if (AccountType.Name == null || AccountType.Name.Trim() == "")
            {
                return;
            }

            _accountingService.AddType(new AccountType()
            {
                Name = name
            });
            AccountTypes = new ObservableCollection<AccountType>(_accountingService.getTypes());
        }

        public void DeleteAccountType(AccountType accountType)
        {
            _accountingService.DeleteType(accountType);
            AccountTypes = new ObservableCollection<AccountType>(_accountingService.getTypes());
        }

        public void UpdateAccountType(AccountType accountType)
        {
            _accountingService.UpdateType(accountType);
            AccountTypes = new ObservableCollection<AccountType>(_accountingService.getTypes());
        }

        public void SearchAccountType()
        {
            var accountTypes = _accountingService.getTypes();

            if (SearchText.Trim() == "")
            {
                AccountTypes = new(accountTypes);
                return;
            }

            AccountTypes = new(accountTypes.Where(w =>
            {
                var lowered = w.Name.ToLower();
                var term = SearchText.ToLower();
                return lowered.Contains(term);
            }));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
