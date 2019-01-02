﻿using System;
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
using System.Data.SQLite;
using RestSharp;
using PackingTracker.Common;
using PackingTracker.Entity;
using com.newtronics.Entity;
using com.newtronics.UI;

namespace PackingTracker.UI
{
	/// <summary>
	/// Description of PackingFrm.
	/// </summary>
	public partial class PackingFrm : Form
	{
		private RestClient client;
		
		private readonly SynchronizationContext synchronizationContext;

        public OrderDetail OrderDetail
        {
            get;
            set;
        }

        public Box Box
		{
			get;
			set;
		}

        public PackingFrm(): this(null, null)
        {
        }

        public PackingFrm(BoxDetail boxDetail, OrderDetail od)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

            #region Initialize
            //箱子详细
            this.Box = new Box();
            this.Box.retdata.BoxInfo = boxDetail;
            this.OrderDetail = od;

            synchronizationContext = SynchronizationContext.Current;
			
            string host = Constants.Host;
            client = new RestClient(host);
            //5 seconds
            client.ReadWriteTimeout = 5000;
            
            //箱内设备
            deviceDataGridView.AutoGenerateColumns = false;
            deviceDataGridView.VirtualMode = true;
            deviceDataGridView.CellValueNeeded += DeviceDataGridView_CellValueNeeded;
            deviceDataGridView.ReadOnly = true;
            deviceDataGridView.AllowUserToAddRows = false;
            deviceDataGridView.RowCount = 0;
            
            #endregion
        }

        void PackingFrmLoad(object sender, EventArgs e)
        {
            UpdateView();
        }

        void UpdateDevView()
        {
            deviceDataGridView.RowCount = (this.Box.retdata.DevInfo == null ? 0 : this.Box.retdata.DevInfo.Count+9)/10;
            deviceDataGridView.Invalidate();
            for (int i = 1; i <= deviceDataGridView.Rows.Count; i++)
            {
                deviceDataGridView.Rows[i - 1].HeaderCell.Value = i.ToString();
            }
            deviceDataGridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        private void DeviceDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = "";
            int index = e.RowIndex * 10 + e.ColumnIndex;
            if(index > -1 && index < this.Box.retdata.DevInfo.Count)
            {
                DevInfo devInfo = this.Box.retdata.DevInfo[index];
                e.Value = devInfo.Did;
            }
        }

        void UpdateView()
        {
            //箱子类型
            boxTypeTextBox.Text = this.Box.retdata.BoxInfo.BoxType;
            //订单ID
            orderIdTextBox.Text = this.Box.retdata.BoxInfo.OrderId;
            //箱号
            boxSNTextBox.Text = this.Box.retdata.BoxInfo.BoxSN;
            //设备输入框
            deviceInputTextBox.Enabled = false;

            if (this.Box.retdata.BoxInfo.Status == "complete")
            {
                packingCompButon.Enabled = false;
            }
            else
            {
                packingCompButon.Enabled = true;
            }

            boxSizeTextBox.Text = String.Format("{0}", this.Box.retdata.BoxInfo.Capacity);
            realCountTextBox.Text = String.Format("{0}", this.Box.retdata.BoxInfo.RealCount);

            //若无箱号，新建箱子
            if (String.IsNullOrEmpty(this.Box.retdata.BoxInfo.BoxSN))
            {
                boxCapacityTextBox.Text = String.Format("{0}", this.Box.retdata.BoxInfo.Capacity);
                applyBoxButton.Enabled = true;
                boxCapacityTextBox.ReadOnly = false;
            }
            else
            {
                //有箱号，查询箱内设备
                boxCapacityTextBox.Text = String.Format("{0}", this.Box.retdata.BoxInfo.Capacity);
                boxCapacityTextBox.ReadOnly = true;
                applyBoxButton.Enabled = false;
                deviceInputTextBox.Enabled = true;

                //查询箱内设备
                QueryBoxDev();
            }
        }

        void QueryBoxDev()
        {
            var request = new RestRequest("dlicense/v2/manu/boxing/listinnerdev", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"boxsn\":");
            sb.Append("\"").Append(this.Box.retdata.BoxInfo.BoxSN).Append("\",");

            sb.Append("\"index\":");
            sb.Append("0,");

            sb.Append("\"pagesize\":");
            sb.Append("0");

            sb.Append("}");

            string body = sb.ToString();
            Console.WriteLine(body);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.ReadWriteTimeout = 5000;

            var asyncHandle = client.ExecuteAsync<Box>(request, res =>
            {
                if (res.IsSuccessful)
                {
                    Box box = res.Data;
                    if (box != null && box.retdata != null)
                    {
                        if (box.Status != 0)
                        {
                            ShowError(String.Format("Error:{0}, {1}", box.Status, box.Msg));
                        }
                        else
                        {
                            synchronizationContext.Post(new SendOrPostCallback(o =>
                            {
                                RetData b = o as RetData;
                                if (b != null)
                                {
                                    this.Box.retdata.Count = b.Count;
                                    this.Box.retdata.DevInfo = b.DevInfo;
                                    UpdateDevView();
                                }
                                else
                                {
                                    ShowError("无法查询箱内设备");
                                }
                            }), box.retdata);
                        }
                    }
                    else
                    {
                        ShowError("无法转换为JSON");
                    }
                }
                else
                {
                    ShowError("未知错误");
                }
            });

            //IRestResponse<Box> res = client.Execute<Box>(request);

            
        }


		
		void PackingCompButonClick(object sender, EventArgs e)
		{
	
		}
		
