using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using Microsoft.Win32;

namespace ЖКХ_Управление.Controls
{
    public partial class ClientsControl : UserControl
    {
        private readonly string connectionString = "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";
        private int _selectedClientId = -1;
        private List<Client> allClients = new List<Client>();

        public ClientsControl()
        {
            InitializeComponent();
            LoadClients();
        }

        public class Client
        {
            public int клиент_id { get; set; }
            public string имя { get; set; }
            public string фамилия { get; set; }
            public string телефон { get; set; }
            public string email { get; set; }
            public DateTime дата_регистрации { get; set; }
        }

        private void LoadClients()
        {
            ClientsDataGrid.ItemsSource = null;
            allClients.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT клиент_id, имя, фамилия, телефон, email, дата_регистрации FROM Клиенты";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allClients.Add(new Client
                        {
                            клиент_id = reader.GetInt32(0),
                            имя = reader.GetString(1),
                            фамилия = reader.GetString(2),
                            телефон = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            email = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            дата_регистрации = reader.GetDateTime(5)
                        });
                    }
                }
            }

            ClientsDataGrid.ItemsSource = allClients;
        }
        private void ExportClientsToExcel_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = "Клиенты.xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Клиенты");

                    // Заголовки
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Имя";
                    worksheet.Cell(1, 3).Value = "Фамилия";
                    worksheet.Cell(1, 4).Value = "Телефон";
                    worksheet.Cell(1, 5).Value = "Email";
                    worksheet.Cell(1, 6).Value = "Дата регистрации";

                    var list = ClientsDataGrid.ItemsSource as List<Client>;
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = list[i].клиент_id;
                        worksheet.Cell(i + 2, 2).Value = list[i].имя;
                        worksheet.Cell(i + 2, 3).Value = list[i].фамилия;
                        worksheet.Cell(i + 2, 4).Value = list[i].телефон;
                        worksheet.Cell(i + 2, 5).Value = list[i].email;
                        worksheet.Cell(i + 2, 6).Value = list[i].дата_регистрации.ToShortDateString();
                    }

                    workbook.SaveAs(dialog.FileName);
                }

                MessageBox.Show("Экспорт завершён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClientId == -1)
            {
                MessageBox.Show("Выберите клиента для удаления!");
                return;
            }

            var selectedClient = allClients.FirstOrDefault(c => c.клиент_id == _selectedClientId);
            if (selectedClient == null) return;

            var confirmWindow = new ConfirmDeleteWindow($"Удалить клиента '{selectedClient.имя} {selectedClient.фамилия}'?");
            if (confirmWindow.ShowDialog() != true)
                return;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Клиенты WHERE клиент_id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", _selectedClientId);
                    command.ExecuteNonQuery();
                }
            }

            _selectedClientId = -1;
            ClearClientFields();
            LoadClients();
        }

        private void ClearClientFields()
        {
            ClientNameTextBox.Text = "";
            ClientSurnameTextBox.Text = "";
            ClientPhoneTextBox.Text = "";
            ClientEmailTextBox.Text = "";
            _selectedClientId = -1;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchTextBox.Text.ToLower();
            string selectedField = SearchFieldComboBox.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(query) || string.IsNullOrEmpty(selectedField))
            {
                ClientsDataGrid.ItemsSource = allClients;
                return;
            }

            var filtered = allClients;

            switch (selectedField)
            {
                case "ID":
                    filtered = allClients.Where(c => c.клиент_id.ToString().Contains(query)).ToList();
                    break;
                case "Имя":
                    filtered = allClients.Where(c => c.имя.ToLower().Contains(query)).ToList();
                    break;
                case "Фамилия":
                    filtered = allClients.Where(c => c.фамилия.ToLower().Contains(query)).ToList();
                    break;
                case "Телефон":
                    filtered = allClients.Where(c => c.телефон.ToLower().Contains(query)).ToList();
                    break;
                case "Email":
                    filtered = allClients.Where(c => c.email.ToLower().Contains(query)).ToList();
                    break;
            }

            ClientsDataGrid.ItemsSource = filtered;
        }

        private void ClientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Client selected)
            {
                _selectedClientId = selected.клиент_id;
                ClientNameTextBox.Text = selected.имя;
                ClientSurnameTextBox.Text = selected.фамилия;
                ClientPhoneTextBox.Text = selected.телефон;
                ClientEmailTextBox.Text = selected.email;
                RegistrationDatePicker.SelectedDate = selected.дата_регистрации;
            }
        }

        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClientId == -1)
            {
                MessageBox.Show("Выберите клиента!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Клиенты 
                    SET имя = @имя, фамилия = @фамилия, телефон = @телефон, email = @email, дата_регистрации = @дата
                    WHERE клиент_id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@имя", ClientNameTextBox.Text);
                    command.Parameters.AddWithValue("@фамилия", ClientSurnameTextBox.Text);
                    command.Parameters.AddWithValue("@телефон", string.IsNullOrWhiteSpace(ClientPhoneTextBox.Text) ? (object)DBNull.Value : ClientPhoneTextBox.Text);
                    command.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(ClientEmailTextBox.Text) ? (object)DBNull.Value : ClientEmailTextBox.Text);
                    command.Parameters.AddWithValue("@дата", RegistrationDatePicker.SelectedDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@id", _selectedClientId);
                    command.ExecuteNonQuery();
                }
            }

            LoadClients();
            MessageBox.Show("Клиент обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadClients();
        }
    }
}
