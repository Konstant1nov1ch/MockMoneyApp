using System;
using Avalonia;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MockMoney.Services;
using Avalonia.Controls.ApplicationLifetimes;
using MockMoney.Views;
using ReactiveUI;

namespace MockMoney.ViewModels
{
    public partial class RegistrationWindowViewModel : ViewModelBase
    {
        private readonly IExternalAPIService _externalAPIService;

        private string _displayName;
        private string _username;
        private string _password;
        private bool _agreementAccepted;

        public string DisplayName
        {
            get => _displayName;
            set => this.RaiseAndSetIfChanged(ref _displayName, value);
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

        public bool AgreementAccepted
        {
            get => _agreementAccepted;
            set
            {
                this.RaiseAndSetIfChanged(ref _agreementAccepted, value);
            }
        }

        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }

        public RegistrationWindowViewModel(IExternalAPIService externalAPIService)
        {
            _externalAPIService = externalAPIService ?? throw new ArgumentNullException(nameof(externalAPIService));

            var canRegister = this.WhenAnyValue(
                x => x.Username,
                x => x.Password,
                x => x.DisplayName,
                x => x.AgreementAccepted,
                (_, _, _, accepted) =>
                    !string.IsNullOrWhiteSpace(Username) &&
                    !string.IsNullOrWhiteSpace(Password) &&
                    !string.IsNullOrWhiteSpace(DisplayName) &&
                    accepted
            );

            RegisterCommand = ReactiveCommand.CreateFromTask(
                RegisterAsync,
                canRegister
            );

            RegisterCommand.Subscribe(_ => CloseWindow());
        }

        private async Task RegisterAsync()
        {
            try
            {
                if (AgreementAccepted)
                {
                    string registrationResult = await _externalAPIService.RegisterAsync(DisplayName, Username, Password);

                    Console.WriteLine(registrationResult); // Вывод результата регистрации

                    // Дополнительные действия после успешной регистрации
                }
                else
                {
                    Console.WriteLine("Please accept the agreement to register.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during registration: {ex.Message}");
                // Дополнительная обработка ошибок при регистрации
            }
        }

        private void CloseWindow()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                foreach (var window in desktop.Windows)
                {
                    if (window is RegistrationWindow registrationWindow)
                    {
                        registrationWindow.Close();
                        break;
                    }
                }
            }
        }
    }
}
