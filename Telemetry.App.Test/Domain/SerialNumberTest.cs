using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Telemetry.Domain.Frame;

namespace Telemetry.App.Test.Domain
{
	[TestClass]
	public class SerialNumberTest
	{
		[TestMethod]
		public void GetDefaultSerialNumberFrameRequest()
		{
			var serialNumber = new SerialNumber();

			Assert.AreEqual(0x7D, serialNumber.FrameHeader);
			Assert.AreEqual(0x00, serialNumber.Lenght);
			Assert.AreEqual(0x01, serialNumber.FunctionCode);
			Assert.AreEqual(null, serialNumber.Data);
			Assert.AreEqual(0x01, (byte)serialNumber.Checksum);
		}

		[TestMethod]
		public void GetSerialNumberValueFromValidSerialNumberResponseFrame()
		{
			var serialNumberFrame = new SerialNumber
			{
				FrameHeader = 0x7D,
				Lenght = 0x08,
				FunctionCode = 0x81,
				Data = new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x00 },
				Checksum = 0xC9
			};

			var expected = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', Convert.ToChar(0) };

			CollectionAssert.AreEqual(expected, serialNumberFrame.GetAsciiCharacters().ToArray());
		}

		[TestMethod]
		public void GetSerialNumberValueFromInvalidSerialNumberFrame()
		{
			var serialNumberFrame = new SerialNumber
			{
				FrameHeader = 0x7D,
				Lenght = 0x06,
				FunctionCode = 0x81,
				Data = new byte[] { 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF},
				Checksum = 0xC9
			};

			Assert.ThrowsException<Exception>(() => serialNumberFrame.GetAsciiCharacters());
		}
	}
}
