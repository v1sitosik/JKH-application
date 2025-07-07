using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ЖКХ_Управление.EmployeeControls;

namespace ЖКХ_Управление
{
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow(int userId)
        {
            InitializeComponent();
            LoadPendingTab(); // По умолчанию при открытии
        }

        private void LoadPendingTab()
        {
            MainContent.Content = new RequestsPendingControl();
            HighlightActiveTab(PendingTab);
        }

        private void LoadInProgressTab()
        {
            MainContent.Content = new RequestsInProgressControl();
            HighlightActiveTab(InProgressTab);
        }

        private void LoadCompletedTab()
        {
            MainContent.Content = new RequestsCompletedControl();
            HighlightActiveTab(CompletedTab);
        }

        private void PendingTab_Click(object sender, RoutedEventArgs e)
        {
            LoadPendingTab();
        }

        private void InProgressTab_Click(object sender, RoutedEventArgs e)
        {
            LoadInProgressTab();
        }

        private void CompletedTab_Click(object sender, RoutedEventArgs e)
        {
            LoadCompletedTab();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void HighlightActiveTab(Button activeButton)
        {
            // Сброс фона всех кнопок
            PendingTab.Background = Brushes.Transparent;
            InProgressTab.Background = Brushes.Transparent;
            CompletedTab.Background = Brushes.Transparent;

            // Подсветка активной вкладки
            activeButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243)); // #2196F3
        }
    }
}
