using Avalonia.Controls;
using MockMoney.ViewModels;

namespace MockMoney.Views
{
    public partial class FirstWindow : Window
    {
        public FirstWindow()
        {
            if (Avalonia.Controls.Design.IsDesignMode)
            {
                InitializeComponent();
                return;
            }

            InitializeComponent();
            DataContext = new FirstWindowViewModel();
        }
    }

}