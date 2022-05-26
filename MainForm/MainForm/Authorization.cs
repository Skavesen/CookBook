using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace MainForm
{
    public partial class Authorization : Form
    {
        private NpgsqlConnection npgSqlConnection;
        NpgsqlCommand sqlCommand;
        private string sql = "";
        public Authorization()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)//Русский
            {
                LanguagesForAddingRecipe.isRu = true;
            }

            if (comboBox1.SelectedIndex == 1)//Английский
            {
                LanguagesForAddingRecipe.isRu = false;
            }

            languageChanges();
        }
        public void languageChanges()//Смена языка в приложении
        {
            button1.Text = LanguagesForAddingRecipe.isRu ? "Регистрация" : "Sign Up";

            button2.Text = LanguagesForAddingRecipe.isRu ? "Вход" : "Sign In";

            label1.Text = LanguagesForAddingRecipe.isRu ? "Вкусно как дома" : "Delicious as at home";

            label2.Text = LanguagesForAddingRecipe.isRu ? "Зарегистрироваться" : "Sign Up";

            label3.Text = LanguagesForAddingRecipe.isRu ? "Имя пользователя:" : "User Name:";

            label4.Text = LanguagesForAddingRecipe.isRu ? "Пароль:" : "Password:";            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Registration registration = new Registration();
            registration.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Server = localhost;" + "Port = 5432;" + "Database = Cook;" + "User Id = postgres;" + "Password = postgres;";
            try
            {
                if (npgSqlConnection != null && npgSqlConnection.State != ConnectionState.Closed)
                {
                    npgSqlConnection.Close();
                }
                npgSqlConnection = new NpgsqlConnection(connectionString);
                npgSqlConnection.Open();

                sql = "select * from u_login(:_username, :_password)";
                sqlCommand = new NpgsqlCommand(sql, npgSqlConnection);
                sqlCommand.Parameters.AddWithValue("_username", textBox1.Text);
                sqlCommand.Parameters.AddWithValue("_password", textBox2.Text);

                int result = (int)sqlCommand.ExecuteScalar();
                npgSqlConnection.Close();

                if (result == 1)
                {
                    Hide();
                    new MainForm(textBox1.Text, textBox2.Text).Show();
                }
                else
                {
                    MessageBox.Show(LanguagesForAddingRecipe.isRu ? "Проверьте логин или пароль." : "Check your username or password.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                npgSqlConnection.Close();
            }
        }
    }
}
