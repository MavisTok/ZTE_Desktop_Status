using System;
using System.Windows;
using System.Windows.Controls;
using ZTE.Api;
using ZTE.Services;
using ZTE.ViewModels;

namespace ZTE
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DashboardViewModel _viewModel;
        private UbusClient _ubusClient;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the application
            InitializeApp();
        }

        private void InitializeApp()
        {
            try
            {
                // Load configuration from AppSettings
                string routerUrl = AppSettings.RouterUrl;
                string session = AppSettings.SessionToken;
                int timeoutSeconds = AppSettings.RequestTimeoutSeconds;

                // Create UbusClient
                _ubusClient = new UbusClient(routerUrl, session, timeoutSeconds);

                // Create SessionManager (for future session management)
                var sessionManager = new SessionManager(_ubusClient);
                sessionManager.InitializeWithFixedSession(session);

                // Create Dashboard Service
                var dashboardService = new HomeDashboardService(_ubusClient);

                // Create and set ViewModel
                _viewModel = new DashboardViewModel(dashboardService, sessionManager);
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
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to initialize application:\n{ex.Message}",
                    "Initialization Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void OnLoginPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null)
                return;

            if (sender is PasswordBox passwordBox)
                _viewModel.Password = passwordBox.Password;
        }

        private void OnExtendedInfoClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_ubusClient != null)
                {
                    var extendedInfoWindow = new ExtendedInfoWindow(_ubusClient);
                    extendedInfoWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show(
                        "无法获取 API 客户端实例",
                        "错误",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"打开扩展信息窗口时出错:\n{ex.Message}",
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
