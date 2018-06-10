using System;

using MVVMApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MVVMApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
