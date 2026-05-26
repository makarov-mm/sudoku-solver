namespace SudokuSolver
{
    partial class SudokuSolver
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnSolve = new Button();
            btnReset = new Button();
            btnExample = new Button();
            pnlMain = new Panel();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(btnSolve);
            flowLayoutPanel1.Controls.Add(btnReset);
            flowLayoutPanel1.Controls.Add(btnExample);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.Location = new Point(0, 859);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(800, 41);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // btnSolve
            // 
            btnSolve.BackColor = SystemColors.ActiveCaption;
            btnSolve.FlatStyle = FlatStyle.Flat;
            btnSolve.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSolve.ForeColor = Color.Black;
            btnSolve.Location = new Point(3, 3);
            btnSolve.Name = "btnSolve";
            btnSolve.Size = new Size(100, 35);
            btnSolve.TabIndex = 0;
            btnSolve.Text = "Solve";
            btnSolve.UseVisualStyleBackColor = false;
            btnSolve.Click += btnSolve_Click;
            // 
            // btnReset
            // 
            btnReset.BackColor = SystemColors.ActiveCaption;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Location = new Point(109, 3);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(100, 35);
            btnReset.TabIndex = 1;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // btnExample
            // 
            btnExample.BackColor = SystemColors.ActiveCaption;
            btnExample.FlatStyle = FlatStyle.Flat;
            btnExample.Location = new Point(215, 3);
            btnExample.Name = "btnExample";
            btnExample.Size = new Size(100, 35);
            btnExample.TabIndex = 2;
            btnExample.Text = "Example";
            btnExample.UseVisualStyleBackColor = false;
            btnExample.Click += btnExample_Click;
            // 
            // pnlMain
            // 
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(800, 859);
            pnlMain.TabIndex = 1;
            pnlMain.Paint += pnlMain_Paint;
            pnlMain.Resize += pnlMain_Resize;
            // 
            // SudokuSolver
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 900);
            Controls.Add(pnlMain);
            Controls.Add(flowLayoutPanel1);
            Name = "SudokuSolver";
            Text = "SudokuSolver";
            Load += SudokuSolver_Load;
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnSolve;
        private Button btnReset;
        private Button btnExample;
        private Panel pnlMain;
    }
}
