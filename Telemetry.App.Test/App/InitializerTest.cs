using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Net;
using Telemetry.App.Aplication;

namespace Telemetry.App.Test.App
{
	[TestClass]
	public class InitializerTest
	{
		[TestMethod]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		public void InitializeAppWithInvalidFilePath()
		{
			new Initializer().GetRequests(new string[] { @"Z:/dsfsudh/sdiuh.mp3" });
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void InitializeAppWithoutFilePath()
		{
			new Initializer().GetRequests(new string[] { });
		}

		[TestMethod]
		public void GetStartupParametersWithValidData()
		{
			var firstLine = "127.0.0.1 10002 0 47200";
			var secondLine = "192.168.0.3 10000 46000 47288";

			var startupParameters = new Initializer().BuildRequestsList(new string[] { firstLine, secondLine }).ToArray();

			Assert.AreEqual(2, startupParameters.Length);

			Assert.AreEqual(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10002), startupParameters[0].EndPoint);
			Assert.AreEqual(0, startupParameters[0].FirstIndex);
			Assert.AreEqual(47200, startupParameters[0].LastIndex);

			Assert.AreEqual(new IPEndPoint(IPAddress.Parse("192.168.0.3"), 10000), startupParameters[1].EndPoint);
			Assert.AreEqual(46000, startupParameters[1].FirstIndex);
			Assert.AreEqual(47288, startupParameters[1].LastIndex);
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void GetStartupParametersWithInvalidConnectionData()
		{
			var firstLine = "999.0.0.1 10002 0 47200";

			var startupParameters = new Initializer().BuildRequestsList(new string[] { firstLine });
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void GetStartupParametersWithInvalidIndexData()
		{
			var firstLine = "192.0.0.1 10002 0 ABB";

			var startupParameters = new Initializer().BuildRequestsList(new string[] { firstLine});
		}

	}
}