using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using MockMoney.Models;
using MockMoney.Views;
using MockMoney.Services;

namespace MockMoney.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private User _currentUser;
        private string _username;
        private string _password;
        private string _token;
        private string _login = "";
        private readonly IExternalAPIService _externalAPIService;

        public User CurrentUser
        {
            get => _currentUser;
            set => this.RaiseAndSetIfChanged(ref _currentUser, value);
        }

        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
        
        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        public string Token
        {
            get => _token;
            set => this.RaiseAndSetIfChanged(ref _token, value);
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenRegistrationCommand { get; }

        public MainWindowViewModel(IExternalAPIService externalAPIService)
        {
            _externalAPIService = externalAPIService ?? throw new ArgumentNullException(nameof(externalAPIService));

            LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
            OpenRegistrationCommand = ReactiveCommand.Create(() => OpenRegistration());
        }

        private async Task LoginAsync()
        {
            Token = await _externalAPIService.GetJWTTokenAsync(Login, Password);

            if (!string.IsNullOrEmpty(Token))
            {
                Console.WriteLine($"Login successful! Token: {Token}");
                CurrentUser = new User(Username, Login, Password)
                {
                    Token = Token
                };
                Console.WriteLine($": {CurrentUser}");
            }
            else
            {
                Console.WriteLine("Login failed. Invalid credentials.");
            }
        }

        private void OpenRegistration()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = (MainWindow)desktop.MainWindow;

                var registrationWindowViewModel = new RegistrationWindowViewModel(_externalAPIService);
                var registrationWindow = new RegistrationWindow(_externalAPIService)
                {
                    DataContext = registrationWindowViewModel
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
