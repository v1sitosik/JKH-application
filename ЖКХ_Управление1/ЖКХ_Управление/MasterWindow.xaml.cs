using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ЖКХ_Управление.MasterControls;
using ЖКХ_Управление.Views;

namespace ЖКХ_Управление
{
    public partial class MasterWindow : Window
    {
        private readonly int мастерId;

        public MasterWindow(int мастерId)
        {
            InitializeComponent();
            this.мастерId = мастерId;
            LoadAssignmentsTab(); // как в EmployeeWindow
        }

        private void LoadAssignmentsTab()
        {
            MainContent.Content = new ActiveAssignmentsControl(мастерId);
            HighlightActiveTab(AssignmentsTab);
        }

        private void LoadCompletedTab()
        {
            MainContent.Content = new CompletedAssignmentsControl(мастерId);
            HighlightActiveTab(CompletedTab);
        }

        private void AssignmentsTab_Click(object sender, RoutedEventArgs e)
        {
            LoadAssignmentsTab();
        }

        private void CompletedTab_Click(object sender, RoutedEventArgs e)
        {
            LoadCompletedTab();
        }

        private void HighlightActiveTab(Button activeButton)
        {
            // Сброс фона
            AssignmentsTab.Background = Brushes.Transparent;
            CompletedTab.Background = Brushes.Transparent;

            // Подсветка активной кнопки
            activeButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243)); // #2196F3
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
