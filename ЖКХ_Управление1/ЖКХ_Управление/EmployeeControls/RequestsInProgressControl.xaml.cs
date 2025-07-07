using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using ЖКХ_Управление.Models;
using ЖКХ_Управление.Views;

namespace ЖКХ_Управление.EmployeeControls
{
    public partial class RequestsInProgressControl : UserControl
    {
        private readonly string connectionString = "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";
        private List<Request> inProgressRequests = new List<Request>();

        public RequestsInProgressControl()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            inProgressRequests.Clear();

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
                    z.категория,
                    COALESCE(nz.статус, 'в работе') AS этап,
                    COALESCE(s.имя + ' ' + s.фамилия, 'Не назначен') AS мастер
                FROM Запросы z
                JOIN Клиенты k ON z.клиент_id = k.клиент_id
                LEFT JOIN Адреса a ON z.адрес_id = a.адрес_id
                LEFT JOIN Назначения nz ON z.запрос_id = nz.запрос_id
                LEFT JOIN Сотрудники s ON nz.сотрудник_id = s.сотрудник_id
                WHERE z.статус = 'в процессе'";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inProgressRequests.Add(new Request
                        {
                            запрос_id = reader.GetInt32(0),
                            имя = reader.GetString(1),
                            фамилия = reader.GetString(2),
                            адрес = reader.IsDBNull(3) ? "Не указан" : reader.GetString(3),
                            текст = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            категория = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            этап = reader.IsDBNull(6) ? "в работе" : reader.GetString(6),
                            мастер = reader.IsDBNull(7) ? "Не назначен" : reader.GetString(7)
                        });
                    }
                }
            }

            InProgressRequestsGrid.ItemsSource = inProgressRequests;
        }

        private void ChangeMasterButton_Click(object sender, RoutedEventArgs e)
        {
            if (InProgressRequestsGrid.SelectedItem is Request selectedRequest)
            {
                var window = new MasterSelectionWindow(selectedRequest, isChange: true);
                window.ShowDialog();
                LoadRequests(); // Перезагрузка
            }
            else
            {
                new ErrorDialogWindow("Выберите заявку для замены мастера!").ShowDialog();
            }
        }
    }
}
