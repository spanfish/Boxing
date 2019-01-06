using PackingTracker.Common;
using PackingTracker.Entity;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PackingTracker.UI
{
    /// <summary>
    /// Description of MainFrm.
    /// </summary>
    public partial class MainFrm : Form
	{
		private RestClient client;
		
		private int index;
		
        private int pageSize;

        private readonly SynchronizationContext synchronizationContext;
		
		private List<OrderDetail> orderList;
		
		public MainFrm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			#region Initialize
			synchronizationContext = SynchronizationContext.Current;
            
            string host = Constants.Host;
            client = new RestClient(host);
            client.ReadWriteTimeout = 5000;

            index = 0;
            pageSize = 50;
            nextPageButton.Enabled = prePageButton.Enabled = refreshButton.Enabled = false;
            #endregion


        }
		
		void MainFrmLoad(object sender, EventArgs e)
		{
			#region Login
			LoginFrm loginFrm = new LoginFrm();
			DialogResult dialogResult = loginFrm.ShowDialog();
			if(dialogResult == DialogResult.OK)
			{
                DataGridViewTextBoxColumn textColumn1 = new DataGridViewTextBoxColumn();
                textColumn1.DataPropertyName = "OrderId";
                textColumn1.Name = "OrderId";
                textColumn1.HeaderText = "订单号码";
                orderDataGridView.Columns.Add(textColumn1);

                DataGridViewTextBoxColumn textColumn2 = new DataGridViewTextBoxColumn();
                textColumn2.DataPropertyName = "ProductName";
                textColumn2.Name = "ProductName";
                textColumn2.HeaderText = "产品名称";
                orderDataGridView.Columns.Add(textColumn2);

                DataGridViewTextBoxColumn textColumn3 = new DataGridViewTextBoxColumn();
                textColumn3.DataPropertyName = "MaterialCode";
                textColumn3.Name = "MaterialCode";
                textColumn3.HeaderText = "物料编码";
                orderDataGridView.Columns.Add(textColumn3);

                DataGridViewTextBoxColumn textColumn4 = new DataGridViewTextBoxColumn();
                textColumn4.DataPropertyName = "Workform";
                textColumn4.Name = "Workform";
                textColumn4.HeaderText = "订单名称";
                orderDataGridView.Columns.Add(textColumn4);

                DataGridViewTextBoxColumn textColumn5 = new DataGridViewTextBoxColumn();
                textColumn5.DataPropertyName = "AgreementId";
                textColumn5.Name = "AgreementId";
                textColumn5.HeaderText = "合同ID";
                orderDataGridView.Columns.Add(textColumn5);

                DataGridViewTextBoxColumn textColumn6 = new DataGridViewTextBoxColumn();
                textColumn6.DataPropertyName = "ConfirmStatus";
                textColumn6.Name = "ConfirmStatus";
                textColumn6.HeaderText = "订单确认状态";
                orderDataGridView.Columns.Add(textColumn6);

                DataGridViewTextBoxColumn textColumn7 = new DataGridViewTextBoxColumn();
                textColumn7.DataPropertyName = "ProduceStatus";
                textColumn7.Name = "ProduceStatus";
                textColumn7.HeaderText = "生产状态";
                orderDataGridView.Columns.Add(textColumn7);

                DataGridViewTextBoxColumn textColumn8 = new DataGridViewTextBoxColumn();
                textColumn8.DataPropertyName = "BoxStatus";
                textColumn8.Name = "BoxStatus";
                textColumn8.HeaderText = "装箱状态";
                orderDataGridView.Columns.Add(textColumn8);

                DataGridViewTextBoxColumn textColumn9 = new DataGridViewTextBoxColumn();
                textColumn9.DataPropertyName = "Count";
                textColumn9.Name = "Count";
                textColumn9.HeaderText = "订单设备数";
                orderDataGridView.Columns.Add(textColumn9);
                orderDataGridView.AutoGenerateColumns = false;
                orderDataGridView.AllowUserToAddRows = false;
                ListOrder(0);
			}
            #endregion
        }

        void ListOrder(int ind)
		{
            nextPageButton.Enabled = prePageButton.Enabled = refreshButton.Enabled = false;
            if (ind < 0)
            {
                ind = 0;
            }
			var request = new RestRequest("dlicense/v2/manu/query", Method.POST);
            
            request.RequestFormat = DataFormat.Json;
			request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);
            
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"index\":");
            sb.Append(string.Format("{0},", ind));

            sb.Append("\"pagesize\":");
            sb.Append(pageSize).Append(",");

            sb.Append("\"sortby\":");
            sb.Append("\"createtime\",");

            sb.Append("\"sortorder\":");
            sb.Append("\"desc\",");

            sb.Append("\"checkstatus\":");
            sb.Append("-1,");

            sb.Append("\"producestatus\":");
            sb.Append("-1");

            sb.Append("}");
            
            string body = sb.ToString();

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            IRestResponse<Order> response = client.Execute<Order>(request);
            //var asyncHandle = client.ExecuteAsync<Order>(request, response => {
            if(response.IsSuccessful)
            {
                Order orderRes = response.Data;
                if(orderRes != null)
                {
                    //synchronizationContext.Post(new SendOrPostCallback(o =>
                    //{
                	    //Order order = o as Order;
                       	if(orderRes.Status == 0)
                       	{
                       		orderList = orderRes.OrderList;
	                        if(orderList == null)
	                        {
	                            orderList = new List<OrderDetail>();
	                        }
	                        var source = new BindingSource(orderList, null);
	                        orderDataGridView.DataSource = source;
                            orderDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            if(orderList.Count > 0)
                            {
                                index = ind;
                            }                            
                       	}
                       	else
                       	{
                       		ShowError(String.Format("错误代码:{0}, 错误消息:{1}", orderRes.Status, orderRes.Msg));
                       	}
                        
                    //}), orderRes);
                }
                else
                {
                	ShowError("转换错误");
                }
            }
            else
            {
                ShowError("未知错误");
            }

            //});
            prePageButton.Enabled = index > 0;
            nextPageButton.Enabled = true;
            refreshButton.Enabled = true;
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
		
		void OrderDataGridViewCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
            if (e.RowIndex >= 0 && e.RowIndex < orderList.Count)
            {
                OrderDetail orderDetail = orderList[e.RowIndex];
                BoxListFrm boxListFrm = new BoxListFrm(orderDetail);

                boxListFrm.ShowDialog();
            }
		}

		void PrePageButtonClick(object sender, EventArgs e)
		{
            ListOrder(index - pageSize);

        }
		void NextPageButtonClick(object sender, EventArgs e)
		{
            ListOrder(index + pageSize);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            ListOrder(index);
        }
    }
}
