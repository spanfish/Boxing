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
	/// Description of BoxListFrm.
	/// </summary>
	public partial class BoxListFrm : Form
	{
		public OrderDetail OrderDetail
		{
			get;
			set;
		}
		
		public List<BoxDetail> DetailList
		{
			get;
			set;
		}
			
		private RestClient client;
		
		private readonly SynchronizationContext synchronizationContext;

        private CancellationTokenSource cts = new CancellationTokenSource();

         public BoxListFrm(OrderDetail orderDetail)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			#region Initialize
			DetailList = new List<BoxDetail>();
			this.OrderDetail = orderDetail;
			this.Text = "装箱列表[订单:" + orderDetail.OrderId + "]";
			this.boxDataGridView.AutoGenerateColumns = false;
            //this.boxDataGridView.VirtualMode = true;
            this.boxDataGridView.ReadOnly = true;
			this.boxDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //this.boxDataGridView.CellValueNeeded += BoxDataGridView_CellValueNeeded;
            //this.boxDataGridView.SelectionChanged += BoxDataGridView_SelectionChanged;

            DataGridViewTextBoxColumn textColumn1 = new DataGridViewTextBoxColumn();
			textColumn1.DataPropertyName = "BoxType";
			textColumn1.Name = "BoxType";
			textColumn1.HeaderText = "箱子类型";
			boxDataGridView.Columns.Add(textColumn1);
			
			DataGridViewTextBoxColumn textColumn2 = new DataGridViewTextBoxColumn();
			textColumn2.DataPropertyName = "BoxSN";
			textColumn2.Name = "BoxSN";
			textColumn2.HeaderText = "箱号";
			boxDataGridView.Columns.Add(textColumn2);

            DataGridViewTextBoxColumn textColumn3 = new DataGridViewTextBoxColumn();
            textColumn3.DataPropertyName = "CreateTime";
            textColumn3.Name = "CreateTime";
            textColumn3.HeaderText = "申请时间";
            boxDataGridView.Columns.Add(textColumn3);
            DataGridViewTextBoxColumn textColumn4 = new DataGridViewTextBoxColumn();
            textColumn4.DataPropertyName = "FinishTime";
            textColumn4.Name = "FinishTime";
            textColumn4.HeaderText = "完成装箱时间";
            boxDataGridView.Columns.Add(textColumn4);
            DataGridViewTextBoxColumn textColumn5 = new DataGridViewTextBoxColumn();
            textColumn5.DataPropertyName = "Status";
            textColumn5.Name = "Status";
            textColumn5.HeaderText = "箱体状态";
            boxDataGridView.Columns.Add(textColumn5);
            DataGridViewTextBoxColumn textColumn6 = new DataGridViewTextBoxColumn();
            textColumn6.DataPropertyName = "Occupied";
            textColumn6.Name = "Occupied";
            textColumn6.HeaderText = "已占用(外箱)";
            boxDataGridView.Columns.Add(textColumn6);
            DataGridViewTextBoxColumn textColumn7 = new DataGridViewTextBoxColumn();
            textColumn7.DataPropertyName = "Capacity";
            textColumn7.Name = "Capacity";
            textColumn7.HeaderText = "容量";
            boxDataGridView.Columns.Add(textColumn7);
            DataGridViewTextBoxColumn textColumn8 = new DataGridViewTextBoxColumn();
            textColumn8.DataPropertyName = "RealCount";
            textColumn8.Name = "RealCount";
            textColumn8.HeaderText = "实际数量";
            boxDataGridView.Columns.Add(textColumn8);
            boxDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            boxDataGridView.AllowUserToAddRows = false;
            

            synchronizationContext = SynchronizationContext.Current;
			
            string host = Constants.Host;
            client = new RestClient(host);
            client.ReadWriteTimeout = 5000;

            //ThreadPool.SetMinThreads(1, 0);
            //ThreadPool.SetMaxThreads(5, 0);

            //cts = new CancellationTokenSource();
            #endregion
        }
        
        void BoxListFrmLoad(object sender, EventArgs e)
		{
			ListBoxAsync();
		}

        public async void ListBoxAsync()
        {
            //innerBoxButton.Enabled = outerBoxButton.Enabled = devOuterBoxButton.Enabled = false;

            var request = new RestRequest("dlicense/v2/manu/boxing/listboxbyworkform", Method.POST);
            request.ReadWriteTimeout = 5000;

            request.RequestFormat = DataFormat.Json;
            request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"orderid\":");
            sb.Append("\"").Append(OrderDetail.OrderId).Append("\"");
            sb.Append("}");

            string body = sb.ToString();

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            //await GetBoxDetail(cts.Token);

            IRestResponse<Box> response = client.Execute<Box>(request);
            if (response.IsSuccessful)
            {
                Box boxRes = response.Data;
                if (boxRes != null)
                {
                    if (boxRes.Status != 0)
                    {
                        ShowError(boxRes.Msg);
                    }
                    else
                    {
                        if (boxRes.retdata == null)
                        {
                            ShowError("无返回值");
                        }
                        else
                        {
                            if (boxRes.retdata.BoxList != null && boxRes.retdata.BoxList.Count > 0)
                            {
                                List<BoxDetail> boxDetailList = new List<BoxDetail>();
                                foreach (BoxList list in boxRes.retdata.BoxList)
                                {
                                    if (list.BoxSN != null)
                                    {
                                        foreach (string boxSN in list.BoxSN)
                                        {
                                            BoxDetail d = new BoxDetail();
                                            d.BoxSN = boxSN;
                                            d.BoxType = list.BoxType;
                                            boxDetailList.Add(d);
                                        }
                                    }

                                }
                                DetailList = boxDetailList;

                                var source = new BindingSource(DetailList, null);
                                //boxDataGridView.RowCount = DetailList.Count;
                                boxDataGridView.DataSource = source;
                                boxDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                                for (int i = 1; i <= boxDataGridView.Rows.Count; i++)
                                {
                                    boxDataGridView.Rows[i - 1].HeaderCell.Value = i.ToString();
                                }
                                boxDataGridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                                await GetBoxDetail(cts.Token);
                            }
                            else
                            {
                                ShowError("无箱子");
                            }
                        }
                    }

                }
                else
                {
                    ShowError("转换错误");
                }
            }
            else
            {
                ShowError(response.ErrorMessage);
            }
            //var asyncHandle = client.ExecuteAsync<Box>(request, response =>
            //{


            //});
        }

        private async Task GetBoxDetail(CancellationToken cancelToken)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < DetailList.Count; i++)
                {
                    if (cancelToken.IsCancellationRequested)
                    {
                        Console.WriteLine("cancelToken");
                        return;
                    }
                    BoxDetail b = DetailList[i];

                    var request = new RestRequest("dlicense/v2/manu/boxing/queryboxinfo", Method.POST);
                    request.RequestFormat = DataFormat.Json;
                    request.ReadWriteTimeout = 5000;
                    request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
                    request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
                    request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
                    request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);
                    request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

                    //Console.WriteLine("GetBoxDetail:" + rowIndex);


                    //调用RestAPI
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{");
                    sb.Append("\"boxsn\":");
                    sb.Append("\"").Append(b.BoxSN).Append("\"");
                    sb.Append("}");

                    string body = sb.ToString();
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse<BoxQuery> response = client.Execute<BoxQuery>(request);
                    //取消调用
                    if (cancelToken.IsCancellationRequested)
                    {
                        Console.WriteLine("cancelToken");
                        return;
                    }
                    BoxQuery boxQuery = response.Data;
                    if (response.IsSuccessful && boxQuery != null && boxQuery.retdata != null)
                    {
                        if (boxQuery.retdata.BoxInfo != null)
                        {
                            b.OrderId = boxQuery.retdata.BoxInfo.OrderId;
                            b.CreateTime = boxQuery.retdata.BoxInfo.CreateTime;
                            b.FinishTime = boxQuery.retdata.BoxInfo.FinishTime;
                            b.Status = boxQuery.retdata.BoxInfo.Status;
                            b.Occupied = boxQuery.retdata.BoxInfo.Occupied;
                            b.Capacity = boxQuery.retdata.BoxInfo.Capacity;
                            b.RealCount = boxQuery.retdata.BoxInfo.RealCount;
                        }
                        
                        synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
                            int r = (int) o;
                            boxDataGridView.InvalidateRow(r);

                        }), i);
                    }
                    //return;
                }
            });
        }            

        public static object GetPropValue(object src, string propName)
        {
            if (src == null || String.IsNullOrEmpty(propName))
            {
                return null;
            }
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private async void BoxDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < DetailList.Count)
            {
                BoxDetail box = DetailList[e.RowIndex];

                DataGridViewColumn column = boxDataGridView.Columns[e.ColumnIndex];
                string propertyName = column.DataPropertyName;
                string value = GetPropValue(box, propertyName) as string;
                if ("CreateTime" == propertyName && String.IsNullOrEmpty(value))
                {
                    e.Value = await GetBoxDetail(cts.Token, e.RowIndex);
                    Console.WriteLine("");
                    boxDataGridView.Rows[e.RowIndex].Cells[3].Value = box.FinishTime;
                    boxDataGridView.Rows[e.RowIndex].Cells[4].Value = box.Status;
                    boxDataGridView.Rows[e.RowIndex].Cells[5].Value = box.Occupied;
                    boxDataGridView.Rows[e.RowIndex].Cells[6].Value = box.Capacity;
                    boxDataGridView.Rows[e.RowIndex].Cells[7].Value = box.RealCount;
                }
                else
                {
                    e.Value = value;
                }
                

                //if (String.IsNullOrEmpty(box.CreateTime))
                //{
                //    await GetBoxDetail(cts.Token, e.RowIndex);
                //}                
            }
        }

        private async Task<string> GetBoxDetail(CancellationToken cancelToken, int rowIndex)
        {
            BoxDetail box = DetailList[rowIndex];

            if (cancelToken.IsCancellationRequested)
            {
                Console.WriteLine("cancelToken");
                return box.CreateTime;
            }
            return await Task.Run(() =>
            {
                
                var request = new RestRequest("dlicense/v2/manu/boxing/queryboxinfo", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.ReadWriteTimeout = 5000;
                request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
                request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
                request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
                request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);
                request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

                Console.WriteLine("GetBoxDetail:" + rowIndex);

                //取消调用
                if (cancelToken.IsCancellationRequested)
                {
                    Console.WriteLine("cancelToken");
                    return box.CreateTime;
                }
                //调用RestAPI
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"boxsn\":");
                sb.Append("\"").Append(box.BoxSN).Append("\"");
                sb.Append("}");

                string body = sb.ToString();
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse<BoxQuery> response = client.Execute<BoxQuery>(request);
                //取消调用
                if (cancelToken.IsCancellationRequested)
                {
                    Console.WriteLine("cancelToken");
                    return box.CreateTime;
                }
                BoxQuery boxQuery = response.Data;
                if (response.IsSuccessful && boxQuery != null && boxQuery.retdata != null)
                {
                    BoxDetail b = boxQuery.retdata.BoxInfo;
                    if (b != null)
                    {
                        box.OrderId = b.OrderId;
                        box.CreateTime = b.CreateTime;
                        box.FinishTime = b.FinishTime;
                        box.Status = b.Status;
                        box.Occupied = b.Occupied;
                        box.Capacity = b.Capacity;
                        box.RealCount = b.RealCount;
                    }

                    //synchronizationContext.Post(new SendOrPostCallback(o =>
                    //{
                    //    Console.WriteLine("InvalidateRow:" + rowIndex);
                    //    if (rowIndex > -1 && rowIndex < DetailList.Count)
                    //    {
                    //        boxDataGridView.InvalidateRow(rowIndex);
                    //        boxDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                    //    }

                    //}), null);
                }
                return box.CreateTime;
            });
        }
		
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
		
		void InnerBoxButtonClick(object sender, EventArgs e)
		{
            BoxDetail box = new BoxDetail();
            box.BoxType = PackingTracker.Common.Constants.BoxTypeInner;
            box.OrderId = this.OrderDetail.OrderId;
            PackingFrm packingFrm = new PackingFrm(box, OrderDetail);

            packingFrm.ShowDialog();
        }

		void BoxDataGridViewCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
            if(e.Button != MouseButtons.Left)
            {
                return;
            }
			if(DetailList.Count > e.RowIndex && e.RowIndex >= 0)
			{
				BoxDetail box = DetailList[e.RowIndex];
                if(String.IsNullOrEmpty(box.CreateTime))
                {
                    MessageBox.Show("正在取得箱子信息，请稍后再试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
				PackingFrm packingFrm = new PackingFrm(box, OrderDetail);

				packingFrm.ShowDialog();
			}
		}

        private void outerBoxButton_Click(object sender, EventArgs e)
        {
            BoxDetail box = new BoxDetail();
            box.BoxType = PackingTracker.Common.Constants.BoxTypeOuterForBox;
            box.OrderId = this.OrderDetail.OrderId;
            PackingFrm packingFrm = new PackingFrm(box, OrderDetail);

            packingFrm.ShowDialog();
        }

        private void boxDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //if (boxDataGridView.SelectedRows.Count > 0)
            //{
            //    int rowIndex = boxDataGridView.SelectedRows[0].Index;
            //    if (DetailList.Count > rowIndex)
            //    {
            //        BoxDetail box = DetailList[rowIndex];
            //        GetBoxDetail(box, rowIndex);
            //    }
            //}

        }

        private void BoxListFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts.Cancel();
        }

        private void printLabelButton_Click(object sender, EventArgs e)
        {

        }

        private void refreshButton_Click_1(object sender, EventArgs e)
        {
            boxDataGridView.Invalidate();
        }

        private void BoxListFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void devOuterBoxButton_Click(object sender, EventArgs e)
        {

        }

        private void deleteBoxButton_Click(object sender, EventArgs e)
        {
            if(boxDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择一个箱子", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int index = boxDataGridView.SelectedRows[0].Index;
            BoxDetail box = DetailList[index];

            if (MessageBox.Show(String.Format("确定要删除箱子:{0}吗", box.BoxSN), "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var request = new RestRequest("dlicense/v2/manu/boxing/dropbox", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.ReadWriteTimeout = 5000;
                request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
                request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
                request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
                request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);
                request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                
                //调用RestAPI
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"orderid\":");
                sb.Append("\"").Append(box.OrderId).Append("\",");
                sb.Append("\"boxsn\":");
                sb.Append("\"").Append(box.BoxSN).Append("\"");
                sb.Append("}");

                string body = sb.ToString();
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse<Res> response = client.Execute<Res>(request);
                Res r = response.Data;
                if (response.IsSuccessful && r != null && r.Status == 0)
                {
                    DetailList.RemoveAt(index);
                    var source = new BindingSource(DetailList, null);
                    boxDataGridView.DataSource = source;
                    boxDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                }
                else
                {
                    string msg = response.ErrorMessage;
                    if (r != null && r.Status != 0)
                    {
                        msg = Constants.GetError(r.Status);
                    }
                    
                    MessageBox.Show("无法删除箱子:" + msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
