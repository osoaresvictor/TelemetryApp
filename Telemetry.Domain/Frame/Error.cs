namespace Telemetry.Domain.Frame
{
	public class Error : Frame
	{
		public Error()
		{
			this.Lenght = 0x00;
			this.FunctionCode = 0xFF;
			this.Checksum = this.VerifyChecksum();
		}
	}
}
