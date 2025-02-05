using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_Bd
{
    public partial class FormAdd2 : Form
    {
        private ClassBaseData _dbHelper;
        private int _routeId;
        public FormAdd2()
        {
            InitializeComponent();
            string connString = "Host=localhost;Username=postgres;Password=12345;Database=CityTransport";
            _dbHelper = new ClassBaseData(connString);
        }
        public FormAdd2(int routeId, string start_point, string end_point)
        {
            InitializeComponent();
            string connString = "Host=localhost;Username=postgres;Password=12345;Database=CityTransport";
            _dbHelper = new ClassBaseData(connString);

            _routeId = routeId;  // Идентификатор пассажира
            textBox1.Text = start_point;  // Заполнение полей текущими данными
            textBox2.Text = end_point;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string start_point = textBox1.Text.Trim();
            string end_point = textBox2.Text.Trim();

            // Проверка на пустые значения
            if (string.IsNullOrEmpty(start_point) || string.IsNullOrEmpty(end_point))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }


            string query = "";
            if (_routeId > 0)
            {
                // Обновление существующей записи. Не изменяем passenger_id!
                query = $@"UPDATE routes SET start_point = '{start_point}', end_point = '{end_point}' WHERE route_id = {_routeId};";
            }
            else
            {
                // Добавление нового пассажира
                query = $@"INSERT INTO routes (start_point, end_point) VALUES ('{start_point}', '{end_point}');";
            }
            // Выполнение запроса с прямыми значениями
            int result = _dbHelper.ExecuteNonQuery(query);

            if (result > 0)
            {
                MessageBox.Show("Маршрут добавлен успешно.");
            }
            else
            {
                MessageBox.Show("Ошибка добавления маршрута.");
            }

            // Закрыть форму добавления
            this.Close();
        }
    }
}
