using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using Microsoft.Win32;

namespace ЖКХ_Управление.Controls
{
    public partial class EmployeesControl : UserControl
    {
        private readonly string connectionString = "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";
        private int _selectedEmployeeId = -1;

        public EmployeesControl()
        {
            InitializeComponent();
            LoadEmployees();
            LoadPositions();
        }

        public class Employee
        {
            public int сотрудник_id { get; set; }
            public string имя { get; set; }
            public string фамилия { get; set; }
            public string логин { get; set; }
            public string пароль { get; set; }
            public string должность { get; set; }
            public DateTime дата_приема { get; set; }
        }

        public class Position
        {
            public int Id { get; set; }
            public string Название { get; set; }

            public override string ToString()
            {
                return Название;
            }
        }

        private void CheckPasswordComplexity(string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("CheckPasswordComplexity", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    command.ExecuteNonQuery(); // Успешно — идём дальше
                }
                catch (SqlException ex)
                {
                    throw new Exception("Пароль не соответствует требованиям: " + ex.Message);
                }
            }
        }

        private void LoadEmployees(bool showPasswords = false)
        {
            EmployeesDataGrid.ItemsSource = null;
            allEmployees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
        SELECT s.сотрудник_id, s.имя, s.фамилия, s.логин, s.пароль,
               d.название AS должность, s.дата_приема
        FROM Сотрудники s
        JOIN Должности d ON s.должность_id = d.должность_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allEmployees.Add(new Employee
                        {
                            сотрудник_id = reader.GetInt32(0),
                            имя = reader.GetString(1),
                            фамилия = reader.GetString(2),
                            логин = reader.GetString(3),
                            пароль = showPasswords ? reader.GetString(4) : "*****",
                            должность = reader.GetString(5),
                            дата_приема = reader.GetDateTime(6)
                        });
                    }
                }
            }

            EmployeesDataGrid.ItemsSource = allEmployees;
        }

        private void LoadPositions()
        {
            var positions = new List<Position>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT должность_id, название FROM Должности";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        positions.Add(new Position
                        {
                            Id = reader.GetInt32(0),
                            Название = reader.GetString(1)
                        });
                    }
                }
            }

            PositionComboBox.ItemsSource = positions;
            PositionComboBox.DisplayMemberPath = "Название";
            PositionComboBox.SelectedValuePath = "Id";
        }

        private void ShowPasswordsCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            LoadEmployees(ShowPasswordsCheckBox.IsChecked == true);
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem is Employee selected)
            {
                _selectedEmployeeId = selected.сотрудник_id;
                NameTextBox.Text = selected.имя;
                SurnameTextBox.Text = selected.фамилия;
                LoginTextBox.Text = selected.логин;
                PasswordBox.Password = "*****";
                HireDatePicker.SelectedDate = selected.дата_приема;

                foreach (var item in PositionComboBox.Items)
                {
                    var pos = item as Position;
                    if (pos != null && pos.Название == selected.должность)
                    {
                        PositionComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(SurnameTextBox.Text) ||
                string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordBox.Password) ||
                PositionComboBox.SelectedItem == null || HireDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (IsLoginExists(LoginTextBox.Text))
            {
                MessageBox.Show("Логин уже занят!");
                return;
            }

            var selectedPosition = PositionComboBox.SelectedItem as Position;
            if (selectedPosition == null)
                return;

            try
            {
                // Проверка сложности пароля через SQL-процедуру
                CheckPasswordComplexity(PasswordBox.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string hashedPassword = HashPassword(PasswordBox.Password);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Сотрудники (имя, фамилия, логин, пароль, должность_id, дата_приема)
                         VALUES (@имя, @фамилия, @логин, @пароль, @должность_id, @дата_приема)";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@имя", NameTextBox.Text);
                    cmd.Parameters.AddWithValue("@фамилия", SurnameTextBox.Text);
                    cmd.Parameters.AddWithValue("@логин", LoginTextBox.Text);
                    cmd.Parameters.AddWithValue("@пароль", hashedPassword);
                    cmd.Parameters.AddWithValue("@должность_id", selectedPosition.Id);
                    cmd.Parameters.AddWithValue("@дата_приема", HireDatePicker.SelectedDate);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadEmployees();
        }

        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployeeId == -1)
            {
                MessageBox.Show("Выберите сотрудника!");
                return;
            }

            var selectedPosition = PositionComboBox.SelectedItem as Position;
            if (selectedPosition == null)
                return;

            string rawPassword = PasswordBox.Password;
            string finalPassword;

            if (rawPassword != "*****")
            {
                try
                {
                    // Проверка сложности нового пароля
                    CheckPasswordComplexity(rawPassword);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                finalPassword = HashPassword(rawPassword);
            }
            else
            {
                finalPassword = GetCurrentPassword(); // пароль не изменён
            }

            if (IsLoginExists(LoginTextBox.Text, _selectedEmployeeId))
            {
                MessageBox.Show("Такой логин уже существует!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE Сотрудники 
                         SET имя=@имя, фамилия=@фамилия, логин=@логин, пароль=@пароль, 
                             должность_id=@должность_id, дата_приема=@дата
                         WHERE сотрудник_id=@id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@имя", NameTextBox.Text);
                    cmd.Parameters.AddWithValue("@фамилия", SurnameTextBox.Text);
                    cmd.Parameters.AddWithValue("@логин", LoginTextBox.Text);
                    cmd.Parameters.AddWithValue("@пароль", finalPassword);
                    cmd.Parameters.AddWithValue("@должность_id", selectedPosition.Id);
                    cmd.Parameters.AddWithValue("@дата", HireDatePicker.SelectedDate);
                    cmd.Parameters.AddWithValue("@id", _selectedEmployeeId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadEmployees();
        }

        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployeeId == -1)
            {
                MessageBox.Show("Выберите сотрудника для удаления!");
                return;
            }

            var selected = allEmployees.Find(emp => emp.сотрудник_id == _selectedEmployeeId);
            if (selected == null) return;

            var confirmWindow = new ConfirmDeleteWindow($"Удалить сотрудника '{selected.имя} {selected.фамилия}'?");
            if (confirmWindow.ShowDialog() != true)
                return;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Сотрудники WHERE сотрудник_id=@id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", _selectedEmployeeId);
                    cmd.ExecuteNonQuery();
                }
            }

            _selectedEmployeeId = -1;
            LoadEmployees();
        }

        private void ExportEmployeesToExcel_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "Excel (*.xlsx)|*.xlsx", FileName = "Сотрудники.xlsx" };
            if (dlg.ShowDialog() == true)
            {
                using (var wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Сотрудники");
                    ws.Cell(1, 1).Value = "ID";
                    ws.Cell(1, 2).Value = "Имя";
                    ws.Cell(1, 3).Value = "Фамилия";
                    ws.Cell(1, 4).Value = "Логин";
                    ws.Cell(1, 5).Value = "Должность";
                    ws.Cell(1, 6).Value = "Дата приёма";

                    var list = EmployeesDataGrid.ItemsSource as List<Employee>;
                    for (int i = 0; i < list.Count; i++)
                    {
                        ws.Cell(i + 2, 1).Value = list[i].сотрудник_id;
                        ws.Cell(i + 2, 2).Value = list[i].имя;
                        ws.Cell(i + 2, 3).Value = list[i].фамилия;
                        ws.Cell(i + 2, 4).Value = list[i].логин;
                        ws.Cell(i + 2, 5).Value = list[i].должность;
                        ws.Cell(i + 2, 6).Value = list[i].дата_приема.ToShortDateString();
                    }

                    wb.SaveAs(dlg.FileName);
                }

                MessageBox.Show("Экспорт завершён!");
            }
        }

        private bool IsLoginExists(string login, int excludeId = -1)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM Сотрудники WHERE логин=@логин AND сотрудник_id!=@id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@логин", login);
                    cmd.Parameters.AddWithValue("@id", excludeId);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        private string GetCurrentPassword()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT пароль FROM Сотрудники WHERE сотрудник_id=@id", connection);
                cmd.Parameters.AddWithValue("@id", _selectedEmployeeId);
                return cmd.ExecuteScalar()?.ToString();
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
        private void LoadEmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployees(ShowPasswordsCheckBox.IsChecked == true);
        }
        private List<Employee> allEmployees = new List<Employee>();

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchTextBox.Text.ToLower();
            string selectedField = SearchFieldComboBox.SelectedItem as string;

            List<Employee> filtered = allEmployees;

            if (!string.IsNullOrWhiteSpace(query) && !string.IsNullOrEmpty(selectedField))
            {
                switch (selectedField)
                {
                    case "ID":
                        filtered = allEmployees.FindAll(emp => emp.сотрудник_id.ToString().Contains(query));
                        break;
                    case "Имя":
                        filtered = allEmployees.FindAll(emp => emp.имя.ToLower().Contains(query));
                        break;
                    case "Фамилия":
                        filtered = allEmployees.FindAll(emp => emp.фамилия.ToLower().Contains(query));
                        break;
                    case "Логин":
                        filtered = allEmployees.FindAll(emp => emp.логин.ToLower().Contains(query));
                        break;
                    case "Должность":
                        filtered = allEmployees.FindAll(emp => emp.должность.ToLower().Contains(query));
                        break;
                }
            }

            EmployeesDataGrid.ItemsSource = filtered;
        }


        public void CloseDatePickers()
        {
            if (HireDatePicker.IsDropDownOpen)
                HireDatePicker.IsDropDownOpen = false;
        }
    }
}
