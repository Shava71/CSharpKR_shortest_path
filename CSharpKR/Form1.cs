using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

    }
}
