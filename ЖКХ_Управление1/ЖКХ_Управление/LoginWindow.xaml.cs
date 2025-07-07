using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace ЖКХ_Управление
{
    public partial class LoginWindow : Window
    {
        private readonly string connectionString =
            "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;
            ErrorTextBlock.Text = "";

            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowError("Введите логин и пароль!");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT s.сотрудник_id, s.должность_id, s.пароль, d.название AS должность
                        FROM Сотрудники s
                        JOIN Должности d ON s.должность_id = d.должность_id
                        WHERE s.логин = @login";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = Convert.ToInt32(reader["сотрудник_id"]);
                                string storedPassword = reader["пароль"].ToString();
                                string role = reader["должность"]?.ToString() ?? "";

                                if (VerifyPassword(password, storedPassword))
                                {
                                    OpenWindow(userId, role);
                                }
                                else
                                {
                                    ShowError("Неверный пароль!");
                                }
                            }
                            else
                            {
                                ShowError("Пользователь не найден!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка подключения к базе данных:\n" + ex.Message);
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] enteredBytes = Encoding.UTF8.GetBytes(enteredPassword);
                byte[] enteredHash = sha256.ComputeHash(enteredBytes);
                string enteredHashString = BitConverter.ToString(enteredHash).Replace("-", "").ToLower();

                return enteredHashString == storedHash;
            }
        }

        private void OpenWindow(int userId, string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Ошибка! Роль пользователя не определена. Проверьте данные в базе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Window nextWindow = null;

            if (role == "Администратор")
                nextWindow = new AdminWindow(userId);
            else if (role == "Оператор")
                nextWindow = new EmployeeWindow(userId);
            else if (role == "Мастер")
                nextWindow = new MasterWindow(userId);


            if (nextWindow != null)
            {
                nextWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Неизвестная роль: {role}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }
    }
}
