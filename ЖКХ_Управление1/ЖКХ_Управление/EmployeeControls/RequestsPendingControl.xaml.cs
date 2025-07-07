using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using ЖКХ_Управление.Models;
using ЖКХ_Управление.Views;

namespace ЖКХ_Управление.EmployeeControls
{
    public partial class RequestsPendingControl : UserControl
    {
        private readonly string connectionString = "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";
        private List<Request> pendingRequests = new List<Request>();

        public RequestsPendingControl()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            pendingRequests.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                SELECT 
                    z.запрос_id, 
                    k.имя, 
                    k.фамилия, 
                    a.адрес, 
                    z.текст, 
                    z.категория
                FROM Запросы z
                JOIN Клиенты k ON z.клиент_id = k.клиент_id
                LEFT JOIN Адреса a ON z.адрес_id = a.адрес_id
                WHERE z.статус = 'новый'";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pendingRequests.Add(new Request
                        {
                            запрос_id = reader.GetInt32(0),
                            имя = reader.GetString(1),
                            фамилия = reader.GetString(2),
                            адрес = reader.IsDBNull(3) ? "Не указан" : reader.GetString(3),
                            текст = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            категория = reader.IsDBNull(5) ? "" : reader.GetString(5)
                        });
                    }
                }
            }

            PendingRequestsGrid.ItemsSource = pendingRequests;
        }

        private void AssignMasterButton_Click(object sender, RoutedEventArgs e)
        {
            if (PendingRequestsGrid.SelectedItem is Request selectedRequest)
            {
                var window = new MasterSelectionWindow(selectedRequest);
                window.ShowDialog();
                LoadRequests(); // Обновляем после назначения
            }
            else
            {
                var error = new ErrorDialogWindow("Выберите заявку для назначения мастера!");
                error.ShowDialog();
            }
        }
    }
}
