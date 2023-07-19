using GalaSoft.MvvmLight.Command;
using ProyectoNTierGUI.Core;
using ProyectoNTierGUI.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ProyectoNTierGUI.ViewModel.Accounting
{
    using ProyectoNTierGUI.Model;
    using System;

    public class AccountsViewModel : INotifyPropertyChanged
    {
        private AccountingService _accountingService = ContainerProvider.Resolve<AccountingService>();
        private ObservableCollection<Account> _accounts = new();
        private ObservableCollection<AccountType> _accountTypes = new();
        public RelayCommand<Account> DeleteCommand { get; set; }
        public RelayCommand<Account> UpdateCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        private string _searchText = "";
        private Account _account = new()
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

        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        public Account Account
        {
            get => _account;
            set
            {
                _account = value;
                OnPropertyChanged(nameof(Account));
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

        public AccountsViewModel()
        {
            CreateCommand = new RelayCommand(CreateAccount);
            DeleteCommand = new RelayCommand<Account>(DeleteAccount);
            UpdateCommand = new RelayCommand<Account>(UpdateAccount);
            SearchCommand = new RelayCommand(SearchAccount);
            AccountTypes = new ObservableCollection<AccountType>(_accountingService.getTypes());

            var accounts = _accountingService.getAccounts();

            foreach (var account in accounts)
            {
                account.AccountType = _accountTypes.FirstOrDefault(type => type.Id == account.AccountType?.Id);
            }

            Accounts = new ObservableCollection<Account>(accounts);
        }

        public void CreateAccount()
        {
            Console.WriteLine(Account.Name);
            if (Account.Name.Trim() == "" || Account.AccountType == null || Account.AccountType.Id == 0)
            {
                return;
            }

            _accountingService.AddAccount(Account);

            var accounts = _accountingService.getAccounts();

            foreach (var a in accounts)
            {
                a.AccountType = _accountTypes.FirstOrDefault(type => type.Id == a.AccountType?.Id);
            }

            Accounts = new ObservableCollection<Account>(accounts);
        }

        private void DeleteAccount(Account account)
        {
            _accountingService.DeleteAccount(account);

            var accounts = _accountingService.getAccounts();

            foreach (var a in accounts)
            {
                a.AccountType = _accountTypes.FirstOrDefault(type => type.Id == a.AccountType?.Id);
            }

            Accounts = new ObservableCollection<Account>(accounts);
        }

        private void UpdateAccount(Account account)
        {
            _accountingService.UpdateAccount(account);

            var accounts = _accountingService.getAccounts();

            foreach (var a in accounts)
            {
                a.AccountType = _accountTypes.FirstOrDefault(type => type.Id == a.AccountType?.Id);
            }

            Accounts = new ObservableCollection<Account>(accounts);
        }

        private void SearchAccount()
        {
            var accounts = _accountingService.getAccounts();

            foreach (var a in accounts)
            {
                a.AccountType = _accountTypes.FirstOrDefault(type => type.Id == a.AccountType?.Id);
            }

            if (SearchText.Trim() == "")
            {
                Accounts = new(accounts);
                return;
            }


            Accounts = new(accounts.Where(w =>
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
