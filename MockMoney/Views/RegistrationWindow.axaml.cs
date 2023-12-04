using Avalonia.Controls;
using MockMoney.Services;
using MockMoney.ViewModels;

namespace MockMoney.Views
{
    public partial class RegistrationWindow : Window
    {
        private readonly IExternalAPIService externalAPIService;

        public RegistrationWindow(IExternalAPIService externalAPIService)
        {
            InitializeComponent();
            DataContext = new RegistrationWindowViewModel(externalAPIService);
        }
    }
}
