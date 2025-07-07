using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using Microsoft.Win32;

namespace ЖКХ_Управление.Controls
{
    public partial class MastersControl : UserControl
    {
        private readonly string connectionString = "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";
        private int _selectedMasterId = -1;
        private List<Master> allMasters = new List<Master>();

        public MastersControl()
        {
            InitializeComponent();
            LoadMasters();
            LoadMasterCategories();
        }

        public class Master
        {
            public int сотрудник_id { get; set; }
            public string имя { get; set; }
            public string фамилия { get; set; }
            public string логин { get; set; }
            public string пароль { get; set; }
            public string категория { get; set; }
            public DateTime дата_приема { get; set; }
        }

        private void LoadMasters(bool showPasswords = false)
        {
            MastersDataGrid.ItemsSource = null;
            allMasters = new List<Master>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT s.сотрудник_id, s.имя, s.фамилия, s.логин, s.пароль,
                           k.название AS категория, s.дата_приема
                    FROM Сотрудники s
                    JOIN Категории_мастеров k ON s.категория_id = k.категория_id
                    WHERE s.должность_id = (SELECT должность_id FROM Должности WHERE название = 'Мастер')";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allMasters.Add(new Master
                        {
                            сотрудник_id = reader.GetInt32(0),
                            имя = reader.GetString(1),
                            фамилия = reader.GetString(2),
                            логин = reader.GetString(3),
                            пароль = showPasswords ? reader.GetString(4) : "*****",
                            категория = reader.GetString(5),
                            дата_приема = reader.GetDateTime(6)
                        });
                    }
                }
            }

            MastersDataGrid.ItemsSource = allMasters;
        }

        private void LoadMasterCategories()
        {
            MasterCategoryComboBox.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT категория_id, название FROM Категории_мастеров";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MasterCategoryComboBox.Items.Add(new KeyValuePair<int, string>(
                            reader.GetInt32(0), reader.GetString(1)
                        ));
                    }
                }
            }

            MasterCategoryComboBox.DisplayMemberPath = "Value";
            MasterCategoryComboBox.SelectedValuePath = "Key";
        }

        private void ShowMasterPasswordsCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            LoadMasters(ShowMasterPasswordsCheckBox.IsChecked == true);
        }

        private void MastersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MastersDataGrid.SelectedItem is Master selected)
            {
                _selectedMasterId = selected.сотрудник_id;
                MasterNameTextBox.Text = selected.имя;
                MasterSurnameTextBox.Text = selected.фамилия;
                MasterLoginTextBox.Text = selected.логин;
                MasterHireDatePicker.SelectedDate = selected.дата_приема;
                MasterPasswordBox.Password = "*****";

                foreach (var item in MasterCategoryComboBox.Items)
                {
                    if (((KeyValuePair<int, string>)item).Value == selected.категория)
                    {
                        MasterCategoryComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void AddMasterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MasterNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(MasterSurnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(MasterLoginTextBox.Text) ||
                string.IsNullOrWhiteSpace(MasterPasswordBox.Password) ||
                MasterCategoryComboBox.SelectedItem == null ||
                MasterHireDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (IsLoginExists(MasterLoginTextBox.Text))
            {
                MessageBox.Show("Логин уже занят!");
                return;
            }

            int categoryId = ((KeyValuePair<int, string>)MasterCategoryComboBox.SelectedItem).Key;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Сотрудники (имя, фамилия, логин, пароль, должность_id, категория_id, дата_приема)
                    VALUES (@имя, @фамилия, @логин, @пароль,
                            (SELECT должность_id FROM Должности WHERE название = 'Мастер'),
                            @категория_id, @дата_приема)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@имя", MasterNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@фамилия", MasterSurnameTextBox.Text);
                    cmd.Parameters.AddWithValue("@логин", MasterLoginTextBox.Text);
                    cmd.Parameters.AddWithValue("@пароль", HashPassword(MasterPasswordBox.Password));
                    cmd.Parameters.AddWithValue("@категория_id", categoryId);
                    cmd.Parameters.AddWithValue("@дата_приема", MasterHireDatePicker.SelectedDate);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadMasters();
            ClearMasterFields();
        }

        private void EditMasterButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMasterId == -1)
            {
                MessageBox.Show("Выберите мастера!");
                return;
            }

            if (IsLoginExists(MasterLoginTextBox.Text, _selectedMasterId))
            {
                MessageBox.Show("Такой логин уже существует!");
                return;
            }

            int categoryId = ((KeyValuePair<int, string>)MasterCategoryComboBox.SelectedItem).Key;
            string password = MasterPasswordBox.Password != "*****"
                ? HashPassword(MasterPasswordBox.Password)
                : GetCurrentPassword();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE Сотрудники
                                 SET имя=@имя, фамилия=@фамилия, логин=@логин, пароль=@пароль,
                                     категория_id=@категория_id, дата_приема=@дата
                                 WHERE сотрудник_id=@id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@имя", MasterNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@фамилия", MasterSurnameTextBox.Text);
                    cmd.Parameters.AddWithValue("@логин", MasterLoginTextBox.Text);
                    cmd.Parameters.AddWithValue("@пароль", password);
                    cmd.Parameters.AddWithValue("@категория_id", categoryId);
                    cmd.Parameters.AddWithValue("@дата", MasterHireDatePicker.SelectedDate);
                    cmd.Parameters.AddWithValue("@id", _selectedMasterId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadMasters();
            ClearMasterFields();
        }

        private void DeleteMasterButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMasterId == -1)
            {
                MessageBox.Show("Выберите мастера!");
                return;
            }

            var selected = allMasters.Find(m => m.сотрудник_id == _selectedMasterId);
            if (selected == null) return;

            var confirmWindow = new ConfirmDeleteWindow($"Удалить мастера '{selected.имя} {selected.фамилия}'?");
            if (confirmWindow.ShowDialog() != true)
                return;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Сотрудники WHERE сотрудник_id = @id", connection);
                cmd.Parameters.AddWithValue("@id", _selectedMasterId);
                cmd.ExecuteNonQuery();
            }

            LoadMasters();
            ClearMasterFields();
        }

        private void ExportMastersToExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = "Мастера.xlsx"
            };

            if (dlg.ShowDialog() == true)
            {
                using (var wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Мастера");
                    ws.Cell(1, 1).Value = "ID";
                    ws.Cell(1, 2).Value = "Имя";
                    ws.Cell(1, 3).Value = "Фамилия";
                    ws.Cell(1, 4).Value = "Логин";
                    ws.Cell(1, 5).Value = "Категория";
                    ws.Cell(1, 6).Value = "Дата приёма";

                    var list = MastersDataGrid.ItemsSource as List<Master>;
                    for (int i = 0; i < list.Count; i++)
                    {
                        ws.Cell(i + 2, 1).Value = list[i].сотрудник_id;
                        ws.Cell(i + 2, 2).Value = list[i].имя;
                        ws.Cell(i + 2, 3).Value = list[i].фамилия;
                        ws.Cell(i + 2, 4).Value = list[i].логин;
                        ws.Cell(i + 2, 5).Value = list[i].категория;
                        ws.Cell(i + 2, 6).Value = list[i].дата_приема.ToShortDateString();
                    }

                    wb.SaveAs(dlg.FileName);
                }

                MessageBox.Show("Экспорт завершён!");
            }
        }

        private void RefreshMastersButton_Click(object sender, RoutedEventArgs e)
        {
            LoadMasters();
            LoadMasterCategories();
        }

        private bool IsLoginExists(string login, int excludeId = -1)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Сотрудники WHERE логин = @логин AND сотрудник_id != @id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@логин", login);
                cmd.Parameters.AddWithValue("@id", excludeId);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private string GetCurrentPassword()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT пароль FROM Сотрудники WHERE сотрудник_id = @id", connection);
                cmd.Parameters.AddWithValue("@id", _selectedMasterId);
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

        private void ClearMasterFields()
        {
            MasterNameTextBox.Text = "";
            MasterSurnameTextBox.Text = "";
            MasterLoginTextBox.Text = "";
            MasterPasswordBox.Password = "";
            MasterCategoryComboBox.SelectedIndex = -1;
            MasterHireDatePicker.SelectedDate = null;
            _selectedMasterId = -1;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchTextBox.Text.ToLower();
            string selectedField = SearchFieldComboBox.SelectedItem as string;

            List<Master> filtered = allMasters;

            if (!string.IsNullOrWhiteSpace(query) && !string.IsNullOrEmpty(selectedField))
            {
                switch (selectedField)
                {
                    case "ID":
                        filtered = allMasters.FindAll(m => m.сотрудник_id.ToString().Contains(query));
                        break;
                    case "Имя":
                        filtered = allMasters.FindAll(m => m.имя.ToLower().Contains(query));
                        break;
                    case "Фамилия":
                        filtered = allMasters.FindAll(m => m.фамилия.ToLower().Contains(query));
                        break;
                    case "Логин":
                        filtered = allMasters.FindAll(m => m.логин.ToLower().Contains(query));
                        break;
                    case "Категория":
                        filtered = allMasters.FindAll(m => m.категория.ToLower().Contains(query));
                        break;
                }
            }

            MastersDataGrid.ItemsSource = filtered;
        }
    }
}
