using System.Threading;
using System.Collections.Generic;

namespace PackingTracker.Entity
{
	/// <summary>
	/// Description of Box.
	/// </summary>
	public class Box
	{
		public int Status { get; set; }

        public string Msg { get; set; }
        
        public RetData retdata{get;set;}

        public Box()
        {
            this.retdata = new RetData();
        }
	}
	
	
	
	public class RetData
	{
		public List<BoxList> BoxList
		{
			get;
			set;
		}
		
		public BoxDetail BoxInfo
		{
			get;
			set;
		}

        public int Count { get; set; }

        public List<DevInfo> DevInfo
        {
            get;
            set;
        }

        //查询外箱时用
        /// <summary>
        /// 内箱SN
        /// </summary>
        public List<string> BoxSN
        {
            get;
            set;
        }
        public List<BoxDetail> InnerBoxes
        {
            get;
            set;
        }
    }
	
    public class DevInfo
    {
        public string BoxSN { get; set; }
        public string Did { get; set; }
        public string BindUserId { get; set; }
        public string BindTime { get; set; }
    }

    public class BoxList
	{
		public string BoxType { get; set; }

        public List<string> BoxSN { get; set; }
	}

    public class BoxDetail
    {
        public string BoxType { get; set; }

        public string BoxSN { get; set; }

        public string Oemfactoryid { get; set; }

        public string Requserid { get; set; }

        public string OrderId { get; set; }

        public string CreateTime { get; set; }

        public string FinishTime { get; set; }

        public string Status { get; set; }

        public int Occupied { get; set; }

        public int Capacity { get; set; }

        public int RealCount { get; set; }
    }
	
	/// <summary>
	/// 箱子信息查询
	/// </summary>
	public class BoxQuery
	{
		public int Status { get; set; }

        public string Msg { get; set; }
        
        public RetData retdata{get;set;}
	}
}
