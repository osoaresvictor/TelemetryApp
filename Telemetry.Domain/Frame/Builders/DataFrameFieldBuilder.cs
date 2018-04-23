using System;

namespace Telemetry.Domain.Frame.Builders
{
	public static class DataFrameFieldBuilder
    {
		public static byte[] GetDataField(this byte[] frame)
		{
			if (frame.Length < 4) throw new Exception("Invalid Frame, data.lenght < 3 bytes!");

			var firstDataByteIndex = 3;
			var lastDataByteIndex = (frame.Length - 1) - 1;
			var quantityBytes = (lastDataByteIndex - firstDataByteIndex) + 1;

			var data = new byte[quantityBytes];

			for (var i = firstDataByteIndex; i <= lastDataByteIndex; i++)
			{
				data[i - firstDataByteIndex] = frame[i];
			}

			return data;
		}
	}
}
