using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Telemetry.Domain.Frame;
using Telemetry.Domain.Frame.Builders;

namespace Telemetry.App.Test.Domain
{
	[TestClass]
	public class DataFrameFieldBuilderTest
	{
		[TestMethod]
		public void GetDataFieldOfValidFrameByteArray()
		{
			var frame = new Frame(0x02, 0x66, new byte[] { 0xAA, 0xBB });
			var expected = frame.Data;

			var frameArray = frame.ToByteArray();
			var frameDataField = DataFrameFieldBuilder.GetDataField(frameArray);

			CollectionAssert.AreEqual(expected, frameDataField);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetDataFieldOfInvalidFrameByteArray()
		{
			var frameArray = new byte[] { 0x7D, 0x00, 0x11 };
			var frameDataField = DataFrameFieldBuilder.GetDataField(frameArray);
		}
	}
}