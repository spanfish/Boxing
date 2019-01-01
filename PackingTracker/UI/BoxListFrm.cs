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
            //this.boxDataGridView.CellValueNeeded += new DataGridViewCellValueEventHandler(boxDataGridView_CellValueNeeded);
            this.boxDataGridView.SelectionChanged += BoxDataGridView_SelectionChanged;

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
			
			//DataGridViewTextBoxColumn textColumn3 = new DataGridViewTextBoxColumn();
			//textColumn3.DataPropertyName = "CreateTime";
			//textColumn3.Name = "CreateTime";
			//textColumn3.HeaderText = "申请时间";
			//boxDataGridView.Columns.Add(textColumn3);
			
			//DataGridViewTextBoxColumn textColumn4 = new DataGridViewTextBoxColumn();
			//textColumn4.DataPropertyName = "FinishTime";
			//textColumn4.Name = "FinishTime";
			//textColumn4.HeaderText = "完成装箱时间";
			//boxDataGridView.Columns.Add(textColumn4);
			
			//DataGridViewTextBoxColumn textColumn5 = new DataGridViewTextBoxColumn();
			//textColumn5.DataPropertyName = "Status";
			//textColumn5.Name = "Status";
			//textColumn5.HeaderText = "箱体状态";
			//boxDataGridView.Columns.Add(textColumn5);
			
			//DataGridViewTextBoxColumn textColumn6 = new DataGridViewTextBoxColumn();
			//textColumn6.DataPropertyName = "Occupied";
			//textColumn6.Name = "Occupied";
			//textColumn6.HeaderText = "已占用(外箱)";
			//boxDataGridView.Columns.Add(textColumn6);
			
			//DataGridViewTextBoxColumn textColumn7 = new DataGridViewTextBoxColumn();
			//textColumn7.DataPropertyName = "Capacity";
			//textColumn7.Name = "Capacity";
			//textColumn7.HeaderText = "容量";
			//boxDataGridView.Columns.Add(textColumn7);
			
			//DataGridViewTextBoxColumn textColumn8 = new DataGridViewTextBoxColumn();
			//textColumn8.DataPropertyName = "RealCount";
			//textColumn8.Name = "RealCount";
			//textColumn8.HeaderText = "实际数量";
			//boxDataGridView.Columns.Add(textColumn8);
			//boxDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            boxDataGridView.AllowUserToAddRows = false;

            synchronizationContext = SynchronizationContext.Current;
			
            string host = Constants.Host;
            client = new RestClient(host);
            client.ReadWriteTimeout = 5000;
            #endregion
        }

        private void BoxDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if(boxDataGridView.SelectedRows.Count > 0)
            {
                int index = boxDataGridView.SelectedRows[0].Index;
                if(index > -1 && index < DetailList.Count)
                {
                    BoxDetail box = DetailList[index];
                    GetBoxDetail(box, index);
                }
            }            
        }

        void BoxListFrmLoad(object sender, EventArgs e)
		{
			ListBox();
		}
		
		public void ListBox()
		{
            innerBoxButton.Enabled = outerBoxButton.Enabled = devOuterBoxButton.Enabled = false;

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
            
            var asyncHandle = client.ExecuteAsync<Box>(request, response => {
                if (response.IsSuccessful)
	            {
	                Box boxRes = response.Data;
	                if(boxRes != null)
	                {
	                    if(boxRes.Status != 0)
	                    {
	                	    ShowError(boxRes.Msg);
	                    }
	                    else
	                    {
	                	    if(boxRes.retdata == null)
	                	    {
	                		    ShowError("无返回值");
	                	    }
	                	    else
	                	    {
	                		    if(boxRes.retdata.BoxList != null && boxRes.retdata.BoxList.Count > 0)
	                		    {
                                    synchronizationContext.Post(new SendOrPostCallback(o =>
                                    {
                                        List<BoxList> boxList = o as List<BoxList>;
                                        List<BoxDetail> boxDetailList = new List<BoxDetail>();
                                        foreach(BoxList list in boxList)
                                        {
                                            if(list.BoxSN != null)
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
                                        var source = new BindingSource(boxDetailList, null);
                                        boxDataGridView.DataSource = source;
                                    }), boxRes.retdata.BoxList);                                    
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
            
            });
        }
		
		//public static object GetPropValue(object src, string propName)
		//{
		//	if(src == null || String.IsNullOrEmpty(propName))
		//	{
		//		return null;
		//	}
		//	return src.GetType().GetProperty(propName).GetValue(src, null);
		//}
		
		//void boxDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
  //  	{
		//	if(DetailList.Count > e.RowIndex)
		//	{
		//		BoxDetail box = DetailList[e.RowIndex];	
		//		DataGridViewColumn column = boxDataGridView.Columns[e.ColumnIndex];
		//		string propertyName = column.DataPropertyName;
		//		e.Value = GetPropValue(box, propertyName);
  //              if (propertyName == "BoxSN" && String.IsNullOrEmpty(box.CreateTime))
  //              {
  //                  GetBoxDetail(box, e.RowIndex);
  //              }
  //          }
  //  	}
		
		public void GetBoxDetail(BoxDetail detail, int rowIndex)
		{
			if(!String.IsNullOrEmpty(detail.CreateTime))
			{
				return;
			}
			var request = new RestRequest("dlicense/v2/manu/boxing/queryboxinfo", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);
            
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"boxsn\":");
            sb.Append("\"").Append(detail.BoxSN).Append("\"");
            sb.Append("}");
            
            string body = sb.ToString();
            //ShowError("body=" + body);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            
            var asyncHandle = client.ExecuteAsync<BoxQuery>(request, response => {
				BoxQuery boxQuery = response.Data;
	            if(response.IsSuccessful && boxQuery != null && boxQuery.retdata != null)
	            {
	            	synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
	            	   	BoxDetail box = o as BoxDetail;

	            	   	if(box != null)
	            	   	{
	            	   		detail.OrderId = box.OrderId;
	            	   		detail.CreateTime = box.CreateTime;
	            	   		detail.FinishTime = box.FinishTime;
	            	   		detail.Status = box.Status;
	            	   		detail.Occupied = box.Occupied;
	            	   		detail.Capacity = box.Capacity;
	            	   		detail.RealCount = box.RealCount;
	            	   	}
	            	   	
	            	   	
                        //boxDataGridView.InvalidateRow(rowIndex);
						//boxDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                    }), boxQuery.retdata.BoxInfo);
	            }
	            else
	            {

	            }

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
            PackingFrm packingFrm = new PackingFrm(box);

            packingFrm.ShowDialog();
        }

		void BoxDataGridViewCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if(DetailList.Count > e.RowIndex && e.RowIndex >= 0)
			{
				BoxDetail box = DetailList[e.RowIndex];
				PackingFrm packingFrm = new PackingFrm(box);

				packingFrm.ShowDialog();
			}
		}

        private void outerBoxButton_Click(object sender, EventArgs e)
        {
            BoxDetail box = new BoxDetail();
            box.BoxType = PackingTracker.Common.Constants.BoxTypeOuterForBox;
            box.OrderId = this.OrderDetail.OrderId;
            PackingFrm packingFrm = new PackingFrm(box);

            packingFrm.ShowDialog();
        }

        private void boxDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (boxDataGridView.SelectedRows.Count > 0)
            {
                int rowIndex = boxDataGridView.SelectedRows[0].Index;
                if (DetailList.Count > rowIndex)
                {
                    BoxDetail box = DetailList[rowIndex];
                    GetBoxDetail(box, rowIndex);
                }
            }

        }
    }
}
