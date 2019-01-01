using System;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace PackingTracker.Common
{
	/// <summary>
	/// Description of DBHelper.
	/// </summary>
	public sealed class DBHelper
	{
		private static readonly string FilePath = @".\box.db";
		
		private DBHelper()
		{
			CreateDB();
		}
		
		private static DBHelper instance = new DBHelper();

        public static DBHelper Instance
        {
            get
            {
                return instance;
            }
        }
		
        /// <summary>
        /// 向箱内添加设备
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="boxSN"></param>
        /// <param name="did"></param>
        /// <returns></returns>
		public int AddDevice(string orderId, string boxSN, string did)
		{
			int ret = 0;
			if(String.IsNullOrEmpty(orderId) || String.IsNullOrEmpty(boxSN) || String.IsNullOrEmpty(did))
			{
				return ret;
			}
			using (var conn = new SQLiteConnection("Data Source=" + FilePath))
			{
				try
				{
					conn.Open();
					
					using (SQLiteCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "insert into boxing(OrderId, BoxSN, DID, Status) values (@OrderId, @BoxSN, @DID, 0)";
                        command.Parameters.Add("@OrderId", DbType.String);
                        command.Parameters.Add("@BoxSN", DbType.String);
                        command.Parameters.Add("@DID", DbType.String);

                        command.Parameters["@OrderId"].Value = orderId;
                        command.Parameters["@BoxSN"].Value = boxSN;
                        command.Parameters["@DID"].Value = did;
                        ret = command.ExecuteNonQuery();
                    }
				}
				catch(Exception)
				{
					
				}
				finally
				{
					if (conn != null)
                    {
                        conn.Close();
                    }
				}
			}
			
			return ret;
		}
		
		/// <summary>
		/// 初始化数据库
		/// </summary>
		private void CreateDB()
		{
			if (!File.Exists(FilePath))
			{
				using (FileStream stream = File.Create(FilePath))
				{
				}
				
				using (var conn = new SQLiteConnection("Data Source=" + FilePath))
				{
					try
					{
						conn.Open();
						
						using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandText = "create table boxing(OrderId TEXT, BoxSN Text, DID TEXT, Status Integer, PRIMARY KEY (OrderId, BoxSN))";
                            command.ExecuteNonQuery();

                            //command.CommandText = "create table Print(Id TEXT, Name Text, DispSeq INTEGER, DispText Text, BarCode TEXT, PRIMARY KEY (Id, Name))";
                            //command.ExecuteNonQuery();
                        }
					}
					finally
					{
						if (conn != null)
                        {
                            conn.Close();
                        }
					}
				}
			}
		}
	}
}
