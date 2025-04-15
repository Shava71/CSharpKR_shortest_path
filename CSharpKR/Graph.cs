using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;

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
        public delegate (int distance, List<int> path, long Ms) SelectAlgorithm(int startVertex, int endVertex);
        
        private int[,] adjacencyMatrix;
        private Point[] points;
        private int selectedVertex = -1;

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
                            if (adjacencyMatrix[i, j] != 0 && i != j) //Исключаем петли и нуль-веса
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

        public List<string> GetAllMethods()
        {
            var methods = new List<string>();
    
            //Метод Дейсктры в комбоБокс
            var dijkstraMethod = GetType().GetMethod(nameof(Dijkstra_algorithm));
            var dijkstraAttr = dijkstraMethod.GetCustomAttribute<DisplayNameAttribute>();
            methods.Add(dijkstraAttr?.DisplayName ?? nameof(Dijkstra_algorithm));
    
            //Метод Беллмана-Форда в комбоБокс
            var bellmanFordMethod = GetType().GetMethod(nameof(BellmanFord_algorithm));
            var bellmanFordAttr = bellmanFordMethod.GetCustomAttribute<DisplayNameAttribute>();
            methods.Add(bellmanFordAttr?.DisplayName ?? nameof(BellmanFord_algorithm));
    
            return methods;
        }

        public void RunAlgorithm(SelectAlgorithm choosedAlgorithm)
        {
            if (adjacencyMatrix == null || adjacencyMatrix.Length == 0)
            {
                MessageBox.Show("Граф не инициализирован!", "Ошибка", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int vertexCount = adjacencyMatrix.GetLength(0);
            
            using (var runDialog = new Form())
            {
                //Берём названиа метода из атрибута
                string methodName = choosedAlgorithm.Method.Name;
                var methodInfo = GetType().GetMethod(methodName);
                var displayNameAttr = methodInfo.GetCustomAttribute<DisplayNameAttribute>();
                string displayName = displayNameAttr?.DisplayName ?? nameof(choosedAlgorithm);
                
                runDialog.Text = "Запуск алгоритма \"" + displayName + "\"";
                runDialog.StartPosition = FormStartPosition.CenterParent;
                runDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                runDialog.MaximizeBox = false;
                runDialog.MinimizeBox = false;
                runDialog.ClientSize = new Size(300, 200);
                
                var labelStart = new Label()
                {
                    Text = "Выберите начальную вершину (1-" + vertexCount + ")",
                    Location = new Point(20, 20),
                    AutoSize = true
                };

                var labelEnd = new Label()
                {
                    Text = "Выберите конечную вершину (1-" + vertexCount + ")",
                    Location = new Point(20, 70),
                    AutoSize = true
                };

                var inputStart = new NumericUpDown()
                {
                    Minimum = 1,
                    Maximum = vertexCount,
                    Value = 1,
                    Location = new Point(20, 45),
                    Width = 100
                };

                var inputEnd = new NumericUpDown()
                {
                    Minimum = 1,
                    Maximum = vertexCount,
                    Value = vertexCount > 1 ? 2 : 1,
                    Location = new Point(20, 95),
                    Width = 100
                };

                var okButton = new Button()
                {
                    Text = "Запустить",
                    DialogResult = DialogResult.OK,
                    Location = new Point(20, 130),
                    Size = new Size(100, 30)
                };

                var cancelButton = new Button()
                {
                    Text = "Отмена",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(130, 130),
                    Size = new Size(100, 30)
                };

                runDialog.Controls.Add(labelStart);
                runDialog.Controls.Add(labelEnd);
                runDialog.Controls.Add(inputStart);
                runDialog.Controls.Add(inputEnd);
                runDialog.Controls.Add(okButton);
                runDialog.Controls.Add(cancelButton);
                
                runDialog.AcceptButton = okButton;
                runDialog.CancelButton = cancelButton;

                if (runDialog.ShowDialog() == DialogResult.OK)
                {
                    int startVertex = (int)inputStart.Value - 1;
                    int endVertex = (int)inputEnd.Value - 1;
                    
                    var (distance, path, ms) = choosedAlgorithm(startVertex, endVertex); //Вызов делегата выбранного метода из comboBox
                    
                    // Создаем новую форму для отображения результатов
                    ShowResults(startVertex, endVertex, distance, path, ms);
                }
            }
        }
        
        private void ShowResults(int startVertex, int endVertex, int distance, List<int> path, long elapsedMs)
        {
            var resultForm = new Form();
            resultForm.Text = "Результаты алгоритма Дейкстры";
            resultForm.StartPosition = FormStartPosition.CenterParent;
            resultForm.Size = new Size(600, 500); // Увеличили размер формы
            
            // Увеличиваем размер изображения в 1.5 раза
            int scaleFactor = 3;
            int newWidth = graphBox.Width * scaleFactor / 2;
            int newHeight = graphBox.Height * scaleFactor / 2;
            
            // Создаем копию текущего графа с выделенным путем
            Bitmap graphImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(graphImage))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                
                // Рисуем увеличенную версию графа
                using (GraphicsPath pathGraphics = new GraphicsPath())
                {
                    // Вершины
                    for (int i = 0; i < points.Length; i++)
                    {
                        pathGraphics.AddEllipse(
                            points[i].X * scaleFactor / 2 - 15 * scaleFactor / 2, 
                            points[i].Y * scaleFactor / 2 - 15 * scaleFactor / 2, 
                            30 * scaleFactor / 2, 
                            30 * scaleFactor / 2);
                    }
                    
                    // Ребра
                    for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                        {
                            if (adjacencyMatrix[i, j] != 0 && i != j) //убираем петли и нуль-веса
                            {
                                double angle = Math.Atan2(
                                    points[j].Y * scaleFactor / 2 - points[i].Y * scaleFactor / 2,
                                    points[j].X * scaleFactor / 2 - points[i].X * scaleFactor / 2);
                                Point adjustedEnd = new Point(
                                    points[j].X * scaleFactor / 2 - (int)(15 * Math.Cos(angle) * scaleFactor / 2),
                                    points[j].Y * scaleFactor / 2 - (int)(15 * Math.Sin(angle) * scaleFactor / 2));

                                using (Pen arrowPen = new Pen(Color.Green, 2 * scaleFactor / 2))
                                {
                                    arrowPen.CustomEndCap = new AdjustableArrowCap(5 * scaleFactor / 2, 5 * scaleFactor / 2);
                                    g.DrawLine(arrowPen, 
                                        points[i].X * scaleFactor / 2, 
                                        points[i].Y * scaleFactor / 2, 
                                        adjustedEnd.X, 
                                        adjustedEnd.Y);
                
                                    // Draw weight
                                    Point midPoint = new Point(
                                        (points[i].X * scaleFactor / 2 + adjustedEnd.X) / 2,
                                        (points[i].Y * scaleFactor / 2 + adjustedEnd.Y) / 2);
                                    g.DrawString(adjacencyMatrix[i, j].ToString(),
                                        new Font("Arial", 8 * scaleFactor / 2), Brushes.Black, midPoint);
                                }
                            }
                        }
                    }
                    
                    // Рисуем граф
                    g.FillPath(Brushes.Yellow, pathGraphics);
                    g.DrawPath(new Pen(Color.Green, 2 * scaleFactor / 2), pathGraphics);
                    
                    // Выделяем путь
                    if (path.Count > 0)
                    {
                        using (Pen pathPen = new Pen(Color.Red, 3 * scaleFactor / 2))
                        {
                            pathPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5);
                            
                            for (int i = 0; i < path.Count - 1; i++)
                            {
                                int from = path[i];
                                int to = path[i+1];
                                
                                double angle = Math.Atan2(
                                    points[to].Y - points[from].Y, 
                                    points[to].X - points[from].X);
                                Point adjustedEnd = new Point(
                                    points[to].X * scaleFactor / 2 - (int)(15 * Math.Cos(angle) * scaleFactor / 2),
                                    points[to].Y * scaleFactor / 2 - (int)(15 * Math.Sin(angle) * scaleFactor / 2));
                                
                                g.DrawLine(pathPen, 
                                    points[from].X * scaleFactor / 2, 
                                    points[from].Y * scaleFactor / 2, 
                                    adjustedEnd.X, 
                                    adjustedEnd.Y);
                            }
                        }
                        
                        // Выделяем вершины пути
                        using (Brush pathBrush = new SolidBrush(Color.Orange))
                        {
                            foreach (int vertex in path)
                            {
                                g.FillEllipse(pathBrush, 
                                    points[vertex].X * scaleFactor / 2 - 15 * scaleFactor / 2, 
                                    points[vertex].Y * scaleFactor / 2 - 15 * scaleFactor / 2, 
                                    30 * scaleFactor / 2, 
                                    30 * scaleFactor / 2);
                                g.DrawEllipse(new Pen(Color.Red, 2 * scaleFactor / 2), 
                                    points[vertex].X * scaleFactor / 2 - 15 * scaleFactor / 2, 
                                    points[vertex].Y * scaleFactor / 2 - 15 * scaleFactor / 2, 
                                    30 * scaleFactor / 2, 
                                    30 * scaleFactor / 2);
                            }
                        }
                    }
                }
                
                // Подписи вершин
                using (Font vertexFont = new Font("Microsoft Sans Serif", 12 * scaleFactor / 2))
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        g.DrawString((i + 1).ToString(), vertexFont, Brushes.Black,
                                     points[i].X * scaleFactor / 2 - 5 * scaleFactor / 2, 
                                     points[i].Y * scaleFactor / 2 - 10 * scaleFactor / 2);
                    }
                }
            }
            
            // Создаем элементы управления для формы результатов
            var pictureBox = new PictureBox()
            {
                Image = graphImage,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Top,
                Height = 300 // Увеличили высоту PictureBox
            };
            
            var lblStart = new Label()
            {
                Text = $"Начальная вершина: {startVertex + 1}",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Microsoft Sans Serif", 10)
            };
            
            var lblEnd = new Label()
            {
                Text = $"Конечная вершина: {endVertex + 1}",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Microsoft Sans Serif", 10)
            };
            
            var lblDistance = new Label()
            {
                Text = distance == int.MaxValue ? "Путь не существует" : $"Длина пути: {distance}",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Microsoft Sans Serif", 10)
            };
            
            var lblTime = new Label()
            {
                Text = $"Время выполнения: {elapsedMs} мс",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Microsoft Sans Serif", 10)
            };
            
            var lblPath = new Label()
            {
                Text = path.Count > 0 ? $"Путь: {string.Join(" → ", path.Select(v => v + 1))}" : "Путь не найден",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Microsoft Sans Serif", 10)
            };
            
            var panel = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };
            
            panel.Controls.Add(lblPath);
            panel.Controls.Add(lblTime);
            panel.Controls.Add(lblDistance);
            panel.Controls.Add(lblEnd);
            panel.Controls.Add(lblStart);
            
            resultForm.Controls.Add(panel);
            resultForm.Controls.Add(pictureBox);
            
            resultForm.ShowDialog();
        }
        
        
        [DisplayName("Алгоритм Дейкстры")]
        public (int distance, List<int> path, long Ms) Dijkstra_algorithm(int startVertex, int endVertex)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            int count = adjacencyMatrix.GetLength(0);
            
            // Проверка на валидность вершин
            if (startVertex < 0 || startVertex >= count || endVertex < 0 || endVertex >= count)
            {
                MessageBox.Show("Некорректные вершины!", "Ошибка", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (int.MaxValue, new List<int>(), 0);
            }

            int[] dist = new int[count];
            int[] previous = new int[count]; // Для хранения пути
            bool[] sptSet = new bool[count];
            
            for (int i = 0; i < count; i++) 
            {
                dist[i] = int.MaxValue;
                sptSet[i] = false;
                previous[i] = -1;
            }
            
            dist[startVertex] = 0;
            
            for (int i = 0; i < count - 1; i++) 
            {
                int u = MinDistance(dist, sptSet);
                
                // Если достигли конечной вершины или все оставшиеся вершины недостижимы
                if (u == -1 || u == endVertex) 
                    break;
                
                sptSet[u] = true;
                
                for (int j = 0; j < count; j++)
                {
                    if (!sptSet[j] && adjacencyMatrix[u, j] != 0 && 
                        dist[u] != int.MaxValue && 
                        dist[u] + adjacencyMatrix[u, j] < dist[j])
                    {
                        dist[j] = dist[u] + adjacencyMatrix[u, j];
                        previous[j] = u; // Запоминаем предыдущую вершину
                    }
                }
            }

            // Восстанавливаем путь
            List<int> path = new List<int>();
            if (dist[endVertex] != int.MaxValue)
            {
                for (int at = endVertex; at != -1; at = previous[at])
                {
                    path.Add(at);
                }
                path.Reverse(); // Переворачиваем, чтобы путь был от начала до конца
            }
            
            stopwatch.Stop();
            return (dist[endVertex], path, stopwatch.ElapsedMilliseconds);
        }

        private int MinDistance(int[] dist, bool[] sptSet)
        {
            int min = int.MaxValue;
            int minIndex = -1;

            for (int i = 0; i < dist.Length; i++)
            {
                if (!sptSet[i] && dist[i] <= min)
                {
                    min = dist[i];
                    minIndex = i;
                }
            }
            return minIndex;
        }


        [DisplayName("Алгоритм Беллмана-Форда")]
        public (int distance, List<int> path, long Ms) BellmanFord_algorithm(int startVertex, int endVertex)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            int count = adjacencyMatrix.GetLength(0);
            
            //Валидация вершин
            if (startVertex < 0 || startVertex >= count || endVertex < 0 || endVertex >= count)
            {
                MessageBox.Show("Некорректные вершины!", "Ошибка", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (int.MaxValue, new List<int>(), 0);
            }

            int[] dist = new int[count];
            int[] previous = new int[count];
            
            // Инициализация расстояний
            for (int i = 0; i < count; i++)
            {
                dist[i] = int.MaxValue;
                previous[i] = -1;
            }
            dist[startVertex] = 0;

            // Релаксация всех рёбер V-1 раз
            for (int i = 0; i < count - 1; i++)
            {
                for (int u = 0; u < count; u++)
                {
                    for (int v = 0; v < count; v++)
                    {
                        if (adjacencyMatrix[u, v] != 0 && dist[u] != int.MaxValue && 
                            dist[u] + adjacencyMatrix[u, v] < dist[v])
                        {
                            dist[v] = dist[u] + adjacencyMatrix[u, v];
                            previous[v] = u;
                        }
                    }
                }
            }

            // Проверка на отрицательные циклы
            for (int u = 0; u < count; u++)
            {
                for (int v = 0; v < count; v++)
                {
                    if (adjacencyMatrix[u, v] != 0 && dist[u] != int.MaxValue && 
                        dist[u] + adjacencyMatrix[u, v] < dist[v])
                    {
                        MessageBox.Show("Граф содержит цикл отрицательного веса!", "Ошибка",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return (int.MaxValue, new List<int>(), 0);
                    }
                }
            }

            // Восстановление пути
            List<int> path = new List<int>();
            if (dist[endVertex] != int.MaxValue)
            {
                for (int at = endVertex; at != -1; at = previous[at])
                {
                    path.Add(at);
                }
                path.Reverse();
            }

            stopwatch.Stop();
            return (dist[endVertex], path, stopwatch.ElapsedMilliseconds);
        }
        
        
        
        
        
        
        
        
        
        public bool ParseTxt(string filePath)
        {
            try
            {
                // Читаем все строки из файла
                string[] lines = File.ReadAllLines(filePath);
                
                // Определяем размер матрицы (количество строк)
                int size = lines.Length;
                
                // Создаем новую матрицу смежности
                int[,] newMatrix = new int[size, size];
                
                for (int i = 0; i < size; i++)
                {
                    // Разбиваем строку на элементы (разделитель - пробел или запятая)
                    string[] elements = lines[i].Split(new[] { ' ', ',', '\t' }, 
                                                     StringSplitOptions.RemoveEmptyEntries);
                    
                    // Проверяем, что количество элементов соответствует размеру матрицы
                    if (elements.Length != size)
                    {
                        MessageBox.Show($"Ошибка в строке {i+1}: количество элементов не соответствует размеру матрицы", 
                                      "Ошибка формата", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    
                    // Парсим элементы строки
                    for (int j = 0; j < size; j++)
                    {
                        if (!int.TryParse(elements[j], out newMatrix[i, j]))
                        {
                            MessageBox.Show($"Ошибка в строке {i+1}, элемент {j+1}: неверный формат числа", 
                                          "Ошибка формата", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        
                        // // Проверка на отрицательные веса (если нужно)
                        // if (newMatrix[i, j] < 0)
                        // {
                        //     MessageBox.Show($"Ошибка: отрицательный вес в строке {i+1}, элемент {j+1}", 
                        //                   "Ошибка данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //     return false;
                        // }
                    }
                }
                
                // Если все успешно, обновляем матрицу и перерисовываем граф
                adjacencyMatrix = newMatrix;
                InitializeDataGridView(size);
                DrawGraph();
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", 
                              "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool ContaintMinusElement()
        {
            bool contains = false;
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] < 0)
                    {
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }
        
        
    }
}