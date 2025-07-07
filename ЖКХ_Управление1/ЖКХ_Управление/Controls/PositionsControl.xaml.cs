using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace ЖКХ_Управление.Controls
{
    public partial class PositionsControl : UserControl
    {
        private readonly string connectionString = "Data Source=DESKTOP-0RJV3FH\\SQLEXPRESS;Initial Catalog=ЖКХ_Система;Integrated Security=True;";
        private List<Position> allPositions = new List<Position>();
        private int selectedPositionId = -1;
        private readonly List<string> protectedTitles = new List<string> { "Администратор", "Оператор", "Мастер" };

        public PositionsControl()
        {
            InitializeComponent();
            LoadPositions();
        }

        public class Position
        {
            public int должность_id { get; set; }
            public string название { get; set; }
        }

        private void LoadPositions()
        {
            allPositions.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT должность_id, название FROM Должности ORDER BY должность_id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allPositions.Add(new Position
                        {
                            должность_id = reader.GetInt32(0),
                            название = reader.GetString(1)
                        });
                    }
                }
            }

            PositionsDataGrid.ItemsSource = null;
            PositionsDataGrid.ItemsSource = allPositions;
        }

        private void PositionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PositionsDataGrid.SelectedItem is Position selected)
            {
                selectedPositionId = selected.должность_id;
                PositionNameTextBox.Text = selected.название;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = PositionNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название должности.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Должности WHERE название = @name";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@name", name);
                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Такая должность уже существует.");
                        return;
                    }
                }

                string query = "INSERT INTO Должности (название) VALUES (@name)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadPositions();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPositionId == -1)
            {
                MessageBox.Show("Выберите должность.");
                return;
            }

            string name = PositionNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название должности.");
                return;
            }

            string oldName = allPositions.Find(p => p.должность_id == selectedPositionId)?.название;
            if (protectedTitles.Contains(oldName))
            {
                MessageBox.Show("Эту должность нельзя изменить.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM Должности WHERE название = @name AND должность_id != @id";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@name", name);
                    checkCmd.Parameters.AddWithValue("@id", selectedPositionId);
                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Такая должность уже существует.");
                        return;
                    }
                }

                string query = "UPDATE Должности SET название = @name WHERE должность_id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@id", selectedPositionId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadPositions();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPositionId == -1)
            {
                MessageBox.Show("Выберите должность.");
                return;
            }

            string name = allPositions.Find(p => p.должность_id == selectedPositionId)?.название;
            if (protectedTitles.Contains(name))
            {
                MessageBox.Show("Эту должность нельзя удалить.");
                return;
            }

            var confirmWindow = new ConfirmDeleteWindow($"Удалить должность '{name}'?");
            if (confirmWindow.ShowDialog() != true)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Сотрудники WHERE должность_id = @id";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id", selectedPositionId);
                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Нельзя удалить должность, которая используется у сотрудников.");
                        return;
                    }
                }

                string query = "DELETE FROM Должности WHERE должность_id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", selectedPositionId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadPositions();
            PositionNameTextBox.Clear();
            selectedPositionId = -1;
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPositions();
        }
    }
}
