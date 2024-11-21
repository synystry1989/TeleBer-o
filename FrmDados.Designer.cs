namespace TeleBerço
{
    partial class FrmDados
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDados));
            this.tsDados = new System.Windows.Forms.ToolStrip();
            this.BtnOk = new System.Windows.Forms.ToolStripButton();
            this.BtnEditar = new System.Windows.Forms.ToolStripButton();
            this.BtnAdicionar = new System.Windows.Forms.ToolStripButton();
            this.BtnSair = new System.Windows.Forms.ToolStripButton();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.DgridDados = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.Label1 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.cbOrdenar = new System.Windows.Forms.ComboBox();
            this.cbFiltro = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnPesquisar = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tsDados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgridDados)).BeginInit();
            this.SuspendLayout();
            // 
            // tsDados
            // 
            this.tsDados.AutoSize = false;
            this.tsDados.BackColor = System.Drawing.Color.Black;
            this.tsDados.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsDados.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.tsDados.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnOk,
            this.BtnEditar,
            this.BtnAdicionar,
            this.BtnSair,
            this.toolStripMenuItem9});
            this.tsDados.Location = new System.Drawing.Point(0, 0);
            this.tsDados.Name = "tsDados";
            this.tsDados.Size = new System.Drawing.Size(1319, 50);
            this.tsDados.TabIndex = 1;
            this.tsDados.Text = "toolStrip1";
            // 
            // BtnOk
            // 
            this.BtnOk.AutoSize = false;
            this.BtnOk.BackColor = System.Drawing.Color.Black;
            this.BtnOk.Font = new System.Drawing.Font("Lucida Fax", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOk.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnOk.Image = global::TeleBerço.Properties.Resources.btnSalvarInativo;
            this.BtnOk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnOk.Margin = new System.Windows.Forms.Padding(10, 10, 0, 2);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(65, 40);
            this.BtnOk.Text = "&Ok";
            this.BtnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnEditar
            // 
            this.BtnEditar.AutoSize = false;
            this.BtnEditar.Font = new System.Drawing.Font("Lucida Fax", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnEditar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnEditar.Image = global::TeleBerço.Properties.Resources.message_file_documents_document_pen_edit_pen_paper_write_icon_210833;
            this.BtnEditar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnEditar.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.BtnEditar.Name = "BtnEditar";
            this.BtnEditar.Size = new System.Drawing.Size(75, 35);
            this.BtnEditar.Text = "&Editar";
            this.BtnEditar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnEditar.Click += new System.EventHandler(this.BtnEditar_Click);
            // 
            // BtnAdicionar
            // 
            this.BtnAdicionar.AutoSize = false;
            this.BtnAdicionar.Font = new System.Drawing.Font("Lucida Fax", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAdicionar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnAdicionar.Image = global::TeleBerço.Properties.Resources.add_yellow_button_icon_227864;
            this.BtnAdicionar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnAdicionar.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.BtnAdicionar.Name = "BtnAdicionar";
            this.BtnAdicionar.Size = new System.Drawing.Size(100, 40);
            this.BtnAdicionar.Text = "Adicionar";
            this.BtnAdicionar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnAdicionar.Click += new System.EventHandler(this.BtnAdicionar_Click);
            // 
            // BtnSair
            // 
            this.BtnSair.AutoSize = false;
            this.BtnSair.Font = new System.Drawing.Font("Lucida Fax", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSair.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnSair.Image = global::TeleBerço.Properties.Resources.arrow_back_previous_left_return_undo_icon_258802;
            this.BtnSair.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnSair.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.BtnSair.Name = "BtnSair";
            this.BtnSair.Size = new System.Drawing.Size(75, 40);
            this.BtnSair.Text = "Voltar";
            this.BtnSair.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSair.Click += new System.EventHandler(this.BtnSair_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripMenuItem9.BackColor = System.Drawing.Color.Black;
            this.toolStripMenuItem9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem9.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.toolStripMenuItem9.Image = global::TeleBerço.Properties.Resources.business_man_user_support_supportfortheuser_aquestion_theclient_2330__1_;
            this.toolStripMenuItem9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripMenuItem9.Margin = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(72, 50);
            this.toolStripMenuItem9.Text = "Info";
            // 
            // DgridDados
            // 
            this.DgridDados.AllowUserToAddRows = false;
            this.DgridDados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgridDados.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgridDados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DgridDados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgridDados.DefaultCellStyle = dataGridViewCellStyle2;
            this.DgridDados.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DgridDados.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DgridDados.Location = new System.Drawing.Point(0, 137);
            this.DgridDados.Name = "DgridDados";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgridDados.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DgridDados.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgridDados.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.DgridDados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgridDados.Size = new System.Drawing.Size(1319, 571);
            this.DgridDados.TabIndex = 2;
            this.DgridDados.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgridDados_KeyDown_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Enabled = false;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(548, 102);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 17);
            this.label5.TabIndex = 152;
            this.label5.Text = "a";
            this.label5.Visible = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(568, 94);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(104, 25);
            this.dateTimePicker1.TabIndex = 5;
            this.dateTimePicker1.Value = new System.DateTime(2024, 9, 30, 0, 0, 0, 0);
            this.dateTimePicker1.Visible = false;
            this.dateTimePicker1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker1_KeyDown);
            // 
            // Label1
            // 
            this.Label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.Black;
            this.Label1.Location = new System.Drawing.Point(1103, 62);
            this.Label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(81, 20);
            this.Label1.TabIndex = 142;
            this.Label1.Text = "Pesquisar :";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(435, 94);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(108, 25);
            this.dateTimePicker2.TabIndex = 4;
            this.dateTimePicker2.Value = new System.DateTime(2024, 9, 30, 0, 0, 0, 0);
            this.dateTimePicker2.Visible = false;
            this.dateTimePicker2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker2_KeyDown);
            // 
            // txtNome
            // 
            this.txtNome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtNome.Location = new System.Drawing.Point(937, 94);
            this.txtNome.Margin = new System.Windows.Forms.Padding(2);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(331, 29);
            this.txtNome.TabIndex = 1;
            this.txtNome.UseWaitCursor = true;
            this.txtNome.TextChanged += new System.EventHandler(this.txtNome_TextChanged);
            // 
            // cbOrdenar
            // 
            this.cbOrdenar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOrdenar.FormattingEnabled = true;
            this.cbOrdenar.Location = new System.Drawing.Point(15, 94);
            this.cbOrdenar.Name = "cbOrdenar";
            this.cbOrdenar.Size = new System.Drawing.Size(160, 25);
            this.cbOrdenar.TabIndex = 3;
            this.cbOrdenar.SelectedIndexChanged += new System.EventHandler(this.cbOrdenar_SelectedIndexChanged);
            // 
            // cbFiltro
            // 
            this.cbFiltro.Enabled = false;
            this.cbFiltro.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbFiltro.FormattingEnabled = true;
            this.cbFiltro.Location = new System.Drawing.Point(181, 94);
            this.cbFiltro.Name = "cbFiltro";
            this.cbFiltro.Size = new System.Drawing.Size(177, 25);
            this.cbFiltro.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(11, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 156;
            this.label2.Text = "Filtrar por :";
            // 
            // btnAplicar
            // 
            this.btnAplicar.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAplicar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnAplicar.FlatAppearance.BorderSize = 2;
            this.btnAplicar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAplicar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAplicar.Location = new System.Drawing.Point(364, 94);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(51, 28);
            this.btnAplicar.TabIndex = 5;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = false;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPesquisar.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnPesquisar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnPesquisar.FlatAppearance.BorderSize = 2;
            this.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPesquisar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnPesquisar.Location = new System.Drawing.Point(1273, 95);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(34, 28);
            this.btnPesquisar.TabIndex = 2;
            this.btnPesquisar.Text = "OK";
            this.btnPesquisar.UseVisualStyleBackColor = false;
            this.btnPesquisar.Click += new System.EventHandler(this.btnPesquisar_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = global::TeleBerço.Properties.Resources.add_yellow_button_icon_227864;
            this.button1.Location = new System.Drawing.Point(900, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 23);
            this.button1.TabIndex = 157;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmDados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1319, 708);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnPesquisar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbFiltro);
            this.Controls.Add(this.cbOrdenar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.DgridDados);
            this.Controls.Add(this.tsDados);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dados";
            this.tsDados.ResumeLayout(false);
            this.tsDados.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgridDados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ToolStrip tsDados;
        internal System.Windows.Forms.ToolStripButton BtnOk;
        internal System.Windows.Forms.ToolStripButton BtnEditar;
        internal System.Windows.Forms.ToolStripButton BtnAdicionar;
        internal System.Windows.Forms.ToolStripButton BtnSair;
        private System.Windows.Forms.DataGridView DgridDados;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        internal System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.ComboBox cbOrdenar;
        private System.Windows.Forms.ComboBox cbFiltro;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnPesquisar;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.Button button1;
    }
}