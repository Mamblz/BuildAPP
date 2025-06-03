using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DesktopProgram.ViewModels
{
    public class ChangeEmailViewModel : INotifyPropertyChanged
    {
        private string _newEmail;
        private string _currentEmail;

        public string CurrentEmail
        {
            get => _currentEmail;
            set => SetProperty(ref _currentEmail, value);
        }

        public string NewEmail
        {
            get => _newEmail;
            set
            {
                if (SetProperty(ref _newEmail, value))
                {
                    OnPropertyChanged(nameof(IsPlaceholderVisible));
                }
            }
        }

        public bool IsPlaceholderVisible => string.IsNullOrWhiteSpace(NewEmail);

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action RequestClose;
        public bool? DialogResult { get; private set; }

        public Action<string, string> ShowMessage { get; set; } = (msg, title) =>
            System.Windows.MessageBox.Show(msg, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

        public ChangeEmailViewModel(string currentEmail)
        {
            CurrentEmail = currentEmail;

            SaveCommand = new RelayCommand(_ => ExecuteSave());
            CancelCommand = new RelayCommand(_ => ExecuteCancel());
        }

        private void ExecuteSave()
        {
            if (string.IsNullOrWhiteSpace(NewEmail) || !NewEmail.Contains("@"))
            {
                ShowMessage?.Invoke("Введите корректный Email.", "Ошибка");
                return;
            }

            ShowMessage?.Invoke("Email успешно изменён.", "Успех");
            DialogResult = true;
            RequestClose?.Invoke();
        }

        private void ExecuteCancel()
        {
            DialogResult = false;
            RequestClose?.Invoke();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        #endregion
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}
