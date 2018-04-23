using System;
using System.Linq;

namespace Telemetry.Domain.Frame
{
	public class Index : Frame
	{
		public Index() { }

		public Index(int index)
		{
			this.Lenght = 0x02;
			this.FunctionCode = 0x03;
			this.Data = BitConverter.GetBytes(index)
											 .Reverse()
											 .Skip(2) //because .GetBytes return 4 bytes array
											 .ToArray();
			this.Checksum = this.VerifyChecksum();
		}

		public bool IsValid()
		{
			return (this.FunctionCode == 0x83 && this.Data[0] == 0);
		}
	}
}
