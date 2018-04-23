using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Telemetry.Domain.Frame;

namespace Telemetry.App.Test.Domain
{
	[TestClass]
	public class StatusTest
    {
		[TestMethod]
		public void GetDefaultEnergyFrameRequest()
		{
			var status = new Status();

			Assert.AreEqual(0x7D, status.FrameHeader);
			Assert.AreEqual(0x00, status.Lenght);
			Assert.AreEqual(0x02, status.FunctionCode);
			Assert.AreEqual(null, status.Data);
			Assert.AreEqual(0x02, (byte)status.Checksum);
		}

		[TestMethod]
		public void GetStatusValueFromValidStatusFrame()
		{
			var statusFrame = new Status
			{
				FrameHeader = 0x7D,
				Lenght = 0x04,
				FunctionCode = 0x82,
				Data = new byte[] { 0x01, 0x2C, 0x02, 0x58 },
				Checksum = 0xF1
			};

			var resultSizeExpected = 2;

			Assert.AreEqual(resultSizeExpected, statusFrame.GetValue().Length);
		}

		[TestMethod]
		public void GetStatusValueFromInvalidStatusFrame()
		{
			var statusFrame = new Status
			{
				FrameHeader = 0x7D,
				Lenght = 0x04,
				FunctionCode = 0x82,
				Data = new byte[] { 0xFF, 0x01, 0x2C, 0x02, 0x58 },
				Checksum = 0xF1
			};

			Assert.ThrowsException<Exception>(() => statusFrame.GetValue());
		}
	}
}
