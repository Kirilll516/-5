using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Курсовая1._3
{

    public partial class Reg : Form
    {
        private string connectionString = "Data Source = Cab109, 49172; Initial Catalog = master2; Integrated Security = True; Encrypt = False";

        public Reg()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Если CheckBox отмечен, показываем пароль
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                // Если CheckBox не отмечен, скрываем пароль
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            string email    = textBox3.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO users (login, Password, email) VALUES (@login, @Password, @email)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@email", email);
                        command.ExecuteNonQuery();
                    }

                    // Отображение сообщения о успешной регистрации
                    MessageBox.Show("Вы зарегистрировались успешно!");

                    // Переход на другую форму
                    menu mainForm = new menu();
                    mainForm.Show();
                    this.Hide(); // Скрыть текущую форму
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }

            }

        }
    }
}
