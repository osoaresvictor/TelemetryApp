using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Telemetry.Domain.Frame;

namespace Telemetry.App.Test.Domain
{
	[TestClass]
	public class FrameTest
	{
		[TestMethod]
		public void CreateAEmptyFrameObject()
		{
			var newEmptyFrame = new Frame();

			Assert.AreEqual(0x7D, newEmptyFrame.FrameHeader);
			Assert.AreEqual(default(byte), newEmptyFrame.Lenght);
			Assert.AreEqual(default(byte), newEmptyFrame.FunctionCode);
			Assert.AreEqual(null, newEmptyFrame.Data);
			Assert.AreEqual(null, newEmptyFrame.Checksum);
		}

		[TestMethod]
		public void CreateAFrameObjectWithoutDataField()
		{
			var defaultSerialNumberFrameRequest = new byte[] { 0x7D, 0x00, 0x01, 0x01 };

			var newFrameWithoutDataField = new Frame(0x00, 0x01).ToByteArray();

			CollectionAssert.AreEqual(defaultSerialNumberFrameRequest, newFrameWithoutDataField);
		}

		[TestMethod]
		public void CreateAFrameObjectWithDataField()
		{
			var setIndexFrameRequest = new byte[] { 0x7D, 0x02, 0x03, 0x01, 0x7C, 0x7C };

			var newFrameWithDataField = new Frame(0x02, 0x03, new byte[] { 0x01, 0x7C }).ToByteArray();

			CollectionAssert.AreEqual(setIndexFrameRequest, newFrameWithDataField);
		}

		[TestMethod]
		public void VerifyChecksumOfEmptyFrameObject()
		{
			Assert.AreEqual(0x00, new Frame().VerifyChecksum());
		}

		[TestMethod]
		public void VerifyChecksumOfFrameObject()
		{
			var frame = new Frame(0x02, 0x03, new byte[] { 0x01, 0x7C });

			Assert.AreEqual<byte>(0x7C, (byte)frame.Checksum);
		}

		[TestMethod]
		public void ConvertEmptyFrameObjectToByteArray()
		{
			Assert.ThrowsException<Exception>(() => new Frame().ToByteArray());
		}

		[TestMethod]
		public void ConvertFrameObjectToByteArray()
		{
			var defaultSerialNumberRequest = new SerialNumber().ToByteArray();

			var expected = new byte[] { 0x7D, 0x00, 0x01, 0x01 };

			CollectionAssert.AreEqual(expected, defaultSerialNumberRequest);
		}
	}
}
