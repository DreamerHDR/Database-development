using System;
using System.Data;
using System.Windows.Forms;

namespace Course_Bd
{
    public partial class FormAdd5 : Form
    {
        private ClassBaseData _dbHelper;
        private int? _tripId; // Для редактирования существующей записи

        public FormAdd5()
        {
            InitializeComponent();
            string connString = "Host=localhost;Username=postgres;Password=12345;Database=CityTransport";
            // Настройка DateTimePicker для редактирования времени
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm";
            dateTimePicker1.ShowUpDown = true;

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm";
            dateTimePicker2.ShowUpDown = true;
            _dbHelper = new ClassBaseData(connString);
            this.Load += new EventHandler(FormAdd5_Load);
        }

        public FormAdd5(int tripId, int transportId, int routeId, DateTime departureTime, DateTime arrivalTime, int passengerCount) : this()
        {
            _tripId = tripId;

            comboBox1.SelectedValue = transportId;
            comboBox2.SelectedValue = routeId;
            dateTimePicker1.Value = departureTime;
            dateTimePicker2.Value = arrivalTime;
            textBox1.Text = passengerCount.ToString();
        }

        private void FormAdd5_Load(object sender, EventArgs e)
        {
            // Заполнение ComboBox для transport_id
            string transportQuery = "SELECT transport_id, transport_type_id FROM transport;";
            DataTable transportData = _dbHelper.ExecuteSelectQuery(transportQuery);

            if (transportData != null)
            {
                comboBox1.DataSource = transportData;
                comboBox1.DisplayMember = "transport_type_id";
                comboBox1.ValueMember = "transport_id";
            }

            // Заполнение ComboBox для route_id
            string routeQuery = "SELECT route_id, end_point FROM routes;";
            DataTable routeData = _dbHelper.ExecuteSelectQuery(routeQuery);

            if (routeData != null)
            {
                comboBox2.DataSource = routeData;
                comboBox2.DisplayMember = "end_point";
                comboBox2.ValueMember = "route_id";
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Получаем данные из элементов управления
            int transportId = (int)comboBox1.SelectedValue;
            int routeId = (int)comboBox2.SelectedValue;
            DateTime departureTime = dateTimePicker1.Value;
            DateTime arrivalTime = dateTimePicker2.Value;
            int passengerCount;

            if (!int.TryParse(textBox1.Text.Trim(), out passengerCount))
            {
                MessageBox.Show("Пожалуйста, введите корректное число пассажиров.");
                return;
            }

            if (departureTime >= arrivalTime)
            {
                MessageBox.Show("Время прибытия должно быть позже времени отправления.");
                return;
            }

            string query;
            if (_tripId.HasValue)
            {
                // Обновляем запись
                query = $@"UPDATE trips 
                          SET transport_id = {transportId}, route_id = {routeId}, departure_time = '{departureTime:yyyy-MM-dd HH:mm:ss}', 
                              arrival_time = '{arrivalTime:yyyy-MM-dd HH:mm:ss}', passenger_count = {passengerCount} 
                          WHERE trip_id = {_tripId.Value};";
            }
            else
            {
                // Добавляем новую запись
                query = $@"INSERT INTO trips (transport_id, route_id, departure_time, arrival_time, passenger_count) 
                          VALUES ({transportId}, {routeId}, '{departureTime:yyyy-MM-dd HH:mm:ss}', 
                                  '{arrivalTime:yyyy-MM-dd HH:mm:ss}', {passengerCount});";
            }

            int result = _dbHelper.ExecuteNonQuery(query);

            if (result > 0)
            {
                MessageBox.Show(_tripId.HasValue ? "Данные успешно обновлены." : "Данные успешно добавлены.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении данных.");
            }
        }

    }
}
