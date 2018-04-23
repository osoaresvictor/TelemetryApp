namespace Telemetry.Domain.Frame
{
	public class Status : Frame
	{
		public Status()
		{
			this.Lenght = 0x00;
			this.FunctionCode = 0x02;
			this.Checksum = this.VerifyChecksum();
		}

		public ushort[] GetValue()
		{
			if (this.Data.Length > 4) throw new System.Exception("Invalid Status Record!");
			
			var status = new ushort[2];

			status[0] = this.CombineBytes(this.Data[0], this.Data[1]);
			status[1] = this.CombineBytes(this.Data[2], this.Data[3]);

			return status;
		}

		private ushort CombineBytes(byte leftByte, byte rightByte)
		{
			return (ushort)((leftByte << 8) | rightByte);
		}
	}
}
