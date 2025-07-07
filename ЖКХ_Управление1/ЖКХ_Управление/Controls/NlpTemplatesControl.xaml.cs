using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ЖКХ_Управление.Controls
{
    public partial class NlpTemplatesControl : UserControl
    {
        private readonly string connectionString = "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";
        private List<NlpTemplate> allTemplates = new List<NlpTemplate>();
        private int selectedTemplateId = -1;

        public NlpTemplatesControl()
        {
            InitializeComponent();
            LoadTemplates();
        }

        public class NlpTemplate
        {
            public int шаблон_id { get; set; }
            public string категория { get; set; }
            public string ключевые_слова { get; set; }
            public string текст_ответа { get; set; }
        }

        private void LoadTemplates()
        {
            allTemplates.Clear();
            TemplatesDataGrid.ItemsSource = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT шаблон_id, категория, ключевые_слова, текст_ответа FROM Шаблоны_NLP";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allTemplates.Add(new NlpTemplate
                        {
                            шаблон_id = reader.GetInt32(0),
                            категория = reader.GetString(1),
                            ключевые_слова = reader.GetString(2),
                            текст_ответа = reader.GetString(3)
                        });
                    }
                }
            }

            TemplatesDataGrid.ItemsSource = allTemplates;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchTextBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(query))
            {
                TemplatesDataGrid.ItemsSource = allTemplates;
                return;
            }

            var filtered = allTemplates.Where(t =>
                t.ключевые_слова.ToLower().Contains(query) ||
                t.категория.ToLower().Contains(query) ||
                t.текст_ответа.ToLower().Contains(query)).ToList();

            TemplatesDataGrid.ItemsSource = filtered;
        }

        private void TemplatesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TemplatesDataGrid.SelectedItem is NlpTemplate selected)
            {
                selectedTemplateId = selected.шаблон_id;
                CategoryTextBox.Text = selected.категория;
                KeywordsTextBox.Text = selected.ключевые_слова;
                AnswerTextBox.Text = selected.текст_ответа;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryTextBox.Text) ||
                string.IsNullOrWhiteSpace(KeywordsTextBox.Text) ||
                string.IsNullOrWhiteSpace(AnswerTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (IsCategoryExists(CategoryTextBox.Text))
            {
                MessageBox.Show("Категория уже существует!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Шаблоны_NLP (категория, ключевые_слова, текст_ответа)
                                 VALUES (@категория, @ключевые_слова, @текст_ответа)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@категория", CategoryTextBox.Text);
                    cmd.Parameters.AddWithValue("@ключевые_слова", KeywordsTextBox.Text);
                    cmd.Parameters.AddWithValue("@текст_ответа", AnswerTextBox.Text);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadTemplates();
            ClearFields();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTemplateId == -1)
            {
                MessageBox.Show("Выберите шаблон для редактирования.");
                return;
            }

            if (string.IsNullOrWhiteSpace(CategoryTextBox.Text) ||
                string.IsNullOrWhiteSpace(KeywordsTextBox.Text) ||
                string.IsNullOrWhiteSpace(AnswerTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (IsCategoryExists(CategoryTextBox.Text, selectedTemplateId))
            {
                MessageBox.Show("Такая категория уже существует!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE Шаблоны_NLP
                                 SET категория = @категория, ключевые_слова = @ключевые_слова, текст_ответа = @текст_ответа
                                 WHERE шаблон_id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@категория", CategoryTextBox.Text);
                    cmd.Parameters.AddWithValue("@ключевые_слова", KeywordsTextBox.Text);
                    cmd.Parameters.AddWithValue("@текст_ответа", AnswerTextBox.Text);
                    cmd.Parameters.AddWithValue("@id", selectedTemplateId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadTemplates();
            ClearFields();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTemplateId == -1)
            {
                MessageBox.Show("Выберите шаблон для удаления.");
                return;
            }

            var selected = allTemplates.FirstOrDefault(t => t.шаблон_id == selectedTemplateId);
            if (selected == null) return;

            var confirmWindow = new ConfirmDeleteWindow($"Удалить шаблон категории '{selected.категория}'?");
            if (confirmWindow.ShowDialog() != true)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Шаблоны_NLP WHERE шаблон_id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", selectedTemplateId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadTemplates();
            ClearFields();
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTemplates();
            ClearFields();
        }

        private void ClearFields()
        {
            CategoryTextBox.Text = "";
            KeywordsTextBox.Text = "";
            AnswerTextBox.Text = "";
            selectedTemplateId = -1;
        }

        private bool IsCategoryExists(string category, int excludeId = -1)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Шаблоны_NLP WHERE категория = @категория AND шаблон_id != @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@категория", category);
                    cmd.Parameters.AddWithValue("@id", excludeId);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        private void TestTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            string input = TestInputTextBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(input))
            {
                TestResultTextBlock.Text = "Введите текст для проверки.";
                return;
            }

            var matched = allTemplates.FirstOrDefault(t =>
                t.ключевые_слова
                 .ToLower()
                 .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                 .Any(word => input.Contains(word)));

            TestResultTextBlock.Text = matched != null
                ? $"Сработал шаблон категории: {matched.категория}"
                : "Совпадений не найдено.";
        }
    }
}
