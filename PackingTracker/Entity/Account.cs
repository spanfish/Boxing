using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingTracker.Entity
{
	/// <summary>
	/// Description of Account.
	/// </summary>
	public class AccountDetail
	{
		public string Userid { get; set; }
        public string Nickname { get; set; }
        public string Grouptype { get; set; }
        public string OemfactoryId { get; set; }
        public string OemfactoryName { get; set; }
        public List<string> Roletype { get; set; }
	}
	
	public class Account
    {
        public int Status { get; set; }

        public string Msg { get; set; }

        public List<AccountDetail> Data { get; set; }
    }
}
