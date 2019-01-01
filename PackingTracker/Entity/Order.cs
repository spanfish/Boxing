using System;
using RestSharp.Deserializers;
using System.Collections.Generic;
namespace PackingTracker.Entity
{
	/// <summary>
	/// Description of Order.
	/// </summary>
	public class Order
	{
		public int Status
        {
            get;
            set;
        }

        public string Msg
        {
            get;
            set;
        }

        public List<OrderDetail> OrderList
        {
            get;
            set;
        }
	}
	
	public class OrderDetail
    {
        /**
        * 订单编码
        **/
		public string AgreementId
        {
            get;
            set;
        }
		
		public string BoxFinishTime
        {
            get;
            set;
        }
		
		public string BoxStartTime
        {
            get;
            set;
        }

        private string _boxStatus;

        public string BoxStatus
        {
            get
            {
                return _boxStatus;
            }
            set
            {
                if(value == "0")
                {
                    _boxStatus = "未装箱";
                }
                else if (value == "1")
                {
                    _boxStatus = "正在装箱";
                }
                else if (value == "2")
                {
                    _boxStatus = "完成装箱";
                }
            }
        }
		public string CheckStatus
        {
            get;
            set;
        }
		public string CheckTime
        {
            get;
            set;
        }

        private string _confirmStatus;

        public string ConfirmStatus
        {
            get
            {
                return _confirmStatus;
            }
            set
            {
                if("yes" == value)
                {
                    _confirmStatus = "已确认";
                }
                else
                {
                    _confirmStatus = "未确认";
                }
            }
        }
		public string ConfirmTime
        {
            get;
            set;
        }
		
		public int Count
        {
            get;
            set;
        }
		public string FactoryName
        {
            get;
            set;
        }
		public string FinishTime
        {
            get;
            set;
        }
		public string MaterialCode
        {
            get;
            set;
        }
		
		public string OEMFactoryId
        {
            get;
            set;
        }
        public string OrderId
        {
            get;
            set;
        }

        public string PlatCode
        {
            get;
            set;
        }

        private string _produceStatus;
        public string ProduceStatus
        {
            get
            {
                return _produceStatus;
            }
            set
            {
                if(value == "0")
                {
                    _produceStatus = "未生产";
                }
                else if (value == "1")
                {
                    _produceStatus = "正在生产";
                }
                else if (value == "2")
                {
                    _produceStatus = "完成生产";
                }
                else if (value == "6")
                {
                    _produceStatus = "订单被取消";
                }
                else if (value == "7")
                {
                    _produceStatus = "订单被中止";
                }
            }
        }
        public string ProductId
        {
            get;
            set;
        }
        public string ProductModel
        {
            get;
            set;
        }
        
        public string ProductName
        {
            get;
            set;
        }
        public string ProductPcbVer
        {
            get;
            set;
        }
        
        public string SalesUserName
        {
            get;
            set;
        }
        
        public string StartTime
        {
            get;
            set;
        }
        
        public string UpdateTime
        {
            get;
            set;
        }
        public string Workform
        {
            get;
            set;
        }
    }
}
