using System.Windows;

namespace ЖКХ_Управление.Views
{
    public partial class ErrorDialogWindow : Window
    {
        public ErrorDialogWindow(string message)
        {
            InitializeComponent();
            MessageTextBlock.Text = message;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
