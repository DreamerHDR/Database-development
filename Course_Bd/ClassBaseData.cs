using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Course_Bd
{
    internal class ClassBaseData
    {
        private readonly string _connectionString;

        // Конструктор для инициализации строки подключения
        public ClassBaseData(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Метод для выполнения SELECT запроса и возврата данных в DataTable
        public DataTable ExecuteSelectQuery(string query)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var adapter = new NpgsqlDataAdapter(query, conn);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при выполнении SELECT запроса: {ex.Message}");
                    return null;
                }
            }
        }

        // Метод для выполнения запросов INSERT, UPDATE, DELETE
        public int ExecuteNonQuery(string query)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        return cmd.ExecuteNonQuery(); // Возвращает количество затронутых строк
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
                    return -1;
                }
            }
        }

        // Метод для выполнения запросов с параметрами (например, для защиты от SQL инъекций)
        public int ExecuteNonQueryWithParameters(string query, params NpgsqlParameter[] parameters)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteNonQuery(); // Возвращает количество затронутых строк
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при выполнении запроса с параметрами: {ex.Message}");
                    return -1;
                }
            }
        }

        // Метод для получения данных из одного столбца для заполнения ComboBox
        public List<string> GetColumnDataForComboBox(string query)
        {
            var result = new List<string>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0)); // Получаем значение из первого столбца
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при извлечении данных: {ex.Message}");
                }
            }

            return result;
        }
    }
}
