using System;
using System.Data;
using System.Windows.Forms;

namespace Course_Bd
{
    public partial class FormAdd4 : Form
    {
        private ClassBaseData _dbHelper;
        private int? _costId; // Nullable для режима редактирования

        public FormAdd4()
        {
            InitializeComponent();
            string connString = "Host=localhost;Username=postgres;Password=12345;Database=CityTransport";
            _dbHelper = new ClassBaseData(connString);

            this.Load += new EventHandler(FormAdd4_Load);
        }

        public FormAdd4(int costId, int routeId, int transportTypeId, decimal tripCost) : this()
        {
            _costId = costId;

            // Устанавливаем значения для элементов
            comboBox1.SelectedValue = routeId;
            comboBox2.SelectedValue = transportTypeId;
            numericUpDown1.Value = tripCost;
        }

        private void FormAdd4_Load(object sender, EventArgs e)
        {
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
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Получаем значения из элементов
            int routeId = (int)comboBox1.SelectedValue;
            int transportTypeId = (int)comboBox2.SelectedValue;
            decimal tripCost = numericUpDown1.Value;

            string query;

            if (_costId.HasValue)
            {
                // Обновление существующей записи
                query = $@"UPDATE trip_cost
                           SET route_id = {routeId}, 
                               transport_type_id = {transportTypeId}, 
                               trip_price = {tripCost}
                           WHERE cost_id = {_costId.Value};";
            }
            else
            {
                // Добавление новой записи
                query = $@"INSERT INTO trip_cost (route_id, transport_type_id, trip_price) 
                           VALUES ({routeId}, {transportTypeId}, {tripCost});";
            }

            int result = _dbHelper.ExecuteNonQuery(query);

            if (result > 0)
            {
                MessageBox.Show(_costId.HasValue ? "Данные успешно обновлены." : "Данные успешно добавлены.");
                this.DialogResult = DialogResult.OK; // Закрываем форму с результатом OK
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка сохранения данных.");
            }
        }

    }
}
