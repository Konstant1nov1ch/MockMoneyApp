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
        private User currentUser;
        private string username;
        private string password;
        private string token;
        private string login = "";
        private readonly IExternalAPIService externalAPIService;

        public User CurrentUser
        {
            get => currentUser;
            set => this.RaiseAndSetIfChanged(ref currentUser, value);
        }

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
        public string Login
        {
            get => login;
            set => this.RaiseAndSetIfChanged(ref login, value);
        }

        public string Token
        {
            get => token;
            set => this.RaiseAndSetIfChanged(ref token, value);
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenRegistrationCommand { get; }

        public MainWindowViewModel(IExternalAPIService externalAPIService)
        {
            this.externalAPIService = externalAPIService ?? throw new ArgumentNullException(nameof(externalAPIService));

            LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
            OpenRegistrationCommand = ReactiveCommand.Create(() => OpenRegistration());
            OpenRegistrationCommand.Subscribe();
        }

        private async Task LoginAsync()
        {
            Token = await externalAPIService.GetJWTTokenAsync(Login, Password);

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

                var registrationWindowViewModel = new RegistrationWindowViewModel(externalAPIService);
                var registrationWindow = new RegistrationWindow(externalAPIService)
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
