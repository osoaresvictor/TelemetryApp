using System.Net;

namespace Telemetry.App.Model
{
	public class StartupParameters
	{
		public IPEndPoint EndPoint { get; set; }
		public int FirstIndex { get; set; }
		public int LastIndex { get; set; }
	}
}
