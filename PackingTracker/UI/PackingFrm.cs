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

        private int realCount;

        public PackingFrm() : this(null, null)
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
            //订单信息
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

        /// <summary>
        /// 更新窗口显示
        /// </summary>
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

            if (this.Box.retdata.BoxInfo.Status == "complete" || String.IsNullOrEmpty(this.Box.retdata.BoxInfo.BoxSN))
            {
                packingCompButon.Enabled = false;
            }
            else
            {
                packingCompButon.Enabled = true;
            }

            if (this.Box.retdata.BoxInfo.Status != "complete")
            {
                realCountTextBox.BackColor = Color.Red;
            }
            else
            {
                realCountTextBox.BackColor = Color.Blue;
            }

            boxSizeTextBox.Text = String.Format("{0}", this.Box.retdata.BoxInfo.Capacity);
            realCountTextBox.Text = String.Format("{0}", this.Box.retdata.BoxInfo.RealCount);
            realCount = this.Box.retdata.BoxInfo.RealCount;
            if (this.Box.retdata.BoxInfo.BoxType == Constants.BoxTypeInner)
            {
                prompLabel.Text = "请输入DID/MAC";
            }
            else
            {
                prompLabel.Text = "请输入内箱号";
            }

            //若无箱号，新建箱子
            if (String.IsNullOrEmpty(this.Box.retdata.BoxInfo.BoxSN))
            {
                boxCapacityTextBox.Text = String.Format("{0}", this.Box.retdata.BoxInfo.Capacity);
                applyBoxButton.Enabled = true;
                boxCapacityTextBox.ReadOnly = false;
                unboundCheckbox.Enabled = false;
            }
            else
            {
                unboundCheckbox.Enabled = true;
                //有箱号，查询箱内设备
                boxCapacityTextBox.Text = String.Format("{0}", this.Box.retdata.BoxInfo.Capacity);
                boxCapacityTextBox.ReadOnly = true;
                applyBoxButton.Enabled = false;
                deviceInputTextBox.Enabled = true;

                //查询箱内设备
                if (this.Box.retdata.BoxInfo.BoxType == Constants.BoxTypeInner)
                {
                    //查询内箱内设备
                    QueryDevInInnerBox();
                }
                else
                {
                    //查询外箱内内箱
                    QueryInnerBoxInOuterBox();
                }
            }
        }

        /// <summary>
        /// 查询内箱内设备
        /// </summary>
        void QueryDevInInnerBox()
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
                            ShowError(box.Status);
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
                                    UpdateInnerBoxView();
                                }
                                else
                                {
                                    ShowError(-1);
                                }
                            }), box.retdata);
                        }
                    }
                    else
                    {
                        ShowError(-1);
                    }
                }
                else
                {
                    ShowError(-1);
                }
            });
        }

        /// <summary>
        /// 显示内箱内设备
        /// </summary>
        void UpdateInnerBoxView()
        {
            deviceDataGridView.VirtualMode = true;
            deviceDataGridView.AutoGenerateColumns = false;
            deviceDataGridView.Columns.Clear();
            for (int i = 1; i < 11; i++)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.Name = string.Format("{0}", i);
                column.HeaderText = string.Format("{0}", i);
                deviceDataGridView.Columns.Add(column);
            }
            deviceDataGridView.RowCount = (this.Box.retdata.DevInfo == null ? 0 : this.Box.retdata.DevInfo.Count + 9) / 10;
            deviceDataGridView.Invalidate();
            for (int i = 1; i <= deviceDataGridView.Rows.Count; i++)
            {
                deviceDataGridView.Rows[i - 1].HeaderCell.Value = i.ToString();
            }
            deviceDataGridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            deviceDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        /// <summary>
        /// 显示外箱内内箱`
        /// </summary>
        void UpdateOutterBoxView()
        {
            deviceDataGridView.VirtualMode = false;
            deviceDataGridView.AutoGenerateColumns = false;
            deviceDataGridView.Columns.Clear();
            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.Name = "BoxType";
            column1.HeaderText = "箱体类型";
            column1.DataPropertyName = "BoxType";
            deviceDataGridView.Columns.Add(column1);

            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.Name = "BoxSN";
            column2.DataPropertyName = "BoxSN";
            column2.HeaderText = "箱号";
            deviceDataGridView.Columns.Add(column2);

            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.Name = "Capacity";
            column3.DataPropertyName = "Capacity";
            column3.HeaderText = "箱体容量";
            deviceDataGridView.Columns.Add(column3);

            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn();
            column4.Name = "RealCount";
            column4.DataPropertyName = "RealCount";
            column4.HeaderText = "实际数量";
            deviceDataGridView.Columns.Add(column4);

            DataGridViewTextBoxColumn column5 = new DataGridViewTextBoxColumn();
            column5.Name = "CreateTime";
            column5.DataPropertyName = "CreateTime";
            column5.HeaderText = "创建时间";
            deviceDataGridView.Columns.Add(column5);

            deviceDataGridView.DataSource = new BindingSource(this.Box.retdata.InnerBoxes, null);
            for (int i = 1; i <= deviceDataGridView.Rows.Count; i++)
            {
                deviceDataGridView.Rows[i - 1].HeaderCell.Value = i.ToString();
            }
            deviceDataGridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            deviceDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void DeviceDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = "";
            int index = e.RowIndex * 10 + e.ColumnIndex;
            if (index > -1 && index < this.Box.retdata.DevInfo.Count)
            {
                DevInfo devInfo = this.Box.retdata.DevInfo[index];
                e.Value = devInfo.Did;
            }
        }

        /// <summary>
        /// 查询外箱里的内箱
        /// </summary>
        void QueryInnerBoxInOuterBox()
        {
            var request = new RestRequest("dlicense/v2/manu/boxing/listinnerbox", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"boxsn\":");
            sb.Append("\"").Append(this.Box.retdata.BoxInfo.BoxSN).Append("\"");
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
                            ShowError(box.Status);
                        }
                        else
                        {
                            List<string> innerBoxes = box.retdata.BoxSN;
                            this.Box.retdata.InnerBoxes = new List<BoxDetail>();

                            if (innerBoxes != null)
                            {
                                foreach (string sn in innerBoxes)
                                {
                                    BoxDetail d = new BoxDetail();
                                    d.BoxSN = sn;
                                    this.Box.retdata.InnerBoxes.Add(d);
                                }
                            }
                            //根据箱号查询外箱内的内箱信息
                            QueryBoxInfo();
                            synchronizationContext.Post(new SendOrPostCallback(o =>
                            {
                                //更新窗口显示
                                UpdateOutterBoxView();
                            }), null);
                        }
                    }
                    else
                    {
                        ShowError(-1);
                    }
                }
                else
                {
                    ShowError(-1);
                }
            });
        }

        /// <summary>
        /// 根据箱号查询外箱内的内箱信息
        /// </summary>
        void QueryBoxInfo()
        {
            foreach (BoxDetail d in this.Box.retdata.InnerBoxes)
            {
                if (String.IsNullOrEmpty(d.CreateTime))
                {
                    BoxDetail bd = QueryBoxInfo(d.BoxSN);
                    if(bd != null)
                    {
                        d.BoxSN = bd.BoxSN;
                        d.BoxType = bd.BoxType;
                        d.Capacity = bd.Capacity;
                        d.CreateTime = bd.CreateTime;
                        d.FinishTime = bd.FinishTime;
                        d.Occupied = bd.Occupied;
                        d.Oemfactoryid = bd.Oemfactoryid;
                        d.OrderId = bd.OrderId;
                        d.RealCount = bd.RealCount;
                        d.Requserid = bd.Requserid;
                        d.Status = bd.Status;
                    }                    
                }
            }
        }

        void UpdateParentBox()
        {
            BoxDetail bd = QueryBoxInfo(this.Box.retdata.BoxInfo.BoxSN);
            if(bd != null)
            {
                this.Box.retdata.BoxInfo.BoxSN = bd.BoxSN;
                this.Box.retdata.BoxInfo.BoxType = bd.BoxType;
                this.Box.retdata.BoxInfo.Capacity = bd.Capacity;
                this.Box.retdata.BoxInfo.CreateTime = bd.CreateTime;
                this.Box.retdata.BoxInfo.FinishTime = bd.FinishTime;
                this.Box.retdata.BoxInfo.Occupied = bd.Occupied;
                this.Box.retdata.BoxInfo.Oemfactoryid = bd.Oemfactoryid;
                this.Box.retdata.BoxInfo.OrderId = bd.OrderId;
                this.Box.retdata.BoxInfo.RealCount = bd.RealCount;
                this.Box.retdata.BoxInfo.Requserid = bd.Requserid;
                this.Box.retdata.BoxInfo.Status = bd.Status;
            }
            realCountTextBox.Text = String.Format("{0}", this.Box.retdata.BoxInfo.RealCount);
            if(this.Box.retdata.BoxInfo.Status != "complete")
            {
                realCountTextBox.BackColor = Color.Red;
            }
            else
            {
                realCountTextBox.BackColor = Color.Blue;
            }
            
            packingCompButon.Enabled = this.Box.retdata.BoxInfo.Status != "complete";
            if(this.Box.retdata.BoxInfo.RealCount == this.Box.retdata.BoxInfo.Capacity && this.Box.retdata.BoxInfo.Status != "complete")
            {
                if(MessageBox.Show("箱子已满，是否完成装箱？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PackingCompButonClick(null, null);
                }
            }
        }

        BoxDetail QueryBoxInfo(string boxSN)
        {
            var request = new RestRequest("dlicense/v2/manu/boxing/queryboxinfo", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"boxsn\":");
            sb.Append("\"").Append(boxSN).Append("\"");
            sb.Append("}");

            string body = sb.ToString();
            Console.WriteLine(body);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.ReadWriteTimeout = 5000;
            IRestResponse<BoxQuery> res = client.Execute<BoxQuery>(request);
            if (res.IsSuccessful)
            {
                BoxQuery bq = res.Data;
                if (bq != null && bq.Status == 0)
                {
                    return bq.retdata.BoxInfo;
                }
            }

            return null;
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
	                		ShowError(box.Status);
	                	}
	                	else
	                	{
	                		synchronizationContext.Post(new SendOrPostCallback(o =>
		                    {
		                	    BoxDetail boxDetail = o as BoxDetail;
                                if(boxDetail == null)
                                {
                                    ShowError(-1);
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
	                	ShowError(-1);
	                }
	            }
	            else
                {
                	ShowError(-1);
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
                string did = deviceInputTextBox.Text;
                if(did != null)
                {
                    if(did.StartsWith("NC"))
                    {
                        int index = did.IndexOf("-");
                        if(index >= 0)
                        {
                            did = did.Substring(index + 1);
                        }
                    }
                }
                if (unboundCheckbox.Checked)
                {
                    RemoveDevOrBox(did);
                }
                else
                {
                    if (this.Box.retdata.BoxInfo.BoxType == Constants.BoxTypeInner)
                    {
                        AddDevToBox(did);
                    }
                    else
                    {
                        AddInnerToOutterBox(did);
                    }
                }

                UpdateParentBox();

                this.deviceInputTextBox.SelectAll();
            }
		}
		
        void RemoveDevOrBox(string did)
        {
            RestRequest request;
            if (this.Box.retdata.BoxInfo.BoxType == Constants.BoxTypeInner)
            {
                request = new RestRequest("dlicense/v2/manu/boxing/deldevbinding", Method.POST);
            }
            else
            {
                request = new RestRequest("dlicense/v2/manu/boxing/delboxbinding", Method.POST);
            }

            request.RequestFormat = DataFormat.Json;
            request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            if (this.Box.retdata.BoxInfo.BoxType == Constants.BoxTypeInner)
            {
                sb.Append("\"orderid\":");
                sb.Append("\"").Append(this.Box.retdata.BoxInfo.OrderId).Append("\",");
                sb.Append("\"dids\":");                
            }
            else
            {
                sb.Append("\"boxsns\":");
            }
            sb.Append("[");
            sb.Append("\"").Append(did).Append("\"");
            sb.Append("]");
            sb.Append("}");


            string body = sb.ToString();
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.ReadWriteTimeout = 5000;
            IRestResponse<Res> response = client.Execute<Res>(request);
            if (response.IsSuccessful)
            {
                Res b = response.Data;
                if (b == null || b.Status != 0)
                {
                    MessageBeep(0xFFFFFFFF);
                    string errMsg = null;
                    if (b != null)
                    {
                        errMsg = Constants.GetError(b.Status);
                    }

                    if (String.IsNullOrEmpty(errMsg))
                    {
                        errMsg = "无法解绑";
                    }
                    msgLabel.Text = errMsg;
                    MessageBox.Show(errMsg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.deviceInputTextBox.Clear();
                    msgLabel.Text = "解绑成功";

                    if (this.Box.retdata.BoxInfo.BoxType == Constants.BoxTypeInner)
                    {
                        QueryDevInInnerBox();
                    }
                    else
                    {
                        for(int i = 0; i < this.Box.retdata.InnerBoxes.Count; i++)
                        {
                            if(String.Equals(did, this.Box.retdata.InnerBoxes[i].BoxSN))
                            {
                                this.Box.retdata.InnerBoxes.RemoveAt(i);
                                break;
                            }
                        }

                        deviceDataGridView.DataSource = new BindingSource(this.Box.retdata.InnerBoxes, null);
                        //更新窗口显示
                        UpdateOutterBoxView();
                    }                    
                }
            }
            else
            {
                MessageBeep(0xFFFFFFFF);
                msgLabel.Text = "无法解绑";
                MessageBox.Show("无法解绑", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

		void AddDevToBox(string did)
		{
			if(String.IsNullOrEmpty(did))
			{
				MessageBox.Show("设备为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

            AddDeviceToBox(did);
        }

        /// <summary>
        /// 外箱添加内箱
        /// </summary>
        /// <param name="did"></param>
        void AddInnerToOutterBox(string did)
        {
            var request = new RestRequest("dlicense/v2/manu/boxing/addboxbinding", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"pboxsn\":");
            sb.Append("\"").Append(this.Box.retdata.BoxInfo.BoxSN).Append("\",");

            sb.Append("\"boxsns\":");
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
                    string errMsg = null;
                    if (b != null)
                    {
                        errMsg = Constants.GetError(b.Status);
                    }
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        string msg = o as string;
                        if (String.IsNullOrEmpty(msg))
                        {
                            msg = "无法添加内箱";
                        }
                        msgLabel.Text = msg;
                        MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }), errMsg);
                }
                else
                {
                    BoxDetail d = new BoxDetail();
                    d.BoxSN = did;
                    this.Box.retdata.InnerBoxes.Add(d);
                    QueryBoxInfo();
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        this.deviceInputTextBox.Clear();
                        msgLabel.Text = "成功添加内箱";
                        realCount++;
                        if (realCount == this.Box.retdata.BoxInfo.Capacity)
                        {
                            printLabelButon_Click(null, null);
                        }
                        //更新窗口显示
                        UpdateOutterBoxView();
                    }), null);
                }
            }
            else
            {
                MessageBeep(0xFFFFFFFF);
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    msgLabel.Text = "无法添加设备";
                    MessageBox.Show("无法添加设备", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }), null);

            }
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
                    string errMsg = null;
                    if(b!=null)
                    {
                        errMsg = Constants.GetError(b.Status);
                    }
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        string msg = o as string;
                        if(String.IsNullOrEmpty(msg))
                        {
                            msg = "无法添加设备";
                        }
                        msgLabel.Text = msg;
                        MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }), errMsg);
                }
                else
                {
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        this.deviceInputTextBox.Clear();
                        msgLabel.Text = "成功添加设备";

                        QueryDevInInnerBox();
                        realCount++;
                        if(this.Box.retdata.Count == this.Box.retdata.BoxInfo.Capacity)
                        {
                            printLabelButon_Click(null, null);
                        }
                    }), null);
                }
            }
            else
            {
                MessageBeep(0xFFFFFFFF);
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    msgLabel.Text = "无法添加设备";
                    MessageBox.Show("无法添加设备", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }), null);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int MessageBeep(uint n);

        private void ShowError(int errCode)
		{
            string errMsg = Constants.GetError(errCode);
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
            if(this.Box.retdata.BoxInfo.Status != "complete")
            {
                if (MessageBox.Show("装箱未完成，确认要打印标签吗", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }
            PrintFrm printFrm = new PrintFrm(this.Box.retdata.BoxInfo.BoxSN, OrderDetail);
            printFrm.ShowDialog(this);
        }

        /// <summary>
        /// 完成装箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PackingCompButonClick(object sender, EventArgs e)
        {
            bool force = false;
            if(this.Box.retdata.BoxInfo.RealCount < this.Box.retdata.BoxInfo.Capacity)
            {
                if(MessageBox.Show("箱子未装满，强制完成？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    force = true;
                }
                else
                {
                    return;
                }
            }
            var request = new RestRequest(String.Format("dlicense/v2/manu/boxing/completeboxing{0}", force ? "?action=force" : ""), Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"boxsn\":");
            sb.Append("\"").Append(this.Box.retdata.BoxInfo.BoxSN).Append("\"");

            sb.Append("}");

            string body = sb.ToString();
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.ReadWriteTimeout = 5000;
            IRestResponse<Res> response = client.Execute<Res>(request);
            if (response.IsSuccessful)
            {
                Res b = response.Data;
                if (b == null || b.Status != 0)
                {
                    string errMsg = null;
                    if (b != null)
                    {
                        errMsg = Constants.GetError(b.Status);
                    }
                    MessageBox.Show(errMsg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    UpdateParentBox();
                    MessageBox.Show("完成装箱", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("无法完成装箱", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PackingFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if(e.KeyCode == Keys.F12)
            {
                printLabelButon_Click(null, null);
            }
        }
    }
}
