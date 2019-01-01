/*
 * Created by SharpDevelop.
 * User: xwang
 * Date: 2018/12/27
 * Time: 14:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace PackingTracker.UI
{
	partial class BoxListFrm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button innerBoxButton;
		private System.Windows.Forms.Button devOuterBoxButton;
		private System.Windows.Forms.Button outerBoxButton;
		
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.outerBoxButton = new System.Windows.Forms.Button();
            this.innerBoxButton = new System.Windows.Forms.Button();
            this.devOuterBoxButton = new System.Windows.Forms.Button();
            this.printLabelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.boxDataGridView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.createTimeTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.finishTimeTtextBox = new System.Windows.Forms.TextBox();
            this.boxStatusTextBox = new System.Windows.Forms.TextBox();
            this.occupiedTextBox = new System.Windows.Forms.TextBox();
            this.capacityTextBox = new System.Windows.Forms.TextBox();
            this.realCountTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boxDataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1008, 729);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.outerBoxButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.innerBoxButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.devOuterBoxButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.printLabelButton, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1002, 34);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // outerBoxButton
            // 
            this.outerBoxButton.Location = new System.Drawing.Point(83, 3);
            this.outerBoxButton.Name = "outerBoxButton";
            this.outerBoxButton.Size = new System.Drawing.Size(74, 28);
            this.outerBoxButton.TabIndex = 2;
            this.outerBoxButton.Text = "外箱";
            this.outerBoxButton.UseVisualStyleBackColor = true;
            this.outerBoxButton.Click += new System.EventHandler(this.outerBoxButton_Click);
            // 
            // innerBoxButton
            // 
            this.innerBoxButton.Location = new System.Drawing.Point(3, 3);
            this.innerBoxButton.Name = "innerBoxButton";
            this.innerBoxButton.Size = new System.Drawing.Size(67, 28);
            this.innerBoxButton.TabIndex = 0;
            this.innerBoxButton.Text = "内箱";
            this.innerBoxButton.UseVisualStyleBackColor = true;
            this.innerBoxButton.Click += new System.EventHandler(this.InnerBoxButtonClick);
            // 
            // devOuterBoxButton
            // 
            this.devOuterBoxButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.devOuterBoxButton.Location = new System.Drawing.Point(163, 3);
            this.devOuterBoxButton.Name = "devOuterBoxButton";
            this.devOuterBoxButton.Size = new System.Drawing.Size(74, 28);
            this.devOuterBoxButton.TabIndex = 1;
            this.devOuterBoxButton.Text = "设备外箱";
            this.devOuterBoxButton.UseVisualStyleBackColor = true;
            // 
            // printLabelButton
            // 
            this.printLabelButton.Location = new System.Drawing.Point(243, 3);
            this.printLabelButton.Name = "printLabelButton";
            this.printLabelButton.Size = new System.Drawing.Size(67, 28);
            this.printLabelButton.TabIndex = 3;
            this.printLabelButton.Text = "打印标签";
            this.printLabelButton.UseVisualStyleBackColor = true;
            this.printLabelButton.Visible = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.93014F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.06986F));
            this.tableLayoutPanel3.Controls.Add(this.boxDataGridView, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 43);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1002, 683);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // boxDataGridView
            // 
            this.boxDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.boxDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxDataGridView.Location = new System.Drawing.Point(3, 3);
            this.boxDataGridView.Name = "boxDataGridView";
            this.boxDataGridView.RowTemplate.Height = 21;
            this.boxDataGridView.Size = new System.Drawing.Size(344, 677);
            this.boxDataGridView.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.realCountTextBox);
            this.panel1.Controls.Add(this.capacityTextBox);
            this.panel1.Controls.Add(this.occupiedTextBox);
            this.panel1.Controls.Add(this.boxStatusTextBox);
            this.panel1.Controls.Add(this.finishTimeTtextBox);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.createTimeTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(353, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(646, 677);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "申请时间";
            // 
            // createTimeTextBox
            // 
            this.createTimeTextBox.Location = new System.Drawing.Point(120, 13);
            this.createTimeTextBox.Name = "createTimeTextBox";
            this.createTimeTextBox.ReadOnly = true;
            this.createTimeTextBox.Size = new System.Drawing.Size(167, 19);
            this.createTimeTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "完成装箱时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "实际数量";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "容量";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "已占用(外箱)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "箱体状态";
            // 
            // finishTimeTtextBox
            // 
            this.finishTimeTtextBox.Location = new System.Drawing.Point(120, 49);
            this.finishTimeTtextBox.Name = "finishTimeTtextBox";
            this.finishTimeTtextBox.ReadOnly = true;
            this.finishTimeTtextBox.Size = new System.Drawing.Size(167, 19);
            this.finishTimeTtextBox.TabIndex = 8;
            // 
            // boxStatusTextBox
            // 
            this.boxStatusTextBox.Location = new System.Drawing.Point(120, 82);
            this.boxStatusTextBox.Name = "boxStatusTextBox";
            this.boxStatusTextBox.ReadOnly = true;
            this.boxStatusTextBox.Size = new System.Drawing.Size(167, 19);
            this.boxStatusTextBox.TabIndex = 9;
            // 
            // occupiedTextBox
            // 
            this.occupiedTextBox.Location = new System.Drawing.Point(120, 112);
            this.occupiedTextBox.Name = "occupiedTextBox";
            this.occupiedTextBox.ReadOnly = true;
            this.occupiedTextBox.Size = new System.Drawing.Size(167, 19);
            this.occupiedTextBox.TabIndex = 10;
            // 
            // capacityTextBox
            // 
            this.capacityTextBox.Location = new System.Drawing.Point(120, 142);
            this.capacityTextBox.Name = "capacityTextBox";
            this.capacityTextBox.ReadOnly = true;
            this.capacityTextBox.Size = new System.Drawing.Size(167, 19);
            this.capacityTextBox.TabIndex = 11;
            // 
            // realCountTextBox
            // 
            this.realCountTextBox.Location = new System.Drawing.Point(120, 171);
            this.realCountTextBox.Name = "realCountTextBox";
            this.realCountTextBox.ReadOnly = true;
            this.realCountTextBox.Size = new System.Drawing.Size(167, 19);
            this.realCountTextBox.TabIndex = 12;
            // 
            // BoxListFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BoxListFrm";
            this.Text = "装箱列表";
            this.Load += new System.EventHandler(this.BoxListFrmLoad);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.boxDataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}

        private System.Windows.Forms.Button printLabelButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView boxDataGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox createTimeTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox boxStatusTextBox;
        private System.Windows.Forms.TextBox finishTimeTtextBox;
        private System.Windows.Forms.TextBox realCountTextBox;
        private System.Windows.Forms.TextBox capacityTextBox;
        private System.Windows.Forms.TextBox occupiedTextBox;
    }
}
