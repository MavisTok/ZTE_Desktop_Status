using System;
using System.IO;
using System.Windows;
using ZTE.ViewModels;

namespace ZTE
{
    /// <summary>
    /// Interaction logic for JsonRpcWindow.xaml
    /// </summary>
    public partial class JsonRpcWindow : Window
    {
        public JsonRpcWindow()
        {
            InitializeComponent();

            // Set up the ViewModel
            var viewModel = new JsonRpcViewModel();
            this.DataContext = viewModel;

            // Load the HAR file
            LoadHarFile(viewModel);
        }

        private void LoadHarFile(JsonRpcViewModel viewModel)
        {
            try
            {
                // Get the path to the HAR file
                string projectPath = AppDomain.CurrentDomain.BaseDirectory;
                string harFilePath = Path.Combine(projectPath, "项目规划", "192.168.0.1.har");

                // Alternative: if running from source, try relative to solution
                if (!File.Exists(harFilePath))
                {
                    // Try to find it relative to the current directory
                    string currentDir = Directory.GetCurrentDirectory();
                    harFilePath = Path.Combine(currentDir, "项目规划", "192.168.0.1.har");
                }

                // Another alternative: look for it in the project directory
                if (!File.Exists(harFilePath))
                {
                    // Go up from bin/Debug to project root
                    DirectoryInfo binDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                    if (binDir.Parent?.Parent != null)
                    {
                        harFilePath = Path.Combine(binDir.Parent.Parent.FullName, "项目规划", "192.168.0.1.har");
                    }
                }

                if (File.Exists(harFilePath))
                {
                    viewModel.LoadHarFile(harFilePath);
                }
                else
                {
                    MessageBox.Show(
                        $"无法找到 HAR 文件。\n查找路径: {harFilePath}",
                        "文件未找到",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"加载 HAR 文件时出错:\n{ex.Message}",
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
