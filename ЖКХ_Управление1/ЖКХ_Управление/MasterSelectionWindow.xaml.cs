using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using ЖКХ_Управление.Models;
using ЖКХ_Управление.Views;

namespace ЖКХ_Управление
{
    public partial class MasterSelectionWindow : Window
    {
        private readonly string connectionString = "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";
        private int запросId;
        private bool isChange;
        private List<Category> categories = new List<Category>();
        private List<MasterWorker> masters = new List<MasterWorker>();

        public MasterSelectionWindow(Request request, bool isChange = false)
        {
            InitializeComponent();
            запросId = request.запрос_id;
            this.isChange = isChange;
            LoadCategories();
        }

        private void LoadCategories()
        {
            categories.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT категория_id, название FROM Категории_мастеров";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            категория_id = reader.GetInt32(0),
                            название = reader.GetString(1)
                        });
                    }
                }
            }
            CategoriesComboBox.ItemsSource = categories;
            CategoriesComboBox.DisplayMemberPath = "название";
            CategoriesComboBox.SelectedValuePath = "категория_id";
        }

        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesComboBox.SelectedItem is Category selectedCategory)
            {
                LoadMasters(selectedCategory.категория_id);
            }
        }

        private void LoadMasters(int categoryId)
        {
            masters.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT s.сотрудник_id, s.имя, s.фамилия 
            FROM Сотрудники s
            WHERE s.категория_id = @categoryId 
              AND s.должность_id = (SELECT должность_id FROM Должности WHERE название = 'Мастер')";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@categoryId", categoryId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            masters.Add(new MasterWorker
                            {
                                сотрудник_id = reader.GetInt32(0),
                                имя = reader.GetString(1),
                                фамилия = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            MastersComboBox.ItemsSource = null;              // 👈 Обязательно сбросить
            MastersComboBox.ItemsSource = masters;
        }

        private void AssignMasterButton_Click(object sender, RoutedEventArgs e)
        {
            if (MastersComboBox.SelectedItem is MasterWorker selectedMaster)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (isChange)
                    {
                        string updateAssignmentQuery = @"
                        UPDATE Назначения 
                        SET сотрудник_id = @сотрудникId, дата_назначения = GETDATE()
                        WHERE запрос_id = @запросId";
                        using (SqlCommand updateCommand = new SqlCommand(updateAssignmentQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@запросId", запросId);
                            updateCommand.Parameters.AddWithValue("@сотрудникId", selectedMaster.сотрудник_id);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertQuery = @"
                        INSERT INTO Назначения (запрос_id, сотрудник_id, дата_назначения, статус) 
                        VALUES (@запросId, @сотрудникId, GETDATE(), 'в работе')";
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@запросId", запросId);
                            insertCommand.Parameters.AddWithValue("@сотрудникId", selectedMaster.сотрудник_id);
                            insertCommand.ExecuteNonQuery();
                        }
                        string updateQuery = @"
                        UPDATE Запросы 
                        SET статус = 'в процессе' 
                        WHERE запрос_id = @запросId";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@запросId", запросId);
                            int rowsAffected = updateCommand.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                MessageBox.Show("Ошибка обновления статуса заявки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                new SuccessDialogWindow(isChange ? "Мастер успешно заменён!" : "Мастер успешно назначен!").ShowDialog();
                this.Close();
            }
            else
            {
                WarningDialogWindow.Show("Выберите мастера!", this);
            }
        }
    }

    public class MasterWorker
    {
        public int сотрудник_id { get; set; }
        public string имя { get; set; }
        public string фамилия { get; set; }
        public string ФИО => $"{имя} {фамилия}";
    }

    public class Category
    {
        public int категория_id { get; set; }
        public string название { get; set; }
    }
}
