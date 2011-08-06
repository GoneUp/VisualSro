namespace MapTool.ChildForms
{
    partial class StartWindow
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
            this.comboSpawnLocation = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSpawn = new System.Windows.Forms.Button();
            this.numSpeed = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.panalProgress = new System.Windows.Forms.Panel();
            this.lblCurrentStep = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pbLoadProgress = new System.Windows.Forms.ProgressBar();
            this.bgwProcessNavmesh = new System.ComponentModel.BackgroundWorker();
            this.panalSettings = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).BeginInit();
            this.panalProgress.SuspendLayout();
            this.panalSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Spawn Location:";
            // 
            // comboSpawnLocation
            // 
            this.comboSpawnLocation.FormattingEnabled = true;
            this.comboSpawnLocation.Items.AddRange(new object[] {
            "Jangan",
            "Donwhang",
            "Hotan",
            "Samarkand",
            "Constantinople",
            "Alexandria North",
            "Alexandria South"});
            this.comboSpawnLocation.Location = new System.Drawing.Point(96, 6);
            this.comboSpawnLocation.Name = "comboSpawnLocation";
            this.comboSpawnLocation.Size = new System.Drawing.Size(150, 21);
            this.comboSpawnLocation.TabIndex = 1;
            this.comboSpawnLocation.Text = "Jangan";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(304, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSpawn
            // 
            this.btnSpawn.Location = new System.Drawing.Point(223, 77);
            this.btnSpawn.Name = "btnSpawn";
            this.btnSpawn.Size = new System.Drawing.Size(75, 23);
            this.btnSpawn.TabIndex = 3;
            this.btnSpawn.Text = "&Spawn";
            this.btnSpawn.UseVisualStyleBackColor = true;
            this.btnSpawn.Click += new System.EventHandler(this.btnSpawn_Click);
            // 
            // numSpeed
            // 
            this.numSpeed.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSpeed.Location = new System.Drawing.Point(96, 33);
            this.numSpeed.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSpeed.Name = "numSpeed";
            this.numSpeed.Size = new System.Drawing.Size(75, 20);
            this.numSpeed.TabIndex = 4;
            this.numSpeed.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Initial Speed:";
            // 
            // panalProcess
            // 
            this.panalProgress.Controls.Add(this.lblCurrentStep);
            this.panalProgress.Controls.Add(this.label3);
            this.panalProgress.Controls.Add(this.pbLoadProgress);
            this.panalProgress.Location = new System.Drawing.Point(12, 12);
            this.panalProgress.Name = "panalProcess";
            this.panalProgress.Size = new System.Drawing.Size(370, 49);
            this.panalProgress.TabIndex = 6;
            this.panalProgress.Visible = false;
            // 
            // lblCurrentStep
            // 
            this.lblCurrentStep.AutoSize = true;
            this.lblCurrentStep.Location = new System.Drawing.Point(38, 0);
            this.lblCurrentStep.Name = "lblCurrentStep";
            this.lblCurrentStep.Size = new System.Drawing.Size(61, 13);
            this.lblCurrentStep.TabIndex = 11;
            this.lblCurrentStep.Text = "Preparing...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Step:";
            // 
            // progressBar1
            // 
            this.pbLoadProgress.Location = new System.Drawing.Point(5, 18);
            this.pbLoadProgress.Name = "progressBar1";
            this.pbLoadProgress.Size = new System.Drawing.Size(362, 24);
            this.pbLoadProgress.TabIndex = 9;
            // 
            // bgwProcessNavmesh
            // 
            this.bgwProcessNavmesh.WorkerReportsProgress = true;
            this.bgwProcessNavmesh.WorkerSupportsCancellation = true;
            this.bgwProcessNavmesh.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwProcessNavmesh_DoWork);
            this.bgwProcessNavmesh.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwProcessNavmesh_ProgressChanged);
            this.bgwProcessNavmesh.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwProcessNavmesh_RunWorkerCompleted);
            // 
            // panalSettings
            // 
            this.panalSettings.Controls.Add(this.label1);
            this.panalSettings.Controls.Add(this.label2);
            this.panalSettings.Controls.Add(this.comboSpawnLocation);
            this.panalSettings.Controls.Add(this.numSpeed);
            this.panalSettings.Location = new System.Drawing.Point(12, 12);
            this.panalSettings.Name = "panalSettings";
            this.panalSettings.Size = new System.Drawing.Size(251, 59);
            this.panalSettings.TabIndex = 7;
            // 
            // StartWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 107);
            this.ControlBox = false;
            this.Controls.Add(this.panalSettings);
            this.Controls.Add(this.btnSpawn);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panalProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StartWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MapTool";
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).EndInit();
            this.panalProgress.ResumeLayout(false);
            this.panalProgress.PerformLayout();
            this.panalSettings.ResumeLayout(false);
            this.panalSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboSpawnLocation;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSpawn;
        private System.Windows.Forms.NumericUpDown numSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panalProgress;
        private System.Windows.Forms.Label lblCurrentStep;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pbLoadProgress;
        private System.ComponentModel.BackgroundWorker bgwProcessNavmesh;
        private System.Windows.Forms.Panel panalSettings;
    }
}