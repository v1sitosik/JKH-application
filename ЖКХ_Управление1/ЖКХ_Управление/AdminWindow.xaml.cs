using System;
using System.Windows;
using System.Windows.Media;
using ЖКХ_Управление.Controls;
using System.Windows.Controls;


namespace ЖКХ_Управление
{
    public partial class AdminWindow : Window
    {
        private int _userId;
        private Button _activeNavButton;

        public AdminWindow(int userId)
        {
            InitializeComponent();
            MainContent.Content = new EmployeesControl(); // вкладка по умолчанию
            _userId = userId;
            _activeNavButton = EmployeesTab; // Назначь поле
            _activeNavButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));

        }

        private void EmployeesTab_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EmployeesControl();
            SetActiveNavButton(sender as Button);
        }

        private void MastersTab_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new MastersControl();
            SetActiveNavButton(sender as Button);
        }

        private void ClientsTab_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ClientsControl();
            SetActiveNavButton(sender as Button);
        }


        public interface IClosePopupControl
        {
            void CloseDatePickers();
        }
        private void NlpTemplatesTab_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new NlpTemplatesControl();
            SetActiveNavButton(sender as Button);
        }
        private void PositionsTab_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PositionsControl(); // Название контрола
            SetActiveNavButton(sender as Button);
        }

        private void CategoriesTab_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CategoriesControl();
            SetActiveNavButton(sender as Button);
        }

        private void NlpTab_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new NlpTemplatesControl();
            SetActiveNavButton(sender as Button);
        }

       

        private void SetActiveNavButton(Button newActiveButton)
        {
            if (_activeNavButton != null)
                _activeNavButton.ClearValue(Button.BackgroundProperty);

            _activeNavButton = newActiveButton;
            _activeNavButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243)); // синий #2196F3
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
