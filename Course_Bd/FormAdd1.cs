using System;
using System.Windows.Forms;

namespace Course_Bd
{
    public partial class FormAdd1 : Form
    {
        private ClassBaseData _dbHelper;
        private int _passengerId; // Идентификатор пассажира

        // Конструктор для добавления нового пассажира
        public FormAdd1()
        {
            InitializeComponent();
            string connString = "Host=localhost;Username=postgres;Password=12345;Database=CityTransport";
            _dbHelper = new ClassBaseData(connString);
        }

        // Конструктор для редактирования существующего пассажира
        public FormAdd1(int passengerId, string firstName, string lastName)
        {
            InitializeComponent();
            string connString = "Host=localhost;Username=postgres;Password=12345;Database=CityTransport";
            _dbHelper = new ClassBaseData(connString);

            _passengerId = passengerId;  // Идентификатор пассажира
            textBox1.Text = firstName;  // Заполнение полей текущими данными
            textBox2.Text = lastName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();

            // Проверка на пустые значения
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            string query = "";
            if (_passengerId > 0)
            {
                // Обновление существующей записи. Не изменяем passenger_id!
                query = $@"UPDATE passengers SET first_name = '{firstName}', last_name = '{lastName}' WHERE passenger_id = {_passengerId};";
            }
            else
            {
                // Добавление нового пассажира
                query = $@"INSERT INTO passengers (first_name, last_name) VALUES ('{firstName}', '{lastName}');";
            }

            // Выполнение запроса с прямыми значениями
            int result = _dbHelper.ExecuteNonQuery(query);

            if (result > 0)
            {
                MessageBox.Show(_passengerId > 0 ? "Пассажир обновлен успешно." : "Пассажир добавлен успешно.");
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении данных пассажира.");
            }
            // Закрыть форму
            this.Close();
        }

    }
}
