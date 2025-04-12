using System.Drawing;
using System.Windows.Forms;

namespace CSharpKR
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.addDot = new System.Windows.Forms.Button();
            this.graphBox = new System.Windows.Forms.PictureBox();
            this.amountOfDots = new System.Windows.Forms.TextBox();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.boxLast_1 = new System.Windows.Forms.TextBox();
            this.boxLast_2 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.comboBoxAlgorithm = new System.Windows.Forms.ComboBox();
            this.panelGraph = new System.Windows.Forms.Panel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridViewMatrix = new System.Windows.Forms.DataGridView();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonImport = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.graphBox)).BeginInit();
            this.panelGraph.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatrix)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(33, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Матрица смежности графа";
            // 
            // addDot
            // 
            this.addDot.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.addDot.Location = new System.Drawing.Point(55, 458);
            this.addDot.Name = "addDot";
            this.addDot.Size = new System.Drawing.Size(204, 46);
            this.addDot.TabIndex = 2;
            this.addDot.Text = "Добавить вершину";
            this.addDot.UseVisualStyleBackColor = true;
            // 
            // graphBox
            // 
            this.graphBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.graphBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphBox.Location = new System.Drawing.Point(0, 0);
            this.graphBox.Name = "graphBox";
            this.graphBox.Size = new System.Drawing.Size(438, 423);
            this.graphBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.graphBox.TabIndex = 3;
            this.graphBox.TabStop = false;
            // 
            // amountOfDots
            // 
            this.amountOfDots.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.amountOfDots.Location = new System.Drawing.Point(78, 28);
            this.amountOfDots.Name = "amountOfDots";
            this.amountOfDots.Size = new System.Drawing.Size(140, 29);
            this.amountOfDots.TabIndex = 6;
            // 
            // buttonCreate
            // 
            this.buttonCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.buttonCreate.Location = new System.Drawing.Point(78, 63);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(140, 33);
            this.buttonCreate.TabIndex = 7;
            this.buttonCreate.Text = "Построить";
            this.buttonCreate.UseVisualStyleBackColor = true;
            // 
            // boxLast_1
            // 
            this.boxLast_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.boxLast_1.Location = new System.Drawing.Point(51, 475);
            this.boxLast_1.Multiline = true;
            this.boxLast_1.Name = "boxLast_1";
            this.boxLast_1.Size = new System.Drawing.Size(50, 34);
            this.boxLast_1.TabIndex = 8;
            // 
            // boxLast_2
            // 
            this.boxLast_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.boxLast_2.Location = new System.Drawing.Point(189, 475);
            this.boxLast_2.Multiline = true;
            this.boxLast_2.Name = "boxLast_2";
            this.boxLast_2.Size = new System.Drawing.Size(58, 34);
            this.boxLast_2.TabIndex = 9;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.richTextBox1.Location = new System.Drawing.Point(25, 454);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(393, 55);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.Text = "Для добавления ребра щёлкните мышью вершины, которые оно должно соединить";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.Color.White;
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.richTextBox2.Location = new System.Drawing.Point(38, 3);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox2.Size = new System.Drawing.Size(231, 29);
            this.richTextBox2.TabIndex = 11;
            this.richTextBox2.Text = "Число вершин графа (n)";
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.Color.White;
            this.richTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox3.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.richTextBox3.Location = new System.Drawing.Point(51, 444);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox3.Size = new System.Drawing.Size(196, 25);
            this.richTextBox3.TabIndex = 13;
            this.richTextBox3.Text = "Соединенные точки:";
            // 
            // comboBoxAlgorithm
            // 
            this.comboBoxAlgorithm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxAlgorithm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.comboBoxAlgorithm.FormattingEnabled = true;
            this.comboBoxAlgorithm.Items.AddRange(new object[] {
            "Алгоритм Дейкстры",
            "Алгоритм Прима",
            "Алгоритм Флойда"});
            this.comboBoxAlgorithm.Location = new System.Drawing.Point(3, 112);
            this.comboBoxAlgorithm.Name = "comboBoxAlgorithm";
            this.comboBoxAlgorithm.Size = new System.Drawing.Size(292, 28);
            this.comboBoxAlgorithm.Sorted = true;
            this.comboBoxAlgorithm.TabIndex = 14;
            this.comboBoxAlgorithm.Text = "Алгоритм поиска кратчайшего пути";
            // 
            // panelGraph
            // 
            this.panelGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGraph.Controls.Add(this.graphBox);
            this.panelGraph.Location = new System.Drawing.Point(2, 27);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(440, 425);
            this.panelGraph.TabIndex = 15;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel3);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel2);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1079, 544);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1079, 544);
            this.toolStripContainer1.TabIndex = 16;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.richTextBox1);
            this.panel2.Controls.Add(this.panelGraph);
            this.panel2.Location = new System.Drawing.Point(324, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(445, 523);
            this.panel2.TabIndex = 19;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dataGridViewMatrix);
            this.panel1.Controls.Add(this.addDot);
            this.panel1.Location = new System.Drawing.Point(13, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(305, 524);
            this.panel1.TabIndex = 18;
            // 
            // dataGridViewMatrix
            // 
            this.dataGridViewMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMatrix.Location = new System.Drawing.Point(19, 27);
            this.dataGridViewMatrix.Name = "dataGridViewMatrix";
            this.dataGridViewMatrix.Size = new System.Drawing.Size(268, 425);
            this.dataGridViewMatrix.TabIndex = 17;
            // 
            // buttonImport
            // 
            this.buttonImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonImport.Location = new System.Drawing.Point(56, 407);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(180, 30);
            this.buttonImport.TabIndex = 17;
            this.buttonImport.Text = "Импорт матрицы";
            this.buttonImport.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonImport);
            this.panel3.Controls.Add(this.richTextBox2);
            this.panel3.Controls.Add(this.boxLast_2);
            this.panel3.Controls.Add(this.richTextBox3);
            this.panel3.Controls.Add(this.boxLast_1);
            this.panel3.Controls.Add(this.comboBoxAlgorithm);
            this.panel3.Controls.Add(this.amountOfDots);
            this.panel3.Controls.Add(this.buttonCreate);
            this.panel3.Location = new System.Drawing.Point(772, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(295, 523);
            this.panel3.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1079, 544);
            this.Controls.Add(this.toolStripContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ForeColor = System.Drawing.SystemColors.Desktop;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Матрица смежности графа";
            ((System.ComponentModel.ISupportInitialize)(this.graphBox)).EndInit();
            this.panelGraph.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatrix)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addDot;
        private System.Windows.Forms.PictureBox graphBox;
        private System.Windows.Forms.TextBox amountOfDots;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.TextBox boxLast_1;
        private System.Windows.Forms.TextBox boxLast_2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.ComboBox comboBoxAlgorithm;
        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataGridView dataGridViewMatrix;
        private System.Windows.Forms.Button buttonImport;
        private Panel panel2;
        private Panel panel1;
        private Panel panel3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}