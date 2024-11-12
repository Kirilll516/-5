using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Курсовая1._3
{
    public partial class Question : Form
    {
        public Question()
        {
            InitializeComponent();
            LoadQuestionsAndAnswers();
            UpdateUI();
        }
        
        string connectionString = "Data Source=Cab109,49172;Initial Catalog=master2;Integrated Security=True;Encrypt=False";

        private string[] questions; // Массив для хранения вопросов
        private string[,] answers; // Массив для хранения ответов (вопрос, ответ1, ответ2, ответ3)
        private int currentIndex = 0; // Индекс текущего вопроса           

       
            private void LoadQuestionsAndAnswers()
        {
            int totalQuestions = 4; // Общее количество вопросов
            questions = new string[totalQuestions]; // Инициализируем массив для хранения вопросов
            answers = new string[totalQuestions, 3]; // Инициализируем массив для хранения ответов (4 вопроса, 3 ответа)

            for (int id = 1; id <= totalQuestions; id++)
            {
                questions[id - 1] = LoadQuestion(id); // Загружаем вопрос
                LoadAnswers(id, id - 1); // Загружаем ответы
            }

            // Устанавливаем первый вопрос и ответы в RadioButton
            UpdateUI();
        }

        private void UpdateUI()
        {
            label1.Text = questions[currentIndex]; // Устанавливаем текст вопроса в label1
            radioButton1.Text = answers[currentIndex, 0]; // Устанавливаем текст для первого RadioButton
            radioButton2.Text = answers[currentIndex, 1]; // Устанавливаем текст для второго RadioButton
            radioButton3.Text = answers[currentIndex, 2]; // Устанавливаем текст для третьего RadioButton
            radioButton1.Checked = false; // Сбрасываем выбор
            radioButton2.Checked = false; // Сбрасываем выбор
            radioButton3.Checked = false; // Сбрасываем выбор

            questionNum.Text = $"Вопрос {currentIndex + 1} из {questions.Length}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Изменяем текст в label1 по нажатию кнопки
            currentIndex++;
            if (currentIndex >= questions.Length)
            {
                MessageBox.Show("Викторина завершена!");
                this.Close(); // Закрываем форму или можно перенаправить на другую форму
            }
            else
            {
                UpdateUI(); // Обновляем UI с новым вопросом и ответами 
            }
        }

        private string LoadQuestion(int id)
        {
            string query = "SELECT Question FROM Questions WHERE id = @id"; // Запрос к базе данных

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        connection.Open();
                        string result = command.ExecuteScalar() as string; // Получаем результат запроса
                        return result ?? "Нет данных"; // Возвращаем текст или сообщение о том, что данных нет
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message); // Обработка ошибок
                        return "Ошибка"; // Возвращаем сообщение об ошибке
                    }
                }
            }
        }

        private void LoadAnswers(int questionId, int resultIndex)
        {
            string query = "SELECT Answer FROM Answers1 WHERE QuestionId = @QuestionId"; // Запрос к базе данных

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QuestionId", questionId);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int index = 0;
                            while (reader.Read() && index < 3) // Предполагаем, что максимум 3 ответа
                            {
                                answers[resultIndex, index] = reader["Answer"].ToString();
                                index++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message); // Обработка ошибок
                    }
                }
            }
        }
    }
}
