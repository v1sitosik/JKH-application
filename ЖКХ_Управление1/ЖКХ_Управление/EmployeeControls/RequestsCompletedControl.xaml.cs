using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows;

namespace ЖКХ_Управление.EmployeeControls
{
    public partial class RequestsCompletedControl : UserControl
    {
        private readonly string connectionString =
            "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";

        public RequestsCompletedControl()
        {
            InitializeComponent();
            LoadCompletedRequests();
        }

        private void LoadCompletedRequests()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            c.имя AS [Имя],
                            c.фамилия AS [Фамилия],
                            a.адрес AS [Адрес],
                            z.текст AS [Запрос],
                            z.категория AS [Категория],
                            ISNULL(s.имя + ' ' + s.фамилия, '') AS [Мастер],
                            CASE 
                                WHEN z.статус = 'отменен' THEN 'Да'
                                ELSE 'Нет'
                            END AS [Отменена]
                        FROM Запросы z
                        JOIN Клиенты c ON z.клиент_id = c.клиент_id
                        JOIN Адреса a ON z.адрес_id = a.адрес_id
                        LEFT JOIN Назначения n ON z.запрос_id = n.запрос_id
                        LEFT JOIN Сотрудники s ON n.сотрудник_id = s.сотрудник_id
                        WHERE z.статус IN ('завершен', 'отменен')";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    CompletedRequestsGrid.ItemsSource = table.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке завершённых заявок: " + ex.Message);
            }
        }
    }
}
