namespace Telemetry.Domain.Frame.Builders
{
	public static class FrameObjectBuilder
    {
		public static T ToFrameObject<T>(this byte[] data) where T : IFrame, new()
		{
			return new T
			{
				FrameHeader = data[0],
				Lenght = data[1],
				FunctionCode = data[2],
				Data = (data[1] == 0) ? null : data.GetDataField(),
				Checksum = data[data.Length - 1]
			};
		}
	}
}
