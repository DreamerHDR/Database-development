using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_Bd
{
    public partial class FormAdd3 : Form
    {
        private ClassBaseData _dbHelper;
        private int? _transportId;

        public FormAdd3()
        {
            InitializeComponent();
            string connString = "Host=localhost;Username=postgres;Password=12345;Database=CityTransport";
            _dbHelper = new ClassBaseData(connString);
            this.Load += new EventHandler(FormAdd3_Load);
        }
        public FormAdd3(int transportId, string transport_type_id, decimal vehicle_count) : this() // Вызываем базовый конструктор
        {
            _transportId = transportId;

            // Устанавливаем текущие значения в элементы управления
            comboBox1.Text = transport_type_id;
            numericUpDown1.Value = vehicle_count;
        }

        // Заполнение ComboBox1 с типами транспорта при загрузке формы
        private void FormAdd3_Load(object sender, EventArgs e)
        {
            // Запрос на получение типов транспорта
            string query = "SELECT transport_type_name, transport_type_id FROM transport_types;";
            DataTable dt = _dbHelper.ExecuteSelectQuery(query);

            if (dt != null)
            {
                // Добавляем данные в ComboBox1
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "transport_type_name"; // Столбец, который будет отображаться в ComboBox
                comboBox1.ValueMember = "transport_type_id";   // Столбец, который будет использоваться как значение
            }
            else
            {
                MessageBox.Show("Не удалось загрузить типы транспорта.");
            }
        }

        // Обработчик кнопки добавления данных
        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем выбранный тип транспорта и стоимость
            string transport_type_id = comboBox1.SelectedValue.ToString();
            decimal vehicle_count = numericUpDown1.Value;

            if (string.IsNullOrEmpty(transport_type_id))
            {
                MessageBox.Show("Пожалуйста, выберите тип транспорта.");
                return;
            }

            string query;
            if (_transportId.HasValue)
            {
                // Обновляем запись в режиме редактирования
                query = $@"UPDATE transport 
                       SET transport_type_id = '{transport_type_id}', vehicle_count = {vehicle_count}
                       WHERE transport_id = {_transportId.Value};";
            }
            else
            {
                // Добавляем новую запись
                query = $@"INSERT INTO transport (transport_type_id, vehicle_count) 
                       VALUES ('{transport_type_id}', {vehicle_count});";
            }


            // Выполнение запроса с прямыми значениями
            int result = _dbHelper.ExecuteNonQuery(query);

            if (result > 0)
            {
                MessageBox.Show("Данные успешно добавлены.");
            }
            else
            {
                MessageBox.Show("Ошибка добавления данных.");
            }

            // Закрытие формы
            this.Close();
        }
    }

}
