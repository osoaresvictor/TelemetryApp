using System;
using System.Collections.Generic;

namespace Telemetry.Domain.Frame
{
	public class Frame : IFrame
	{
		public byte FrameHeader { get; set; } = 0x7D; //default frame header;
		public byte Lenght { get; set; }
		public byte FunctionCode { get; set; }
		public byte[] Data { get; set; }
		public byte? Checksum { get; set; }

		public Frame() { }

		public Frame(byte Lenght, byte FunctionCode, byte[] Data = null)
		{
			this.Lenght = Lenght;
			this.FunctionCode = FunctionCode;
			this.Data = Data;
			this.Checksum = this.VerifyChecksum();
		}

		public byte VerifyChecksum()
		{
			var checksum = default(byte);

			if (this.Data != null)
			{
				foreach (var dataItem in this.Data)
				{
					checksum = (byte)(checksum ^ dataItem);
				}
			}

			checksum = (byte)(checksum ^ this.Lenght ^ this.FunctionCode);

			return checksum;
		}

		public byte[] ToByteArray()
		{
			if (this.Lenght == 0 && this.FunctionCode == 0 && this.Checksum == null)
				throw new Exception("This frame can't be converted because your fields are empty!");

			var bytes = new List<byte>();
			bytes.Add(this.FrameHeader);
			bytes.Add(this.Lenght);
			bytes.Add(this.FunctionCode);

			if (this.Data != null)
			{
				bytes.AddRange(this.Data);
			}

			if (this.Checksum == null) throw new Exception("Invalid Frame, Checksum is null!");
			bytes.Add((byte)this.Checksum);

			return bytes.ToArray();
		}
	}
}
