using System;
using System.Drawing;
using System.Windows.Forms;

namespace CSharpKR
{
    interface IGraph
    {
        void GenerateGraph(int n);
        void AddVertex();
        void HandleMouseClick(MouseEventArgs e);
        void DrawGraph(int highlightVertex = -1);
    }

    internal class Graph : IGraph
    {
        private int[,] adjacencyMatrix;
        private Point[] points;
        private int selectedVertex = -1;
        private readonly Random random = new Random();

        private readonly PictureBox graphBox;
        private readonly DataGridView matrixBox;
        private readonly TextBox boxLast_1;
        private readonly TextBox boxLast_2;

        public Graph(PictureBox graphBox, DataGridView matrixBox, TextBox boxLast_1, TextBox boxLast_2)
        {
            this.graphBox = graphBox;
            this.matrixBox = matrixBox;
            this.boxLast_1 = boxLast_1;
            this.boxLast_2 = boxLast_2;

            this.matrixBox.CellValueChanged += MatrixBox_CellValueChanged;
        }

        private void MatrixBox_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (int.TryParse(matrixBox.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString(), out int value))
                {
                    adjacencyMatrix[e.RowIndex, e.ColumnIndex] = value;
                    //adjacencyMatrix[e.ColumnIndex, e.RowIndex] = value; 
                    DrawGraph();
                }
            }
        }

        public int VertexCount => adjacencyMatrix?.GetLength(0) ?? 0;

        public void GenerateGraph(int n)
        {
            adjacencyMatrix = new int[n, n];
            InitializeDataGridView(n);
            DrawGraph();
        }

        private void InitializeDataGridView(int n)
        {
            matrixBox.Rows.Clear();
            matrixBox.Columns.Clear();

            for (int i = 0; i < n; i++)
            {
                matrixBox.Columns.Add($"col{i}", (i + 1).ToString());
                matrixBox.Columns[i].Width = 40;
            }

            matrixBox.Rows.Add(n);
            for (int i = 0; i < n; i++)
            {
                matrixBox.Rows[i].HeaderCell.Value = (i + 1).ToString();
                for (int j = 0; j < n; j++)
                {
                    matrixBox.Rows[i].Cells[j].Value = adjacencyMatrix[i, j];
                }
            }
        }

        public void AddVertex()
        {
            int n = VertexCount + 1;
            if (n > 20) return;

            var newMatrix = new int[n, n];
            for (int i = 0; i < VertexCount; i++)
                for (int j = 0; j < VertexCount; j++)
                    newMatrix[i, j] = adjacencyMatrix[i, j];

            adjacencyMatrix = newMatrix;
            InitializeDataGridView(n);
            DrawGraph();
        }

        public void HandleMouseClick(MouseEventArgs e)
        {
            if (points == null || points.Length == 0) return;

            int clickedVertex = -1;
            for (int i = 0; i < points.Length; i++)
            {
                if (Math.Pow(e.X - points[i].X, 2) + Math.Pow(e.Y - points[i].Y, 2) <= 100)
                {
                    clickedVertex = i;
                    break;
                }
            }

            if (clickedVertex == -1) return;

            if (selectedVertex == -1)
            {
                selectedVertex = clickedVertex;
            }
            else
            {
                if (selectedVertex != clickedVertex)
                {
                    // Open dialog to input weight
                    using (var inputDialog = new Form())
                    {
                        inputDialog.Text = "Введите вес ребра";
                        var numericUpDown = new NumericUpDown()
                        {
                            Minimum = 0,
                            Maximum = 100,
                            Location = new Point(10, 10)
                        };
                        var okButton = new Button()
                        {
                            Text = "OK",
                            DialogResult = DialogResult.OK,
                            Location = new Point(10, 40)
                        };
                        inputDialog.Controls.Add(numericUpDown);
                        inputDialog.Controls.Add(okButton);
                        inputDialog.AcceptButton = okButton;

                        if (inputDialog.ShowDialog() == DialogResult.OK)
                        {
                            int weight = (int)numericUpDown.Value;
                            adjacencyMatrix[selectedVertex, clickedVertex] = weight;
                            //adjacencyMatrix[clickedVertex, selectedVertex] = weight;
                            matrixBox.Rows[selectedVertex].Cells[clickedVertex].Value = weight;
                            //matrixBox.Rows[clickedVertex].Cells[selectedVertex].Value = weight;

                            //boxLast_1.Text = (selectedVertex + 1).ToString();
                            //boxLast_2.Text = (clickedVertex + 1).ToString();
                        }
                    }
                }

                selectedVertex = -1;
            }

            DrawGraph(clickedVertex);
        }


        public void DrawGraph(int highlightVertex = -1)
        {
            int n = adjacencyMatrix.GetLength(0);
            Bitmap bmp = new Bitmap(graphBox.Width, graphBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(SystemColors.ButtonHighlight);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // Включаем сглаживание

                using (Pen pen = new Pen(Color.Green))
                using (Brush defaultBrush = new SolidBrush(Color.Yellow))
                using (Brush selectedBrush = new SolidBrush(Color.Blue))
                using (Brush textBrush = new SolidBrush(Color.Black))
                using (Font weightFont = new Font("Arial", 8))
                {
                    int R = Math.Min(graphBox.Width, graphBox.Height) / 2 - 30;
                    Point center = new Point(graphBox.Width / 2, graphBox.Height / 2);
                    points = new Point[n];

                    for (int i = 0; i < n; i++)
                    {
                        double angle = 2 * Math.PI * i / n;
                        int x = center.X + (int)(R * Math.Cos(angle));
                        int y = center.Y + (int)(R * Math.Sin(angle));
                        points[i] = new Point(x, y);
                    }

                    // Draw edges with weights
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++) // Для ориентированного графа проверяем все сочетания
                        {
                            if (adjacencyMatrix[i, j] > 0 && i != j) // Исключаем петли
                            {
                                // Корректируем конечную точку, чтобы стрелка не перекрывала вершину
                                double angle = Math.Atan2(points[j].Y - points[i].Y, points[j].X - points[i].X);
                                Point adjustedEnd = new Point(
                                    points[j].X - (int)(15 * Math.Cos(angle)),
                                    points[j].Y - (int)(15 * Math.Sin(angle)));

                                using (Pen arrowPen = new Pen(Color.Green, 2)) // Увеличиваем толщину
                                {
                                    // Настраиваем стрелку
                                    arrowPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5);
                                    g.DrawLine(arrowPen, points[i], adjustedEnd);

                                    // Draw weight
                                    Point midPoint = new Point(
                                        (points[i].X + adjustedEnd.X) / 2,
                                        (points[i].Y + adjustedEnd.Y) / 2);
                                    g.DrawString(adjacencyMatrix[i, j].ToString(),
                                        weightFont, textBrush, midPoint);
                                }
                            }
                        }
                    }

                    // Draw vertices
                    for (int i = 0; i < n; i++)
                    {
                        Brush currentBrush = (i == selectedVertex || i == highlightVertex) ? selectedBrush : defaultBrush;
                        g.FillEllipse(currentBrush, points[i].X - 15, points[i].Y - 15, 30, 30);
                        g.DrawEllipse(pen, points[i].X - 15, points[i].Y - 15, 30, 30);
                        g.DrawString((i + 1).ToString(), new Font("Microsoft Sans Serif", 12), textBrush,
                                     points[i].X - 5, points[i].Y - 10);
                    }
                }
            }
            graphBox.Image = bmp;
        }
    }
}