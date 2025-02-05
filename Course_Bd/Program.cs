using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_Bd
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //string connectionString = "Host=localhost;Username=postgres;Password=12345;Database=CityTransport";
            //Form1 mainForm = new Form1(connectionString);
            //Application.Run(mainForm);

            FormLogin loginForm = new FormLogin();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                string connectionString = loginForm.ConnectionString;
                string userRole = loginForm.UserRole;

                // Передаем строку подключения в основную форму
                Form1 mainForm = new Form1(connectionString, userRole);
                Application.Run(mainForm);
            }
        }
    }
}
