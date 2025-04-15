using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpKR
{
    public partial class Form1 : Form
    {
        private Graph graph;

        public Form1()
        {
            InitializeComponent();
            graph = new Graph(graphBox, dataGridViewMatrix, boxLast_1, boxLast_2);

            this.addDot.Click += new EventHandler(addDot_Click);
            this.graphBox.MouseClick += new MouseEventHandler(graphBox_MouseClick);
            this.buttonCreate.Click += new EventHandler(buttonCreate_Click);

            comboBoxAlgorithm.Items.AddRange(graph.GetAllMethods().ToArray());

            // Add button for importing matrix
            var btnImport = new Button
            {
                Text = "Импорт матрицы",
                Font = new Font("Microsoft Sans Serif", 12F),
                Location = new Point(810, 250),
                Size = new Size(180, 30)
            };
            btnImport.Click += BtnImport_Click;
            this.Controls.Add(btnImport);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                // Here you would implement the logic to parse the matrix from a txt file
                // For example:
                // graph.ImportMatrixFromTxt(System.IO.Path.Combine(path, "matrix.txt"));
                MessageBox.Show($"Выбрана папка: {path}\n(Реализуйте парсинг матрицы из файла)");
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (int.TryParse(amountOfDots.Text, out int n) && n >= 1 && n <= 20)
            {
                graph.GenerateGraph(n);
            }
            else
            {
                MessageBox.Show("Введите число вершин в пределах от 1 до 20.");
            }
        }

        private void addDot_Click(object sender, EventArgs e)
        {
            if (graph.VertexCount < 20)
            {
                graph.AddVertex();
                amountOfDots.Text = graph.VertexCount.ToString();
            }
            else
            {
                MessageBox.Show("Максимальное количество вершин - 20.");
            }
        }

        private void graphBox_MouseClick(object sender, MouseEventArgs e)
        {
            graph.HandleMouseClick(e);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (comboBoxAlgorithm.SelectedItem == null)
            {
                MessageBox.Show("Выберите метод!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string choosedAlgorithm = comboBoxAlgorithm.SelectedItem.ToString();
            {
                if (choosedAlgorithm == "Алгоритм Дейкстры" && graph.ContaintMinusElement())
                {
                    MessageBox.Show("В графе присутствуют отрицательные элементы. Алгоритм Дейкстры не подходит", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            Graph.SelectAlgorithm algorithm = choosedAlgorithm switch
            {
                "Алгоритм Дейкстры" => graph.Dijkstra_algorithm,
                "Алгоритм Беллмана-Форда" => graph.BellmanFord_algorithm,
                _ => null
            };
            
            if (algorithm != null)
            {
                graph.RunAlgorithm(algorithm);
            }
            else
            {
                MessageBox.Show("Выбранный алгоритм не найден!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonImport_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Выберите папку с файлами матрицы смежности";
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;

            // Показываем диалог и обрабатываем результат
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog1.SelectedPath;

                // Здесь можно добавить обработку выбранной папки
                // Например:
                string[] matrixFiles = Directory.GetFiles(selectedPath, "*.txt");
                if (matrixFiles.Length > 0)
                {
                    // Загружаем первый найденный файл
                    graph.ParseTxt(matrixFiles[0]);

                    // Или показываем список файлов для выбора
                    MessageBox.Show($"Найдено {matrixFiles.Length} файлов матриц");
                }
                else
                {
                    MessageBox.Show("В выбранной папке нет .txt файлов",
                        "Файлы не найдены", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }
        
    }
}
