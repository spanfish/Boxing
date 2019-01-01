using System;

namespace PackingTracker.Common
{
	/// <summary>
	/// Description of Constants.
	/// </summary>
	public sealed class Constants
	{
		/// <summary>
		/// 内箱
		/// </summary>
		public static string BoxTypeInner = "inner";
		
		/// <summary>
		/// 设备外箱
		/// </summary>
		public static string BoxTypeOuterForDev = "outerfordev";
		
		/// <summary>
		/// 外箱
		/// </summary>
		public static string BoxTypeOuterForBox = "outerforbox";
		
		private Constants()
		{
		}
#if DEBUG
		public static string Host = "https://lifecycle2.ibroadlink.com";
#else
		public static string Host = "https://lifecyle2.ibroadlink.com";
#endif
	}
}
