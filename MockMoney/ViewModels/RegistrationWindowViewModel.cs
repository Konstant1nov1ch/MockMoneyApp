using ReactiveUI;
using System;
using System.Reactive;

namespace MockMoney.ViewModels
{
    public partial class RegistrationWindowViewModel : ViewModelBase
    {
        private string username;
        private string password;
        private bool agreementAccepted;

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

        public bool AgreementAccepted
        {
            get => agreementAccepted;
            set => this.RaiseAndSetIfChanged(ref agreementAccepted, value);
        }

        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }

        public RegistrationWindowViewModel()
        {
            RegisterCommand = ReactiveCommand.Create(Register, CanRegister());
        }

        private void Register()
        {
            if (AgreementAccepted)
            {
                // Обработка регистрации
                Console.WriteLine($"Registered: Username: {Username}, Password: {Password}");
            }
            else
            {
                Console.WriteLine("Please accept the agreement to register.");
            }
        }

        private IObservable<bool> CanRegister() =>
            this.WhenAnyValue(
                x => x.Username,
                x => x.Password,
                x => x.AgreementAccepted,
                (username, password, agreementAccepted) =>
                    !string.IsNullOrWhiteSpace(username) &&
                    !string.IsNullOrWhiteSpace(password) &&
                    agreementAccepted
            );
    }
}
