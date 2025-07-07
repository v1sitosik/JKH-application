using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ЖКХ_Управление.Controls
{
    public partial class CategoriesControl : UserControl
    {
        private readonly string connectionString = "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";
        private int _selectedCategoryId = -1;
        private List<string> protectedCategories = new List<string> { "Сантехника", "Электрика", "Строительство" };

        public CategoriesControl()
        {
            InitializeComponent();
            LoadCategories();
        }

        public class Category
        {
            public int категория_id { get; set; }
            public string название { get; set; }
        }

        private void LoadCategories()
        {
            CategoriesDataGrid.ItemsSource = null;
            var categories = new List<Category>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT категория_id, название FROM Категории_мастеров ORDER BY категория_id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
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

            CategoriesDataGrid.ItemsSource = categories;
        }

        private void CategoriesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is Category selected)
            {
                _selectedCategoryId = selected.категория_id;
                CategoryNameTextBox.Text = selected.название;
            }
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            string name = CategoryNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите название категории.");
                return;
            }

            if (IsCategoryExists(name))
            {
                MessageBox.Show("Такая категория уже существует.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Категории_мастеров (название) VALUES (@name)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadCategories();
            CategoryNameTextBox.Clear();
        }

        private void EditCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            string name = CategoryNameTextBox.Text.Trim();
            if (_selectedCategoryId == -1)
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }

            if (protectedCategories.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Эту категорию нельзя изменить.");
                return;
            }

            if (IsCategoryExists(name))
            {
                MessageBox.Show("Такая категория уже существует.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Категории_мастеров SET название = @name WHERE категория_id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@id", _selectedCategoryId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadCategories();
            CategoryNameTextBox.Clear();
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategoryId == -1)
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }

            string name = CategoryNameTextBox.Text.Trim();

            if (protectedCategories.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Эту категорию нельзя удалить.");
                return;
            }

            // Стилизованное подтверждение удаления
            var confirmWindow = new ConfirmDeleteWindow($"Удалить категорию '{name}'?");
            if (confirmWindow.ShowDialog() != true)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Сотрудники WHERE категория_id = @id";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id", _selectedCategoryId);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Нельзя удалить категорию, которая используется у сотрудников.");
                        return;
                    }
                }

                string deleteQuery = "DELETE FROM Категории_мастеров WHERE категория_id = @id";
                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@id", _selectedCategoryId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadCategories();
            CategoryNameTextBox.Clear();
        }


        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadCategories();
            CategoryNameTextBox.Clear();
        }

        private bool IsCategoryExists(string name)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Категории_мастеров WHERE LOWER(название) = LOWER(@name)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }
    }
}
