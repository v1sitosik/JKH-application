using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Controls;
using ЖКХ_Управление.Models;

namespace ЖКХ_Управление.MasterControls
{
    public partial class CompletedAssignmentsControl : UserControl
    {
        private readonly string connectionString =
            "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";

        private readonly int мастерId;

        public CompletedAssignmentsControl(int мастерId)
        {
            InitializeComponent();
            this.мастерId = мастерId;
            LoadCompletedAssignments();
        }

        private void LoadCompletedAssignments()
        {
            var list = new List<MasterAssignmentViewModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                SELECT 
                    nz.назначение_id,
                    z.запрос_id,
                    k.имя + ' ' + k.фамилия AS Клиент,
                    a.адрес,
                    z.текст,
                    z.категория,
                    nz.дата_назначения,
                    nz.статус
                FROM Назначения nz
                JOIN Запросы z ON nz.запрос_id = z.запрос_id
                JOIN Клиенты k ON z.клиент_id = k.клиент_id
                LEFT JOIN Адреса a ON z.адрес_id = a.адрес_id
                WHERE nz.сотрудник_id = @мастерId AND nz.статус = 'выполнено'";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@мастерId", мастерId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new MasterAssignmentViewModel
                            {
                                НазначениеId = reader.GetInt32(0),
                                ЗапросId = reader.GetInt32(1),
                                Клиент = reader.GetString(2),
                                Адрес = reader.IsDBNull(3) ? "Не указан" : reader.GetString(3),
                                Текст = reader.IsDBNull(4) ? "Нет данных" : reader.GetString(4),
                                Категория = reader.IsDBNull(5) ? "Не указано" : reader.GetString(5),
                                ДатаНазначения = reader.GetDateTime(6),
                                СтатусНазначения = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            CompletedRequestsDataGrid.ItemsSource = list;
        }
    }
}
