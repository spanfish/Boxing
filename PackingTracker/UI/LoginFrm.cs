using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

using RestSharp;

using PackingTracker.Common;
using PackingTracker.Entity;

namespace PackingTracker.UI
{
	/// <summary>
	/// Description of LoginFrm.
	/// </summary>
	public partial class LoginFrm : Form
	{
		private RestClient client;
		
		private readonly SynchronizationContext synchronizationContext;
		
		private List<AccountDetail> accounts;
		
		public LoginFrm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			#region Initialize
			synchronizationContext = SynchronizationContext.Current;
			
			loginIDTextBox.Text = "15372098619";
            passwordTextBox.Text = "yunduan412";
            
            string host = Constants.Host;
            client = new RestClient(host);
			#endregion
			
		}
		void LoginFrmLoad(object sender, EventArgs e)
		{
			
		}
		
		void LoginButtonClick(object sender, EventArgs e)
		{
			#region Login
			if(loginIDTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("输入登陆ID", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (passwordTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("输入密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
            	Handshake(loginIDTextBox.Text.Trim(), passwordTextBox.Text.Trim());
            }
			#endregion
		}
		
		void Handshake(string id, string pwd)
		{
			var request = new RestRequest("dlicense/v2/account/handshake", Method.GET);
			request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
			var asyncHandle = client.ExecuteAsync<Handshake>(request, response =>
            {
                if(!response.IsSuccessful)
                {
                    ShowError(response.ErrorMessage);
                    return;
                }
                Handshake handshake = response.Data;

                if (handshake != null && handshake.Status == 0)
                {
                    Login(id, pwd, handshake);
                }
                else
                {
                    ShowError(handshake == null ? null : handshake.Msg);
                }
            });
		}
		
		void Login(string Id, string pwd, Handshake res)
		{
			var request = new RestRequest("dlicense/v2/account/login", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("timestamp", res.Timestamp);
            
            byte[] data = System.Text.Encoding.ASCII.GetBytes(pwd + "4969fj#k23#");
            byte[] encryptedPwdByte;
            SHA1 sha = new SHA1CryptoServiceProvider();
            encryptedPwdByte = sha.ComputeHash(data);
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (byte curByte in encryptedPwdByte)
            {
                sb.Append(curByte.ToString("x2"));
            }
            string encryptedPwd = sb.ToString();
            sb = new System.Text.StringBuilder();
            sb.Append("{\"phone\":\"");
            sb.Append(Id);
            sb.Append("\",");
            sb.Append("\"password\":\"");
            sb.Append(encryptedPwd);
            sb.Append("\"");
            sb.Append("}");
            
            string body = sb.ToString();

            string token = body + "xgx3d*fe3478$ukx";

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] srcBytes = System.Text.Encoding.UTF8.GetBytes(token);
            byte[] destBytes = md5.ComputeHash(srcBytes);
            sb = new System.Text.StringBuilder();
            foreach (byte curByte in destBytes)
            {
                sb.Append(curByte.ToString("x2"));
            }

            request.AddHeader("token", sb.ToString());

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var asyncHandle = client.ExecuteAsync<Login>(request, response => {
                if (!response.IsSuccessful)
                {
                    ShowError(response.ErrorMessage);
                    return;
                }

                Login loginRes = response.Data;

                if (loginRes != null && loginRes.Status == 0)
                {                    
                    //AppConfig.Login = loginRes;
                    SharedApp.Instance.Login = loginRes;
                    QueryAccount();
                }
                else
                {
                    ShowError(loginRes == null ? null : loginRes.Msg);
                }
            });
		}
		
		void QueryAccount()
		{
			var request = new RestRequest("dlicense/v2/account/role/query", Method.GET);

            request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);

            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var asyncHandle = client.ExecuteAsync<Account>(request, response => {
                if (!response.IsSuccessful)
                {
                    ShowError(response.ErrorMessage);
                    return;
                }

                Account accountRes = response.Data;
                if (accountRes != null && accountRes.Data != null)
                {
                    if (accountRes.Data.Count == 1)
                    {
                        SharedApp.Instance.AccountDetail = accountRes.Data[0];
	
                        synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
//                            ShowError(SharedApp.Instance.AccountDetail.OemfactoryName);
                            this.DialogResult = DialogResult.OK;
                        	this.Close();
                        }), null);
                    }
                    else
                    {
                        synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
                            this.accounts = o as List<AccountDetail>;
                            var source = new BindingSource(accounts, null);
                            this.accountsComboBox.DataSource = source;
                            this.accountsComboBox.ValueMember = "OemfactoryId";
                            this.accountsComboBox.DisplayMember = "OemfactoryName";
                            this.accountsComboBox.Visible = true;
                            factoryLabel.Visible = true;
                            enterButton.Visible = true;
                            
                           
							
                        }), accountRes.Data);

                    }
                }
                else
                {
                    ShowError(accountRes == null ? null : accountRes.Msg);
                }
            });
		}
		
		private void ShowError(string errMsg)
		{
			synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                enterButton.Enabled = true;

                string msg = o as string;
                if (msg == null)
                {
                    msg = "未知错误，请重试";
                }

                MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }), errMsg);
		}
		
		void EnterButtonClick(object sender, EventArgs e)
		{
			if(this.accounts != null && this.accounts.Count > 0)
            {
                foreach(AccountDetail a in this.accounts)
                {
                    if(a.OemfactoryId == (this.accountsComboBox.SelectedValue as string))
                    {
                        SharedApp.Instance.AccountDetail = a;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;
                    }
                }
            }
		}
	}
}
