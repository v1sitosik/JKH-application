using System.Windows;

namespace ЖКХ_Управление.Views
{
    public partial class SuccessDialogWindow : Window
    {
        public SuccessDialogWindow(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        public SuccessDialogWindow(string title, string message)
        {
            InitializeComponent();
            this.Title = title;
            MessageText.Text = message;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static void Show(string title, string message, Window owner = null)
        {
            var dialog = new SuccessDialogWindow(title, message)
            {
                Owner = owner
            };
            dialog.ShowDialog();
        }
    }
}
