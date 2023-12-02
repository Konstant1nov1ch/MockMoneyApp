using ReactiveUI;
using System.Threading.Tasks;
using System;
using System.Reactive;
using MockMoney.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace MockMoney.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private string username;
        private string password;

        public string Username
        {
            get => username;
            set => this.RaiseAndSetIfChanged(ref username, value);
        }

        public string Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref password, value);
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenRegistrationCommand { get; }

        public MainWindowViewModel()
        {
            LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
            OpenRegistrationCommand = ReactiveCommand.Create(OpenRegistration);
        }

        private async Task LoginAsync()
        {
            // Логика для входа
            Console.WriteLine($"Username: {Username}, Password: {Password}");
            // Возможно, здесь будете вызывать метод для проверки входа или обработки данных
        }

        private void OpenRegistration()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = (MainWindow)desktop.MainWindow;

                var registrationWindow = new RegistrationWindow
                {
                    DataContext = new RegistrationWindowViewModel()
                };

                registrationWindow.Closed += (_, _) =>
                {
                    mainWindow.DataContext = this;
                    mainWindow.Show();
                };

                registrationWindow.Show();
                mainWindow.Hide();
            }
        }
    }
}
