using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;

namespace MockMoney.ViewModels
{
    public class FirstWindowViewModel : ViewModelBase
    {
        private object currentContent;

        public object CurrentContent
        {
            get => currentContent;
            set => this.RaiseAndSetIfChanged(ref currentContent, value);
        }

        public ReactiveCommand<string, Unit> NavigateCommand { get; }

        public FirstWindowViewModel()
        {
            NavigateCommand = ReactiveCommand.Create<string>(ShowPage);
            ShowPage("Page1");
        }

        private void ShowPage(string param)
        {
            switch (param)
            {
                case "Page1":
                    CurrentContent = Application.Current.FindResource("Page1");
                    break;
                case "Page2":
                    CurrentContent = Application.Current.FindResource("Page2");
                    break;
                case "Page3":
                    CurrentContent = Application.Current.FindResource("Page3");
                    break;
            }
        }


    }
}