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

        public DataTable GetTable(string table)
        {
            DataTable dt = null;
            if (File.Exists(FilePath))
            {
                using (var conn = new SQLiteConnection("Data Source=" + FilePath))
                {
                    try
                    {
                        conn.Open();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandText = "SELECT * from " + table;

                            using (SQLiteDataReader dr = command.ExecuteReader())
                            {
                                if(dr != null)
                                {
                                    dt.Load(dr);
                                }
                            }
                                
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

            return dt;
        }
       
        public SQLiteConnection Open()
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }
            var conn = new SQLiteConnection("Data Source=" + FilePath);

            try
            {
                conn.Open();
            }
            finally
            {

            }
            return conn;
        }

        public void Close(SQLiteConnection conn)
        {
            if (conn != null)
            { 
                conn.Close();
            }
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

                            command.CommandText = "create table Setting(Id TEXT, Name Text, DefValue TEXT, PRIMARY KEY (Id, Name))";
                            command.ExecuteNonQuery();

                            command.CommandText = "create table Print(Id TEXT, Name Text, DispSeq INTEGER, DispText Text, BarCode TEXT, PRIMARY KEY (Id, Name))";
                            command.ExecuteNonQuery();
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
