using System;
using System.Windows;

namespace ЖКХ_Управление
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Только запуск окна авторизации
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
