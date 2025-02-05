using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace Course_Bd
{
    public partial class Form1 : Form
    {
        private string _connectionString;
        private ClassBaseData _dbHelper;
        private string _userRole;

        public Form1(string connectionString, string userRole)
        {
            InitializeComponent();
            _userRole = userRole;
            ApplyRoleRestrictions();
            _connectionString = connectionString;
            //string connString = "Host=localhost;Username=postgres;Password=12345;Database=CityTransport";
            _dbHelper = new ClassBaseData(connectionString);
            this.Load += new EventHandler(Form1_Load);
        }

        private void ApplyRoleRestrictions()
        {
            if (_userRole == "regular_user")
            {
                // Отключаем кнопки добавления, редактирования и удаления
                button1.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;

                button2.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;

                button3.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;

                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

                MessageBox.Show("Ваши права ограничены: доступно только чтение данных и измнения справочника.", "Ограничения", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (_userRole == "guest_user")
            {
                // Отключаем кнопки добавления, редактирования и удаления
                button1.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;

                button2.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;

                button3.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;

                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

                button13.Enabled = false;
                button14.Enabled = false;
                button15.Enabled = false;

                MessageBox.Show("Ваши права ограничены: доступно только чтение данных.", "Ограничения", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Пример запроса для получения данных
            string query = @"SELECT * FROM passengers;";  // Замените на свой запрос

            DataTable dt = _dbHelper.ExecuteSelectQuery(query);
            if (dt != null)
            {
                // Привязка данных к DataGridView
                dataGridView1.DataSource = dt;
                // Пример скрытия столбца
                //dataGridView1.Columns["id_column"].Visible = false;
            }

            string query1 = @"SELECT * FROM routes;";  // Замените на свой запрос

            DataTable dt1 = _dbHelper.ExecuteSelectQuery(query1);
            if (dt1 != null)
            {
                // Привязка данных к DataGridView
                dataGridView2.DataSource = dt1;
                // Пример скрытия столбца
                //dataGridView1.Columns["id_column"].Visible = false;
            }

            string query2 = @"SELECT * FROM transport;";  // Замените на свой запрос

            DataTable dt2 = _dbHelper.ExecuteSelectQuery(query2);
            if (dt2 != null)
            {
                // Привязка данных к DataGridView
                dataGridView3.DataSource = dt2;
                // Пример скрытия столбца
                //dataGridView1.Columns["id_column"].Visible = false;
            }

            string query3 = @"SELECT * FROM transport_types;";  // Замените на свой запрос

            DataTable dt3 = _dbHelper.ExecuteSelectQuery(query3);
            if (dt3 != null)
            {
                // Привязка данных к DataGridView
                dataGridView4.DataSource = dt3;
                // Пример скрытия столбца
                //dataGridView1.Columns["id_column"].Visible = false;
            }

            string query4 = @"SELECT * FROM trip_cost;";  // Замените на свой запрос

            DataTable dt4 = _dbHelper.ExecuteSelectQuery(query4);
            if (dt4 != null)
            {
                // Привязка данных к DataGridView
                dataGridView5.DataSource = dt4;
                // Пример скрытия столбца
                //dataGridView1.Columns["id_column"].Visible = false;
            }

            string query6 = @"SELECT * FROM trips;";  // Замените на свой запрос

            DataTable dt6 = _dbHelper.ExecuteSelectQuery(query6);
            if (dt6 != null)
            {
                // Привязка данных к DataGridView
                dataGridView7.DataSource = dt6;
                // Пример скрытия столбца
                //dataGridView1.Columns["id_column"].Visible = false;
            }

            string query7 = @"SELECT * FROM get_all_trips();";  // Замените на свой запрос

            DataTable dt7 = _dbHelper.ExecuteSelectQuery(query7);
            if (dt7 != null)
            {
                // Привязка данных к DataGridView
                dataGridView6.DataSource = dt7;
                // Пример скрытия столбца
                //dataGridView1.Columns["id_column"].Visible = false;
            }



            // Заполняем ComboBox1 маршрутами
            string routeQuery = "SELECT route_id, end_point FROM routes;";
            DataTable routeTable = _dbHelper.ExecuteSelectQuery(routeQuery);

            if (routeTable != null)
            {
                comboBox1.DataSource = routeTable;
                comboBox1.DisplayMember = "end_point"; // Что отображать
                comboBox1.ValueMember = "route_id";     // Значение
            }
            else
            {
                MessageBox.Show("Не удалось загрузить маршруты.");
            }

            // Заполняем ComboBox2 типами транспорта
            string transportTypeQuery = "SELECT transport_type_id, transport_type_name FROM transport_types;";
            DataTable transportTypeTable = _dbHelper.ExecuteSelectQuery(transportTypeQuery);

            if (transportTypeTable != null)
            {
                comboBox2.DataSource = transportTypeTable;
                comboBox2.DisplayMember = "transport_type_name";
                comboBox2.ValueMember = "transport_type_id";
            }
            else
            {
                MessageBox.Show("Не удалось загрузить типы транспорта.");
            }

            comboBox1.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            comboBox2.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Вызываем обновление стоимости при изменении значения в ComboBox
            UpdateTripCost();
        }

        private void UpdateTripCost()
        {
            // Получаем выбранные значения из ComboBox
            int routeId = (int)comboBox1.SelectedValue;
            int transportTypeId = (int)comboBox2.SelectedValue;

            // Запрос для получения стоимости поездки
            string query = $@"
            SELECT trip_price 
            FROM trip_cost 
            WHERE route_id = {routeId} AND transport_type_id = {transportTypeId};";

            // Выполняем запрос и получаем результат
            DataTable result = _dbHelper.ExecuteSelectQuery(query);

            if (result != null && result.Rows.Count > 0)
            {
                // Если результат найден, обновляем значение в numericUpDown
                int tripCost = (int)result.Rows[0]["trip_price"];
                numericUpDown1.Value = tripCost;
            }
            else
            {
                // Если данных нет, выставляем значение 0
                numericUpDown1.Value = 0;
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Передаем объект _dbHelper в форму FormAdd1
            FormAdd1 formAdd = new FormAdd1();
            formAdd.ShowDialog(); // Показываем форму как модальное окно
            UpdateDataGridView();
        }

        private void UpdateDataGridView()
        {
            // Пример запроса для обновления данных в DataGridView
            string query = @"SELECT * FROM passengers;";

            DataTable dt = _dbHelper.ExecuteSelectQuery(query);
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormAdd2 formAdd1 = new FormAdd2();
            formAdd1.ShowDialog(); // Показываем форму как модальное окно
            UpdateDataGridView1();
        }

        private void UpdateDataGridView1()
        {
            // Пример запроса для обновления данных в DataGridView
            string query = @"SELECT * FROM routes;";

            DataTable dt = _dbHelper.ExecuteSelectQuery(query);
            if (dt != null)
            {
                dataGridView2.DataSource = dt;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormAdd3 formAdd2 = new FormAdd3();
            formAdd2.ShowDialog(); // Показываем форму как модальное окно
            UpdateDataGridView2();
        }

        private void UpdateDataGridView2()
        {
            // Пример запроса для обновления данных в DataGridView
            string query = @"SELECT * FROM transport;";

            DataTable dt = _dbHelper.ExecuteSelectQuery(query);
            if (dt != null)
            {
                dataGridView3.DataSource = dt;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем данные из выбранной строки DataGridView
                int passengerId = (int)dataGridView1.SelectedRows[0].Cells["passenger_id"].Value;
                string firstName = dataGridView1.SelectedRows[0].Cells["first_name"].Value.ToString();
                string lastName = dataGridView1.SelectedRows[0].Cells["last_name"].Value.ToString();

                // Открываем форму для редактирования и передаем данные
                FormAdd1 formAdd = new FormAdd1(passengerId, firstName, lastName);
                formAdd.ShowDialog(); // Показываем форму как модальное окно
                UpdateDataGridView();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для редактирования.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем идентификатор пассажира из выбранной строки
                int passengerId = (int)dataGridView1.SelectedRows[0].Cells["passenger_id"].Value;

                // Подтверждение удаления
                var confirmation = MessageBox.Show(
                    "Вы уверены, что хотите удалить эту запись?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmation == DialogResult.Yes)
                {
                    // Формируем SQL-запрос на удаление
                    string query = $@"DELETE FROM passengers WHERE passenger_id = {passengerId};";

                    // Выполняем запрос
                    int result = _dbHelper.ExecuteNonQuery(query);

                    if (result > 0)
                    {
                        MessageBox.Show("Запись успешно удалена.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления записи.");
                    }
                    UpdateDataGridView();

                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // Получаем данные из выбранной строки DataGridView
                int routeId = (int)dataGridView2.SelectedRows[0].Cells["route_id"].Value;
                string start_point = dataGridView2.SelectedRows[0].Cells["start_point"].Value.ToString();
                string end_point = dataGridView2.SelectedRows[0].Cells["end_point"].Value.ToString();

                // Открываем форму для редактирования и передаем данные
                FormAdd2 formAdd = new FormAdd2(routeId, start_point, end_point);
                formAdd.ShowDialog(); // Показываем форму как модальное окно
                UpdateDataGridView1();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для редактирования.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // Получаем идентификатор пассажира из выбранной строки
                int routeId = (int)dataGridView2.SelectedRows[0].Cells["route_id"].Value;

                // Подтверждение удаления
                var confirmation = MessageBox.Show(
                    "Вы уверены, что хотите удалить эту запись?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmation == DialogResult.Yes)
                {
                    // Формируем SQL-запрос на удаление
                    string query = $@"DELETE FROM routes WHERE route_id = {routeId};";

                    // Выполняем запрос
                    int result = _dbHelper.ExecuteNonQuery(query);

                    if (result > 0)
                    {
                        MessageBox.Show("Запись успешно удалена.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления записи.");
                    }
                    UpdateDataGridView1();

                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView3.SelectedRows.Count > 0)
            {
                // Получаем данные из выбранной строки
                int transportId = (int)dataGridView3.SelectedRows[0].Cells["transport_id"].Value;
                string transport_type_id = dataGridView3.SelectedRows[0].Cells["transport_type_id"].Value.ToString();
                decimal vehicle_count = Convert.ToDecimal(dataGridView3.SelectedRows[0].Cells["vehicle_count"].Value);

                // Открываем форму для редактирования и передаем данные
                FormAdd3 formAdd3 = new FormAdd3(transportId, transport_type_id, vehicle_count);
                formAdd3.ShowDialog(); // Показываем форму как модальное окно
                UpdateDataGridView2();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для редактирования.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView3.SelectedRows.Count > 0)
            {
                // Получаем идентификатор пассажира из выбранной строки
                int transportId = (int)dataGridView3.SelectedRows[0].Cells["transport_id"].Value;

                // Подтверждение удаления
                var confirmation = MessageBox.Show(
                    "Вы уверены, что хотите удалить эту запись?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmation == DialogResult.Yes)
                {
                    // Формируем SQL-запрос на удаление
                    string query = $@"DELETE FROM transport WHERE transport_id = {transportId};";

                    // Выполняем запрос
                    int result = _dbHelper.ExecuteNonQuery(query);

                    if (result > 0)
                    {
                        MessageBox.Show("Запись успешно удалена.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления записи.");
                    }
                    UpdateDataGridView2();

                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FormAdd4 formAdd3 = new FormAdd4();
            formAdd3.ShowDialog(); // Показываем форму как модальное окно
            UpdateDataGridView3();
        }

        private void UpdateDataGridView3()
        {
            // Пример запроса для обновления данных в DataGridView
            string query = @"SELECT * FROM trip_cost;";

            DataTable dt = _dbHelper.ExecuteSelectQuery(query);
            if (dt != null)
            {
                dataGridView5.DataSource = dt;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView5.SelectedRows.Count > 0)
            {
                // Получаем данные из выбранной строки
                int costId = (int)dataGridView5.SelectedRows[0].Cells["cost_id"].Value;
                int routeId = (int)dataGridView5.SelectedRows[0].Cells["route_id"].Value;
                int transportTypeId = (int)dataGridView5.SelectedRows[0].Cells["transport_type_id"].Value;
                int tripCost = (int)dataGridView5.SelectedRows[0].Cells["trip_price"].Value;

                // Открываем форму для редактирования
                FormAdd4 formAdd4 = new FormAdd4(costId, routeId, transportTypeId, tripCost);
                formAdd4.ShowDialog(); // Показываем форму как модальное окно
                UpdateDataGridView3();
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView5.SelectedRows.Count > 0)
            {
                // Получаем идентификатор пассажира из выбранной строки
                int costId = (int)dataGridView5.SelectedRows[0].Cells["cost_id"].Value;

                // Подтверждение удаления
                var confirmation = MessageBox.Show(
                    "Вы уверены, что хотите удалить эту запись?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmation == DialogResult.Yes)
                {
                    // Формируем SQL-запрос на удаление
                    string query = $@"DELETE FROM trip_cost WHERE cost_id = {costId};";

                    // Выполняем запрос
                    int result = _dbHelper.ExecuteNonQuery(query);

                    if (result > 0)
                    {
                        MessageBox.Show("Запись успешно удалена.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления записи.");
                    }
                    UpdateDataGridView3();

                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            FormAdd5 formAdd4 = new FormAdd5();
            formAdd4.ShowDialog(); // Показываем форму как модальное окно
            UpdateDataGridView4();
        }

        private void UpdateDataGridView4()
        {
            // Пример запроса для обновления данных в DataGridView
            string query = @"SELECT * FROM trips;";

            DataTable dt = _dbHelper.ExecuteSelectQuery(query);
            if (dt != null)
            {
                dataGridView7.DataSource = dt;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView7.SelectedRows.Count > 0)
            {
                // Получаем данные из выбранной строки
                int tripId = Convert.ToInt32(dataGridView7.SelectedRows[0].Cells["trip_id"].Value);
                int transportId = Convert.ToInt32(dataGridView7.SelectedRows[0].Cells["transport_id"].Value);
                int routeId = Convert.ToInt32(dataGridView7.SelectedRows[0].Cells["route_id"].Value);
                DateTime departureTime = Convert.ToDateTime(dataGridView7.SelectedRows[0].Cells["departure_time"].Value);
                DateTime arrivalTime = Convert.ToDateTime(dataGridView7.SelectedRows[0].Cells["arrival_time"].Value);
                int passengerCount = Convert.ToInt32(dataGridView7.SelectedRows[0].Cells["passenger_count"].Value);

                // Открываем форму для редактирования
                FormAdd5 formAdd4 = new FormAdd5(tripId, transportId, routeId, departureTime, arrivalTime, passengerCount);
                formAdd4.ShowDialog(); // Показываем форму как модальное окно
                UpdateDataGridView4();
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView7.SelectedRows.Count > 0)
            {
                // Получаем идентификатор пассажира из выбранной строки
                int tripId = (int)dataGridView7.SelectedRows[0].Cells["trip_id"].Value;

                // Подтверждение удаления
                var confirmation = MessageBox.Show(
                    "Вы уверены, что хотите удалить эту запись?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmation == DialogResult.Yes)
                {
                    // Формируем SQL-запрос на удаление
                    string query = $@"DELETE FROM trips WHERE trip_id = {tripId};";

                    // Выполняем запрос
                    int result = _dbHelper.ExecuteNonQuery(query);

                    if (result > 0)
                    {
                        MessageBox.Show("Запись успешно удалена.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления записи.");
                    }
                    UpdateDataGridView4();

                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем ID поездки из TextBox
                int tripId;
                if (int.TryParse(textBox1.Text, out tripId))
                {
                    // Выполняем запрос к функции get_passenger_count
                    var passengerCount = GetPassengerCount(tripId);

                    // Подготавливаем таблицу с результатами
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Passenger Count");
                    DataRow row = dt.NewRow();
                    row["Passenger Count"] = passengerCount;
                    dt.Rows.Add(row);

                    // Отображаем результат в DataGridView
                    dataGridView8.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректный ID поездки.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
            }

        }

        private int GetPassengerCount(int tripId)
        {
            int result = 0;

            try
            {
                // Используем строку подключения _connectionString
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();

                    // Запрос для вызова функции и получения результата
                    string query = "SELECT public.get_passenger_count(@tripId);";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        // Добавляем параметр tripId в запрос
                        cmd.Parameters.AddWithValue("tripId", tripId);

                        // Выполняем запрос и получаем результат
                        var queryResult = cmd.ExecuteScalar();
                        if (queryResult != null)
                        {
                            result = Convert.ToInt32(queryResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
            }

            return result;
        }
    }

}

