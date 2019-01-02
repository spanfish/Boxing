/*
 * Created by SharpDevelop.
 * User: xwang
 * Date: 2018/12/27
 * Time: 14:05
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace PackingTracker.UI
{
	partial class PackingFrm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox boxCapacityTextBox;
		private System.Windows.Forms.Button applyBoxButton;
		private System.Windows.Forms.Button packingCompButon;
		private System.Windows.Forms.TextBox deviceInputTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button printLabelButon;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox boxSNTextBox;
		private System.Windows.Forms.TextBox orderIdTextBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox boxTypeTextBox;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.boxCapacityTextBox = new System.Windows.Forms.TextBox();
            this.applyBoxButton = new System.Windows.Forms.Button();
            this.packingCompButon = new System.Windows.Forms.Button();
            this.deviceInputTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.printLabelButon = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.boxSNTextBox = new System.Windows.Forms.TextBox();
            this.orderIdTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.boxTypeTextBox = new System.Windows.Forms.TextBox();
            this.realCountTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.boxSizeTextBox = new System.Windows.Forms.TextBox();
            this.deviceDataGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.msgLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.deviceDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "申请数量";
            // 
            // boxCapacityTextBox
            // 
            this.boxCapacityTextBox.Location = new System.Drawing.Point(92, 60);
            this.boxCapacityTextBox.Name = "boxCapacityTextBox";
            this.boxCapacityTextBox.Size = new System.Drawing.Size(66, 19);
            this.boxCapacityTextBox.TabIndex = 1;
            this.boxCapacityTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BoxCapacityTextBoxKeyDown);
            this.boxCapacityTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BoxCapacityTextBoxKeyPress);
            // 
            // applyBoxButton
            // 
            this.applyBoxButton.Location = new System.Drawing.Point(296, 60);
            this.applyBoxButton.Name = "applyBoxButton";
            this.applyBoxButton.Size = new System.Drawing.Size(75, 23);
            this.applyBoxButton.TabIndex = 2;
            this.applyBoxButton.Text = "申请箱号";
            this.applyBoxButton.UseVisualStyleBackColor = true;
            this.applyBoxButton.Click += new System.EventHandler(this.ApplyBoxButtonClick);
            // 
            // packingCompButon
            // 
            this.packingCompButon.Location = new System.Drawing.Point(631, 649);
            this.packingCompButon.Name = "packingCompButon";
            this.packingCompButon.Size = new System.Drawing.Size(77, 23);
            this.packingCompButon.TabIndex = 3;
            this.packingCompButon.Text = "完成装箱";
            this.packingCompButon.UseVisualStyleBackColor = true;
            this.packingCompButon.Click += new System.EventHandler(this.PackingCompButonClick);
            // 
            // deviceInputTextBox
            // 
            this.deviceInputTextBox.Location = new System.Drawing.Point(105, 650);
            this.deviceInputTextBox.Name = "deviceInputTextBox";
            this.deviceInputTextBox.Size = new System.Drawing.Size(290, 19);
            this.deviceInputTextBox.TabIndex = 4;
            this.deviceInputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DeviceInputTextBoxKeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 654);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "请输入DID/MAC";
            // 
            // printLabelButon
            // 
            this.printLabelButon.Location = new System.Drawing.Point(714, 649);
            this.printLabelButon.Name = "printLabelButon";
            this.printLabelButon.Size = new System.Drawing.Size(79, 23);
            this.printLabelButon.TabIndex = 6;
            this.printLabelButon.Text = "打印标签";
            this.printLabelButon.UseVisualStyleBackColor = true;
            this.printLabelButon.Click += new System.EventHandler(this.printLabelButon_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "订单号";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 23);
            this.label4.TabIndex = 9;
            this.label4.Text = "箱号";
            // 
            // boxSNTextBox
            // 
            this.boxSNTextBox.Location = new System.Drawing.Point(92, 90);
            this.boxSNTextBox.Name = "boxSNTextBox";
            this.boxSNTextBox.ReadOnly = true;
            this.boxSNTextBox.Size = new System.Drawing.Size(279, 19);
            this.boxSNTextBox.TabIndex = 10;
            // 
            // orderIdTextBox
            // 
            this.orderIdTextBox.Location = new System.Drawing.Point(92, 1);
            this.orderIdTextBox.Name = "orderIdTextBox";
            this.orderIdTextBox.ReadOnly = true;
            this.orderIdTextBox.Size = new System.Drawing.Size(279, 19);
            this.orderIdTextBox.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 23);
            this.label5.TabIndex = 13;
            this.label5.Text = "箱子类型";
            // 
            // boxTypeTextBox
            // 
            this.boxTypeTextBox.Location = new System.Drawing.Point(92, 31);
            this.boxTypeTextBox.Name = "boxTypeTextBox";
            this.boxTypeTextBox.ReadOnly = true;
            this.boxTypeTextBox.Size = new System.Drawing.Size(279, 19);
            this.boxTypeTextBox.TabIndex = 14;
            // 
            // realCountTextBox
            // 
            this.realCountTextBox.BackColor = System.Drawing.SystemColors.Highlight;
            this.realCountTextBox.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.realCountTextBox.ForeColor = System.Drawing.Color.White;
            this.realCountTextBox.Location = new System.Drawing.Point(575, 650);
            this.realCountTextBox.Name = "realCountTextBox";
            this.realCountTextBox.ReadOnly = true;
            this.realCountTextBox.Size = new System.Drawing.Size(50, 26);
            this.realCountTextBox.TabIndex = 19;
            this.realCountTextBox.Text = "11";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(401, 653);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "申请数量";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(516, 653);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "已装数量";
            // 
            // boxSizeTextBox
            // 
            this.boxSizeTextBox.Location = new System.Drawing.Point(460, 651);
            this.boxSizeTextBox.Name = "boxSizeTextBox";
            this.boxSizeTextBox.ReadOnly = true;
            this.boxSizeTextBox.Size = new System.Drawing.Size(50, 19);
            this.boxSizeTextBox.TabIndex = 21;
            // 
            // deviceDataGridView
            // 
            this.deviceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.deviceDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10});
            this.deviceDataGridView.Location = new System.Drawing.Point(14, 119);
            this.deviceDataGridView.Name = "deviceDataGridView";
            this.deviceDataGridView.ReadOnly = true;
            this.deviceDataGridView.RowTemplate.Height = 21;
            this.deviceDataGridView.ShowEditingIcon = false;
            this.deviceDataGridView.Size = new System.Drawing.Size(775, 521);
            this.deviceDataGridView.TabIndex = 22;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "5";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "6";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "7";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "8";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "9";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "10";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            // 
            // msgLabel
            // 
            this.msgLabel.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.msgLabel.ForeColor = System.Drawing.Color.Red;
            this.msgLabel.Location = new System.Drawing.Point(8, 693);
            this.msgLabel.Name = "msgLabel";
            this.msgLabel.Size = new System.Drawing.Size(781, 47);
            this.msgLabel.TabIndex = 23;
            // 
            // PackingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 749);
            this.Controls.Add(this.msgLabel);
            this.Controls.Add(this.deviceDataGridView);
            this.Controls.Add(this.boxSizeTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.realCountTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.boxTypeTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.orderIdTextBox);
            this.Controls.Add(this.boxSNTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.printLabelButon);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.deviceInputTextBox);
            this.Controls.Add(this.packingCompButon);
            this.Controls.Add(this.applyBoxButton);
            this.Controls.Add(this.boxCapacityTextBox);
            this.Controls.Add(this.label1);
            this.Name = "PackingFrm";
            this.Text = "装箱";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PackingFrmLoad);
            ((System.ComponentModel.ISupportInitialize)(this.deviceDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private System.Windows.Forms.TextBox realCountTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox boxSizeTextBox;
        private System.Windows.Forms.DataGridView deviceDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.Label msgLabel;
    }
}
