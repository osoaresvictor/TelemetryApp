using System;

namespace Telemetry.Domain.Frame
{
	public class DateTime : Frame
	{
		public DateTime()
		{
			this.Lenght = 0x0;
			this.FunctionCode = 0x4;
			this.Checksum = this.VerifyChecksum();
		}

		public System.DateTime GetDateTime()
		{
			var binaryDateTime = default(string);

			foreach (var item in this.Data)
			{
				binaryDateTime += Convert.ToString(item, 2).PadLeft(8, '0');
			}

			var year = Convert.ToInt32(binaryDateTime.Substring(0, 12), 2);
			var month = Convert.ToInt32(binaryDateTime.Substring(12, 4), 2);
			var day = Convert.ToInt32(binaryDateTime.Substring(16, 5), 2);

			var hour = Convert.ToInt32(binaryDateTime.Substring(21, 5), 2);
			var minute = Convert.ToInt32(binaryDateTime.Substring(26, 6), 2);
			var second = Convert.ToInt32(binaryDateTime.Substring(32, 6), 2);

			return new System.DateTime(year, month, day, hour, minute, second);
		}
	}
}