		/// <summary>
		/// 申请箱号
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ApplyBoxButtonClick(object sender, EventArgs e)
		{
			string capacity = boxCapacityTextBox.Text;
			if(String.IsNullOrEmpty(capacity))
			{
				MessageBox.Show("请输入箱子大小", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if(!String.IsNullOrEmpty(this.Box.retdata.BoxInfo.BoxSN))
			{
				MessageBox.Show("箱号已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			var request = new RestRequest("dlicense/v2/manu/boxing/applybox", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);
            
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"orderid\":");
            sb.Append("\"").Append(this.Box.retdata.BoxInfo.OrderId).Append("\",");

            sb.Append("\"capacity\":");
            sb.Append(capacity).Append(",");

            sb.Append("\"boxtype\":");
            //"boxtype" = "inner"|"outerfordev"|"outerforbox" 分别代表内箱，设备外箱和外箱
            sb.Append("\"").Append(this.Box.retdata.BoxInfo.BoxType).Append("\"");
            sb.Append("}");
            
            string body = sb.ToString();
            Console.WriteLine(body);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            
            var asyncHandle = client.ExecuteAsync<Box>(request, response => {

	            if(response.IsSuccessful)
	            {
	                Box box = response.Data;
	                if(box != null && box.retdata != null && box.retdata.BoxInfo != null)
	                {
	                	if(box.Status != 0)
	                	{
	                		ShowError(String.Format("Error:{0}, {1}", box.Status, box.Msg));
	                	}
	                	else
	                	{
	                		synchronizationContext.Post(new SendOrPostCallback(o =>
		                    {
		                	    BoxDetail boxDetail = o as BoxDetail;
                                if(boxDetail == null)
                                {
                                    ShowError("无法申请箱号");
                                }
                                else
                                {
                                    this.Box.retdata.BoxInfo = boxDetail;
                                    UpdateView();
                                }
                               
		                    }), box.retdata.BoxInfo);
	                	}
	                }
	                else
	                {
	                	ShowError("无法转换为JSON");
	                }
	            }
	            else
                {
                	ShowError("未知错误");
                }
	        });
		}
		
		/// <summary>
		/// Only number
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void BoxCapacityTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			
		}
		void BoxCapacityTextBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		    {
		        e.Handled = true;
		    }
		}
		
		/// <summary>
		/// 设备入箱
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void DeviceInputTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				AddDeviceToLocalBox(deviceInputTextBox.Text);
                this.deviceInputTextBox.SelectAll();
            }
		}
		
		void AddDeviceToLocalBox(string did)
		{
			if(String.IsNullOrEmpty(did))
			{
				MessageBox.Show("设备为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

            AddDeviceToBox(did);
        }
		
		void AddDeviceToBox(string did)
		{
            var request = new RestRequest("dlicense/v2/manu/boxing/adddevbinding", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"boxsn\":");
            sb.Append("\"").Append(this.Box.retdata.BoxInfo.BoxSN).Append("\",");
            sb.Append("\"orderid\":");
            sb.Append("\"").Append(this.Box.retdata.BoxInfo.OrderId).Append("\",");

            sb.Append("\"dids\":");
            sb.Append("[");
            sb.Append("\"").Append(did).Append("\"");
            sb.Append("]");
            sb.Append("}");
            
            string body = sb.ToString();
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.ReadWriteTimeout = 5000;
            IRestResponse<AddDevBinding> res = client.Execute<AddDevBinding>(request);
            if (res.IsSuccessful)
            {
                AddDevBinding b = res.Data;
                if (b == null || b.Status != 0)
                {
                    MessageBeep(0xFFFFFFFF);
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        string msg = o as string;
                        if(String.IsNullOrEmpty(msg))
                        {
                            msg = "无法添加设备";
                        }
                        msgLabel.Text = msg;

                    }), b.Msg);
                }
                else
                {
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        msgLabel.Text = "成功添加设备";

                    }), null);
                }
            }
            else
            {
                MessageBeep(0xFFFFFFFF);
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    msgLabel.Text = "无法添加设备";

                }), null);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int MessageBeep(uint n);

        private void ShowError(string errMsg)
		{
			synchronizationContext.Post(new SendOrPostCallback(o =>
            {
				string msg = o as string;
                if (msg == null)
                {
                    msg = "未知错误，请重试";
                }

                MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }), errMsg);
		}

        private void printLabelButon_Click(object sender, EventArgs e)
        {
            PrintFrm printFrm = new PrintFrm(this.Box.retdata.BoxInfo.BoxSN, OrderDetail);
            printFrm.ShowDialog(this);
        }
    }
}
