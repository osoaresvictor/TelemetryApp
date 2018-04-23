namespace Telemetry.Domain.Frame
{
	public interface IFrame
    {
		byte FrameHeader { get; set; }
		byte Lenght { get; set; }
		byte FunctionCode { get; set; }
		byte[] Data { get; set; }
		byte? Checksum { get; set; }

		byte VerifyChecksum();
		byte[] ToByteArray();
	}
}
