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
            this.refreshButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.boxDataGridView = new System.Windows.Forms.DataGridView();
            this.deleteBoxButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.searchBoxButton = new System.Windows.Forms.Button();
            this.searchBoxTextBox = new System.Windows.Forms.TextBox();
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
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 231F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.outerBoxButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.innerBoxButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.devOuterBoxButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.printLabelButton, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.refreshButton, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.deleteBoxButton, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 5, 0);
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
            this.devOuterBoxButton.Visible = false;
            this.devOuterBoxButton.Click += new System.EventHandler(this.devOuterBoxButton_Click);
            // 
            // printLabelButton
            // 
            this.printLabelButton.Location = new System.Drawing.Point(323, 3);
            this.printLabelButton.Name = "printLabelButton";
            this.printLabelButton.Size = new System.Drawing.Size(67, 28);
            this.printLabelButton.TabIndex = 3;
            this.printLabelButton.Text = "打印标签";
            this.printLabelButton.UseVisualStyleBackColor = true;
            this.printLabelButton.Visible = false;
            this.printLabelButton.Click += new System.EventHandler(this.printLabelButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(243, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(74, 28);
            this.refreshButton.TabIndex = 4;
            this.refreshButton.Text = "刷新";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click_1);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.93014F));
            this.tableLayoutPanel3.Controls.Add(this.boxDataGridView, 0, 0);
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
            this.boxDataGridView.Size = new System.Drawing.Size(996, 677);
            this.boxDataGridView.TabIndex = 0;
            this.boxDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BoxDataGridViewCellMouseDoubleClick);
            // 
            // deleteBoxButton
            // 
            this.deleteBoxButton.Location = new System.Drawing.Point(643, 3);
            this.deleteBoxButton.Name = "deleteBoxButton";
            this.deleteBoxButton.Size = new System.Drawing.Size(74, 28);
            this.deleteBoxButton.TabIndex = 5;
            this.deleteBoxButton.Text = "删除箱子";
            this.deleteBoxButton.UseVisualStyleBackColor = true;
            this.deleteBoxButton.Click += new System.EventHandler(this.deleteBoxButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.searchBoxTextBox);
            this.panel1.Controls.Add(this.searchBoxButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(403, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 28);
            this.panel1.TabIndex = 8;
            // 
            // searchBoxButton
            // 
            this.searchBoxButton.Location = new System.Drawing.Point(156, 0);
            this.searchBoxButton.Name = "searchBoxButton";
            this.searchBoxButton.Size = new System.Drawing.Size(66, 28);
            this.searchBoxButton.TabIndex = 10;
            this.searchBoxButton.Text = "检索箱子";
            this.searchBoxButton.UseVisualStyleBackColor = true;
            this.searchBoxButton.Click += new System.EventHandler(this.searchBoxButton_Click);
            // 
            // searchBoxTextBox
            // 
            this.searchBoxTextBox.Location = new System.Drawing.Point(3, 5);
            this.searchBoxTextBox.Name = "searchBoxTextBox";
            this.searchBoxTextBox.Size = new System.Drawing.Size(153, 19);
            this.searchBoxTextBox.TabIndex = 11;
            // 
            // BoxListFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "BoxListFrm";
            this.Text = "装箱列表";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.BoxListFrm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BoxListFrm_FormClosing);
            this.Load += new System.EventHandler(this.BoxListFrmLoad);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BoxListFrm_KeyDown);
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
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button deleteBoxButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox searchBoxTextBox;
        private System.Windows.Forms.Button searchBoxButton;
    }
}
