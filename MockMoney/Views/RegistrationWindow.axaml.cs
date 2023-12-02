using Avalonia.Markup.Xaml;
using MockMoney.ViewModels;
using Avalonia.Controls;

namespace MockMoney.Views
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            DataContext = new RegistrationWindowViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}