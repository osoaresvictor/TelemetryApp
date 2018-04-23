using Telemetry.Domain.Frame;

namespace Telemetry.Domain
{
	public class RecordContent
    {
		public int Index { get; set; }
		public Energy Energy { get; set; }
		public DateTime DateTime { get; set; }
	}
}
