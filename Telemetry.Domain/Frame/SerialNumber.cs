using System;
using System.Collections.Generic;

namespace Telemetry.Domain.Frame
{
	public class SerialNumber : Frame
	{
		public SerialNumber()
		{
			this.Lenght = 0x0;
			this.FunctionCode = 0x1;
			this.Checksum = this.VerifyChecksum();
		}

		public IEnumerable<char> GetAsciiCharacters()
		{
			if (this.Data[this.Lenght - 1] != 0x00) throw new Exception("Invalid SerialNumber Record!");

			var charContent = new char[this.Data.Length];

			for (var i = 0; i < this.Data.Length; i++)
			{
				charContent[i] = Convert.ToChar(this.Data[i]);
			}

			return charContent;
		}
	}
}
