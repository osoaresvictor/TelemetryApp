using System;

namespace Telemetry.Domain.Frame
{
	public class Energy : Frame
	{
		public Energy()
		{
			this.Lenght = 0x00;
			this.FunctionCode = 0x05;
			this.Checksum = this.VerifyChecksum();
		}

		public float GetEnergyValue()
		{
			var bytes = this.CombineBytes(this.Data);
			var value = BitConverter.ToSingle(bytes, 0);
			return BitConverter.ToSingle(bytes, 0);
		}

		private byte[] CombineBytes(byte[] data)
		{
			if (data.Length > 4) throw new Exception("Invalid Energy Record!");

			byte firstleftByte = data[0],
					secondByte = data[1],
					thirdByte = data[2],
					lastRightByte = data[3];

			return BitConverter.GetBytes((firstleftByte << (3 * 8)) | (secondByte << (2 * 8)) | (thirdByte << (1 * 8)) | lastRightByte);
		}
	}
}