using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MockMoney.ViewModels;
using MockMoney.Views;
using MockMoney.Services;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Console;

namespace MockMoney
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            // Инициализация логгера
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Создаем экземпляр сервиса
                var externalAPIService = new ExternalAPIService(new System.Net.Http.HttpClient());

                // Передаем сервис в конструктор MainWindowViewModel
                var mainWindowViewModel = new MainWindowViewModel(externalAPIService);

                // Используем логгер для записи информации
                Log.Information("MainWindowViewModel created");

                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel,
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}