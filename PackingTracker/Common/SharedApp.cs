using System;
using PackingTracker.Entity;

namespace PackingTracker.Common
{
	/// <summary>
	/// Description of SharedApp.
	/// </summary>
	public sealed class SharedApp
	{
		private static readonly SharedApp instance = new SharedApp();

        public static SharedApp Instance 
        {
            get {
                return instance;
            }
        }

        private SharedApp()
        {
        }
		
        /// <summary>
        /// 登陆信息
        /// </summary>
		public Login Login { get; set; }
		
		/// <summary>
		/// 账户详细
		/// </summary>
		public AccountDetail AccountDetail { get; set; }
	}
}
