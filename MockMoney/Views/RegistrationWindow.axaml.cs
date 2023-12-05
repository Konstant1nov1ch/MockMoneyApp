using Avalonia.Controls;
    using MockMoney.Services;

namespace MockMoney.Views
{
    public partial class RegistrationWindow : Window {
        
        private readonly IExternalAPIService _externalAPIService;

        public RegistrationWindow(IExternalAPIService externalAPIService)
        {
            if (Avalonia.Controls.Design.IsDesignMode)
            {
                // Инициализация фиктивных данных для дизайнера
                InitializeComponent();
                return;
            }
            else
            {

                _externalAPIService = externalAPIService;
                InitializeComponent();
            }
        }
    }
}