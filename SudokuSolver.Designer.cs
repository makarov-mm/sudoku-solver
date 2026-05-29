namespace SudokuSolver
{
    partial class SudokuSolver
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            bottomBar = new FlowLayoutPanel();
            btnSolve = new Button();
            btnClear = new Button();
            btnNew = new Button();
            lblDifficulty = new Label();
            cmbDifficulty = new ComboBox();
            lblStatus = new Label();
            pnlMain = new DoubleBufferedPanel();
            bottomBar.SuspendLayout();
            SuspendLayout();
            // 
            // bottomBar
            // 
            bottomBar.AutoSize = true;
            bottomBar.BackColor = Color.FromArgb(238, 242, 248);
            bottomBar.Controls.Add(btnSolve);
            bottomBar.Controls.Add(btnClear);
            bottomBar.Controls.Add(btnNew);
            bottomBar.Controls.Add(lblDifficulty);
            bottomBar.Controls.Add(cmbDifficulty);
            bottomBar.Controls.Add(lblStatus);
            bottomBar.Dock = DockStyle.Bottom;
            bottomBar.Location = new Point(0, 559);
            bottomBar.Name = "bottomBar";
            bottomBar.Padding = new Padding(10, 8, 10, 8);
            bottomBar.Size = new Size(600, 53);
            bottomBar.TabIndex = 1;
            bottomBar.WrapContents = false;
            // 
            // btnSolve
            // 
            btnSolve.BackColor = Color.FromArgb(52, 120, 200);
            btnSolve.FlatAppearance.BorderSize = 0;
            btnSolve.FlatStyle = FlatStyle.Flat;
            btnSolve.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSolve.ForeColor = Color.White;
            btnSolve.Location = new Point(13, 11);
            btnSolve.Name = "btnSolve";
            btnSolve.Size = new Size(96, 32);
            btnSolve.TabIndex = 0;
            btnSolve.Text = "Solve";
            btnSolve.UseVisualStyleBackColor = false;
            btnSolve.Click += btnSolve_Click;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.White;
            btnClear.FlatAppearance.BorderColor = Color.FromArgb(176, 190, 210);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 9.5F);
            btnClear.ForeColor = Color.FromArgb(40, 56, 78);
            btnClear.Location = new Point(115, 11);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(96, 32);
            btnClear.TabIndex = 1;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // btnNew
            // 
            btnNew.BackColor = Color.White;
            btnNew.FlatAppearance.BorderColor = Color.FromArgb(176, 190, 210);
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Segoe UI", 9.5F);
            btnNew.ForeColor = Color.FromArgb(40, 56, 78);
            btnNew.Location = new Point(217, 11);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(110, 32);
            btnNew.TabIndex = 2;
            btnNew.Text = "New puzzle";
            btnNew.UseVisualStyleBackColor = false;
            btnNew.Click += btnNew_Click;
            // 
            // lblDifficulty
            // 
            lblDifficulty.Anchor = AnchorStyles.Left;
            lblDifficulty.AutoSize = true;
            lblDifficulty.Font = new Font("Segoe UI", 9.5F);
            lblDifficulty.ForeColor = Color.FromArgb(70, 86, 108);
            lblDifficulty.Location = new Point(333, 18);
            lblDifficulty.Margin = new Padding(6, 0, 3, 0);
            lblDifficulty.Name = "lblDifficulty";
            lblDifficulty.Size = new Size(58, 17);
            lblDifficulty.TabIndex = 3;
            lblDifficulty.Text = "Difficulty";
            lblDifficulty.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbDifficulty
            // 
            cmbDifficulty.Anchor = AnchorStyles.Left;
            cmbDifficulty.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDifficulty.Font = new Font("Segoe UI", 9.5F);
            cmbDifficulty.Items.AddRange(new object[] { "Easy", "Medium", "Hard", "Expert" });
            cmbDifficulty.Location = new Point(397, 13);
            cmbDifficulty.Name = "cmbDifficulty";
            cmbDifficulty.Size = new Size(100, 25);
            cmbDifficulty.TabIndex = 4;
            cmbDifficulty.SelectedIndexChanged += cmbDifficulty_SelectedIndexChanged;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Left;
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9.5F);
            lblStatus.ForeColor = Color.FromArgb(52, 90, 140);
            lblStatus.Location = new Point(503, 18);
            lblStatus.Margin = new Padding(10, 0, 3, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 17);
            lblStatus.TabIndex = 5;
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(247, 249, 252);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(600, 559);
            pnlMain.TabIndex = 0;
            pnlMain.Paint += pnlMain_Paint;
            pnlMain.MouseClick += pnlMain_MouseClick;
            pnlMain.MouseMove += pnlMain_MouseMove;
            pnlMain.MouseLeave += pnlMain_MouseLeave;
            pnlMain.Resize += pnlMain_Resize;
            // 
            // SudokuSolver
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 612);
            Controls.Add(pnlMain);
            Controls.Add(bottomBar);
            KeyPreview = true;
            MinimumSize = new Size(420, 520);
            Name = "SudokuSolver";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sudoku";
            KeyDown += SudokuSolver_KeyDown;
            Load += SudokuSolver_Load;
            bottomBar.ResumeLayout(false);
            bottomBar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel bottomBar;
        private Button btnSolve;
        private Button btnClear;
        private Button btnNew;
        private Label lblDifficulty;
        private ComboBox cmbDifficulty;
        private Label lblStatus;
        private DoubleBufferedPanel pnlMain;
    }
}
