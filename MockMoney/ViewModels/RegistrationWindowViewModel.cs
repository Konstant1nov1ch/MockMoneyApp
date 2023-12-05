using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MockMoney.Services;
using MockMoney.Views;
using ReactiveUI;

namespace MockMoney.ViewModels
{
    public partial class RegistrationWindowViewModel : ViewModelBase
    {
        private string _displayName;
        private string _username;
        private string _password;
        private bool _agreementAccepted;
        private readonly IExternalAPIService _externalAPIService;

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
            set => this.RaiseAndSetIfChanged(ref _agreementAccepted, value);
        }

        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }

        public RegistrationWindowViewModel(IExternalAPIService externalAPIService)
        {
            _externalAPIService = externalAPIService ?? throw new ArgumentNullException(nameof(externalAPIService));

            if (Avalonia.Controls.Design.IsDesignMode)
            {
                // Фиктивные данные для дизайнера
                DisplayName = "Design Mode User";
                Username = "design_user";
                Password = "design_password";
                AgreementAccepted = true;
            }
            else
            {
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
        }

        private async Task RegisterAsync()
        {
            try
            {
                if (Avalonia.Controls.Design.IsDesignMode)
                {
                    // Режим дизайна - добавьте нужную логику, если требуется
                }
                else if (AgreementAccepted)
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
