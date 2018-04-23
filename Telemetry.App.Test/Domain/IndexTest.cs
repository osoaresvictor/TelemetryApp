using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telemetry.Domain.Frame;

namespace Telemetry.App.Test.Domain
{
	[TestClass]
	public class IndexTest
	{
		[TestMethod]
		public void GetAValidIndexRequest()
		{
			var indexRequest = new Index(380);

			var dataExpected = new byte[] { 0x01, 0x7C };

			CollectionAssert.AreEqual(dataExpected, indexRequest.Data);
		}

		[TestMethod]
		public void ValidateIndexResponse()
		{
			var indexResponse = new Index
			{
				Lenght = 0x01,
				FunctionCode = 0x83,
				Data = new byte[] { 0x00 },
				Checksum = 0x82
			};

			Assert.AreEqual(true, indexResponse.IsValid());
		}
	}
}
