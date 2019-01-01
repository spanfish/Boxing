/*
 * Created by SharpDevelop.
 * User: xwang
 * Date: 2018/12/27
 * Time: 11:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace PackingTracker.UI
{
	partial class LoginFrm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TextBox loginIDTextBox;
		private System.Windows.Forms.TextBox passwordTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button loginButton;
		private System.Windows.Forms.ComboBox accountsComboBox;
		private System.Windows.Forms.Label factoryLabel;
		private System.Windows.Forms.Button enterButton;
		
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
			this.loginIDTextBox = new System.Windows.Forms.TextBox();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.loginButton = new System.Windows.Forms.Button();
			this.accountsComboBox = new System.Windows.Forms.ComboBox();
			this.factoryLabel = new System.Windows.Forms.Label();
			this.enterButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// loginIDTextBox
			// 
			this.loginIDTextBox.Location = new System.Drawing.Point(95, 38);
			this.loginIDTextBox.Name = "loginIDTextBox";
			this.loginIDTextBox.Size = new System.Drawing.Size(195, 19);
			this.loginIDTextBox.TabIndex = 0;
			// 
			// passwordTextBox
			// 
			this.passwordTextBox.Location = new System.Drawing.Point(95, 75);
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.Size = new System.Drawing.Size(195, 19);
			this.passwordTextBox.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(33, 41);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "用户名";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(33, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "密码";
			// 
			// loginButton
			// 
			this.loginButton.Location = new System.Drawing.Point(95, 116);
			this.loginButton.Name = "loginButton";
			this.loginButton.Size = new System.Drawing.Size(195, 23);
			this.loginButton.TabIndex = 4;
			this.loginButton.Text = "登陆";
			this.loginButton.UseVisualStyleBackColor = true;
			this.loginButton.Click += new System.EventHandler(this.LoginButtonClick);
			// 
			// accountsComboBox
			// 
			this.accountsComboBox.FormattingEnabled = true;
			this.accountsComboBox.Location = new System.Drawing.Point(95, 167);
			this.accountsComboBox.Name = "accountsComboBox";
			this.accountsComboBox.Size = new System.Drawing.Size(195, 20);
			this.accountsComboBox.TabIndex = 5;
			this.accountsComboBox.Visible = false;
			// 
			// factoryLabel
			// 
			this.factoryLabel.Location = new System.Drawing.Point(33, 170);
			this.factoryLabel.Name = "factoryLabel";
			this.factoryLabel.Size = new System.Drawing.Size(56, 23);
			this.factoryLabel.TabIndex = 6;
			this.factoryLabel.Text = "工厂";
			this.factoryLabel.Visible = false;
			// 
			// enterButton
			// 
			this.enterButton.Location = new System.Drawing.Point(95, 205);
			this.enterButton.Name = "enterButton";
			this.enterButton.Size = new System.Drawing.Size(195, 23);
			this.enterButton.TabIndex = 7;
			this.enterButton.Text = "进入";
			this.enterButton.UseVisualStyleBackColor = true;
			this.enterButton.Visible = false;
			this.enterButton.Click += new System.EventHandler(this.EnterButtonClick);
			// 
			// LoginFrm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(358, 244);
			this.Controls.Add(this.enterButton);
			this.Controls.Add(this.factoryLabel);
			this.Controls.Add(this.accountsComboBox);
			this.Controls.Add(this.loginButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.passwordTextBox);
			this.Controls.Add(this.loginIDTextBox);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoginFrm";
			this.Text = "登陆";
			this.Load += new System.EventHandler(this.LoginFrmLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
