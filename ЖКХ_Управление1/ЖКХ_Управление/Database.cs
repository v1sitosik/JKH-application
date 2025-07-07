using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ЖКХ_Управление
{
    public class Database
    {
        private readonly string connectionString;

        public Database()
        {
            // Получаем строку подключения из App.config
            connectionString = ConfigurationManager.ConnectionStrings["JkhDbConnection"].ConnectionString;
        }

        // ✅ Метод для проверки подключения к базе данных
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Console.WriteLine("✅ Подключение к базе данных успешно!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка подключения: {ex.Message}");
                return false;
            }
        }
    }
}
