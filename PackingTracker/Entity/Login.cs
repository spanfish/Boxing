using System;

namespace PackingTracker.Entity
{
	/// <summary>
	/// Description of Login.
	/// </summary>
	public class Login
	{
		public int Status { get; set; }

        public string Msg { get; set; }

        public string Userid { get; set; }

        public string Loginsession { get; set; }

        public string Nickname { get; set; }

        public string Loginip { get; set; }

        public string Logintime { get; set; }
	}
}
