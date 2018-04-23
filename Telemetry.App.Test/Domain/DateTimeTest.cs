using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Telemetry.App.Test.Domain
{
	[TestClass]
	public class DateTimeTest
	{
		[TestMethod]
		public void GetDefaultDateTimeFrameRequest()
		{
			var datetime = new Telemetry.Domain.Frame.DateTime();

			Assert.AreEqual(0x7D, datetime.FrameHeader);
			Assert.AreEqual(0x00, datetime.Lenght);
			Assert.AreEqual(0x04, datetime.FunctionCode);
			Assert.AreEqual(null, datetime.Data);
			Assert.AreEqual(0x04, (byte)datetime.Checksum);
		}

		[TestMethod]
		public void GetDateTimeValueFromValidDateTimeFrame()
		{
			var dateTimeFrameResponse = new Telemetry.Domain.Frame.DateTime
			{
				FrameHeader = 0x7D,
				Lenght = 0x05,
				FunctionCode = 0x84,
				Data = new byte[] { 0x7D, 0xE1, 0xBC, 0x59, 0x2B },
				Checksum = 0xD3
			};

			var expected = new System.DateTime(2014, 01, 23, 17, 25, 10);

			Assert.AreEqual(expected, dateTimeFrameResponse.GetDateTime());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void GetDateTimeValueFromInvalidDateTimeFrame()
		{
			var dateTimeFrameResponse = new Telemetry.Domain.Frame.DateTime
			{
				FrameHeader = 0x7D,
				Lenght = 0x05,
				FunctionCode = 0x84,
				Data = new byte[] { 0x99, 0x7D, 0xE1, 0xBC, 0x59, 0x2B },
				Checksum = 0xD3
			};

			var datetime = dateTimeFrameResponse.GetDateTime();
		}
	}
}
