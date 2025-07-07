using System.Windows;

namespace ЖКХ_Управление
{
    public partial class WarningDialogWindow : Window
    {
        public WarningDialogWindow(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public static void Show(string message, Window owner = null)
        {
            var dialog = new WarningDialogWindow(message)
            {
                Owner = owner
            };
            dialog.ShowDialog();
        }
    }
}
