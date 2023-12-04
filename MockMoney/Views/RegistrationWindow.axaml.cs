using Avalonia.Controls;
using MockMoney.Services;

namespace MockMoney.Views
{
    public partial class RegistrationWindow : Window
    {
        private readonly IExternalAPIService _externalAPIService;

        public RegistrationWindow(IExternalAPIService externalAPIService)
        {
            _externalAPIService = externalAPIService;
            InitializeComponent();
        }
    }
}