using System.Windows;

namespace ЖКХ_Управление
{
    public partial class ConfirmDeleteWindow : Window
    {
        public ConfirmDeleteWindow(string message)
        {
            InitializeComponent();
            ConfirmMessage.Text = message;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
