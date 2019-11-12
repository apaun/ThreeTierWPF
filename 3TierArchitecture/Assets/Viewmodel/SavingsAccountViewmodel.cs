
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Assets.DAL;
using Assets.Model;
using ViewmodelBase;

namespace Assets.Viewmodel
{
    public sealed class SavingsAccountViewmodel : ViewModelBase
    {
        private enum Operation
        {
            AddNewAccount,
            UpdateAccountDetails,
            DeleteAccount,
            None
        }

        private Operation _currentOperation;
        private bool _isAccountDetailsEnabled;
        private string _accountNumber;
        private string _bank;
        private string _branch;
        private float _amount;
        private string _ifscCode;
        private string _accountType;
        private Account _selectedAccount;

        public bool IsAccountDetailsEnabled
        {
            get { return _isAccountDetailsEnabled; }
            set { _isAccountDetailsEnabled = value; OnPropertyChanged(); }
        }

        public string AccountNumber
        {
            get { return _accountNumber; }
            set { _accountNumber = value; OnPropertyChanged(); }
        }

        public string Bank
        {
            get { return _bank; }
            set { _bank = value; OnPropertyChanged(); }
        }

        public string Branch
        {
            get { return _branch; }
            set
            {
                _branch = value;
                OnPropertyChanged();
            }
        }

        public float Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged(); }
        }

        public string IfscCode
        {
            get { return _ifscCode; }
            set { _ifscCode = value; OnPropertyChanged(); }
        }

        public string AccountType
        {
            get { return _accountType; }
            set { _accountType = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Account> AccountList { get; set; }

        public ObservableCollection<string> AccountTypeList { get; set; }

        public Account SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;

                OnPropertyChanged();
            }
        }

        public ICommand AddNewAccountCommand => new RelayCommand(OnAddNewAccount, () => !IsAccountDetailsEnabled);

        public ICommand UpdateAccountCommand => new RelayCommand(OnUpdateAccount, () => !IsAccountDetailsEnabled && SelectedAccount != null);

        public ICommand DeleteAccountCommand => new RelayCommand(OnDeleteAccount, () => !IsAccountDetailsEnabled && SelectedAccount != null);

        public ICommand SaveCommand => new RelayCommand(OnSave, () => IsAccountDetailsEnabled);

        public ICommand CancelCommand => new RelayCommand(OnCancel, () => IsAccountDetailsEnabled);

        public ICommand RefreshCommand => new RelayCommand(OnRefresh, () => !IsAccountDetailsEnabled);

        public SavingsAccountViewmodel()
        {
            AccountList = new ObservableCollection<Account>();
            AccountTypeList = new ObservableCollection<string> {""};
            foreach (var type in Enum.GetValues(typeof(AccountTypeEnum)).Cast<AccountTypeEnum>())
            {
                AccountTypeList.Add(type.ToString());
            }
            AccountType = AccountTypeList[0];
            _currentOperation = Operation.None;
            IsAccountDetailsEnabled = false;
        }

        private void OnAddNewAccount()
        {
            AccountNumber = string.Empty;
            Bank = string.Empty;
            Branch = string.Empty;
            IfscCode = string.Empty;
            Amount = 0;
            AccountType = AccountTypeList[0];
            IsAccountDetailsEnabled = true;
            _currentOperation = Operation.AddNewAccount;
        }

        private void OnUpdateAccount()
        {
            if (_selectedAccount != null)
            {
                AccountNumber = _selectedAccount.AccountNumber;
                Bank = _selectedAccount.Bank;
                Branch = _selectedAccount.Branch;
                IfscCode = _selectedAccount.IfscCode;
                Amount = _selectedAccount.Amount;
                AccountType = _selectedAccount.TypeEnum.ToString();
                IsAccountDetailsEnabled = true;
                _currentOperation = Operation.UpdateAccountDetails;
            }
        }

        private void OnDeleteAccount()
        {
            //AccountList.Remove(SelectedAccount);
            IsAccountDetailsEnabled = true;
            _currentOperation = Operation.DeleteAccount;
        }

        private void OnSave()
        {
            try
            {
                switch (_currentOperation)
                {
                    case Operation.AddNewAccount:
                        var account = new Account(AccountNumber, Bank, Branch, Amount, IfscCode, (AccountTypeEnum)Enum.Parse(typeof(AccountTypeEnum), AccountType));
                        Dal.AddAccount(account);
                        break;
                    case Operation.UpdateAccountDetails:
                        var updateAccount = new Account(AccountNumber, Bank, Branch, Amount, IfscCode, (AccountTypeEnum)Enum.Parse(typeof(AccountTypeEnum), AccountType));
                        Dal.Update(updateAccount);
                        break;
                    case Operation.DeleteAccount:
                        Dal.Delete(SelectedAccount);
                        break;
                    case Operation.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Assets", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _currentOperation = Operation.None;
                IsAccountDetailsEnabled = false;
            }
        }

        private void OnCancel()
        {
            AccountNumber = string.Empty;
            Bank = string.Empty;
            Branch = string.Empty;
            IfscCode = string.Empty;
            Amount = 0;
            _currentOperation = Operation.None;
            IsAccountDetailsEnabled = false;
        }

        private void OnRefresh()
        {
            try
            {
                AccountList.Clear();
                foreach (var account in Dal.GetAccounts())
                {
                    AccountList.Add(account);
                }
                OnPropertyChanged(nameof(AccountList));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Assets", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
