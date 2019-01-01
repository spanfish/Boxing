using System;

namespace PackingTracker.Entity
{
	/// <summary>
	/// Description of Handshake.
	/// </summary>
	public class Handshake
	{
		public Handshake()
		{
			
		}
		
		public int Status { get; set; }

        public string Msg { get; set; }

        public string Timestamp { get; set; }
	}
}
