using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Course_Bd
{
    public partial class FormLogin : Form
    {
        public string ConnectionString { get; private set; } // Строка подключения, доступная после авторизации
        public string UserRole { get; private set; }
        public FormLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            // Формируем строку подключения
            string connString = $"Host=localhost;Username={username};Password={password};Database=CityTransport";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connString))
                {
                    connection.Open(); // Проверка подключения

                    // Запрос для получения роли пользователя
                    string query = "SELECT current_user;"; // Или другой запрос для вашей схемы базы данных
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        UserRole = command.ExecuteScalar()?.ToString(); // Получаем роль пользователя
                    }
                }

                // Если подключение успешно, сохраняем строку подключения
                ConnectionString = connString;
                MessageBox.Show($"Авторизация успешна! Ваша роль: {UserRole}");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // Ошибка подключения
                MessageBox.Show($"Ошибка авторизации: {ex.Message}");
            }
        }

    }
}
