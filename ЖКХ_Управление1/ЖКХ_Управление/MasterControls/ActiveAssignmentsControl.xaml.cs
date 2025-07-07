using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using ЖКХ_Управление.Models;
using ЖКХ_Управление.Views;

namespace ЖКХ_Управление.MasterControls
{
    public partial class ActiveAssignmentsControl : UserControl
    {
        private readonly string connectionString =
            "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";

        private readonly int мастерId;

        public ActiveAssignmentsControl(int мастерId)
        {
            InitializeComponent();
            this.мастерId = мастерId;
            LoadAssignments();
        }

        private void LoadAssignments()
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
                WHERE nz.сотрудник_id = @мастерId AND nz.статус = 'в работе'";

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
                                Текст = reader.GetString(4),
                                Категория = reader.GetString(5),
                                ДатаНазначения = reader.GetDateTime(6),
                                СтатусНазначения = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            AssignedRequestsDataGrid.ItemsSource = list;
        }

        private void CompleteRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (AssignedRequestsDataGrid.SelectedItem is MasterAssignmentViewModel selected)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var updateAssignment = @"
                        UPDATE Назначения 
                        SET статус = 'выполнено'
                        WHERE назначение_id = @назначениеId";
                    using (var cmd = new SqlCommand(updateAssignment, connection))
                    {
                        cmd.Parameters.AddWithValue("@назначениеId", selected.НазначениеId);
                        cmd.ExecuteNonQuery();
                    }

                    var checkQuery = @"
                        SELECT COUNT(*) 
                        FROM Назначения 
                        WHERE запрос_id = @запросId AND статус = 'в работе'";
                    using (var cmd = new SqlCommand(checkQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@запросId", selected.ЗапросId);
                        int remaining = (int)cmd.ExecuteScalar();

                        if (remaining == 0)
                        {
                            var updateRequest = @"
                                UPDATE Запросы 
                                SET статус = 'завершен' 
                                WHERE запрос_id = @запросId";
                            using (var update = new SqlCommand(updateRequest, connection))
                            {
                                update.Parameters.AddWithValue("@запросId", selected.ЗапросId);
                                update.ExecuteNonQuery();
                            }
                        }
                    }
                }

                var owner = Window.GetWindow(this as DependencyObject);
                SuccessDialogWindow.Show("Успешно", "Заявка завершена.", owner);
                LoadAssignments();
            }
            else
            {
                var owner = Window.GetWindow(this as DependencyObject);
                WarningDialogWindow.Show("Выберите заявку!", owner);
            }
        }
    }
}
