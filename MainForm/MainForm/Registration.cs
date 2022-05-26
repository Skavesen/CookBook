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
    public partial class Registration : Form
    {
        private NpgsqlConnection npgSqlConnection;
        NpgsqlCommand sqlCommand;
        private string sql = "";
        public Registration()
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

            label1.Text = LanguagesForAddingRecipe.isRu ? "Вкусно как дома" : "Delicious as at home";

            label2.Text = LanguagesForAddingRecipe.isRu ? "Зарегистрироваться" : "Sign Up";

            label3.Text = LanguagesForAddingRecipe.isRu ? "Имя пользователя:" : "User Name:";

            label4.Text = LanguagesForAddingRecipe.isRu ? "Пароль:" : "Password:";

            label5.Text = LanguagesForAddingRecipe.isRu ? "Повторите пароль:" : "Re-Enter Password:";

            label7.Text = LanguagesForAddingRecipe.isRu ? "Я согласен с условиями" : "I Agree Terms and Conditions";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || checkBox1.Checked == false)
            {
                MessageBox.Show(LanguagesForAddingRecipe.isRu ? "Проверьте заполненные поля." : "Check the completed fields.");
            }
            else
            {
                if(textBox2.Text != textBox3.Text)
                {
                    MessageBox.Show(LanguagesForAddingRecipe.isRu ? "Пароли не совпадают." : "Passwords don't match.");
                }
                else
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

                        sql = "INSERT INTO \"Login\"(\"Username\",\"Password\") VALUES('" + textBox1.Text + "', '" + textBox2.Text + "');";
                        sqlCommand = new NpgsqlCommand(sql, npgSqlConnection);
                        sqlCommand.ExecuteNonQuery();
                        npgSqlConnection.Close();
                        MessageBox.Show(LanguagesForAddingRecipe.isRu ? "Создана учётная запись." : "Account created.");
                        Hide();
                        new MainForm(textBox1.Text, textBox2.Text).Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        npgSqlConnection.Close();
                    }
                }
            }

        }
    }
}
