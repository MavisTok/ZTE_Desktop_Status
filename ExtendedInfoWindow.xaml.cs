using System;
using System.Windows;
using ZTE.Api;
using ZTE.Services;
using ZTE.ViewModels;

namespace ZTE
{
    /// <summary>
    /// Interaction logic for ExtendedInfoWindow.xaml
    /// </summary>
    public partial class ExtendedInfoWindow : Window
    {
        private readonly ExtendedInfoViewModel2 _viewModel;

        public ExtendedInfoWindow(UbusClient ubusClient)
        {
            InitializeComponent();

            // Create service and viewmodel
            var service = new ExtendedInfoService(ubusClient);
            _viewModel = new ExtendedInfoViewModel2(service);

            DataContext = _viewModel;

            // Start auto-refresh
            _viewModel.StartAutoRefresh();

            // Handle window closing
            Closing += (s, e) =>
            {
                if (_viewModel != null)
                    _viewModel.StopAutoRefresh();
            };
        }
    }
}
