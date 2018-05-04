using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Telemetry.App.Aplication;
using Telemetry.App.Repository;

namespace Telemetry.App.Test.App
{
	[TestClass]
	public class ConnectionHandlerTest
	{
		public static ConnectionHandler ConnectionHandler { get; set; }

		[ClassInitialize]
		public static void Initialize(TestContext testContext)
		{
			ConnectionHandler = new ConnectionHandler(new TcpSocketClient());
		}

		[TestMethod]
		[ExpectedException(typeof(System.FormatException))]
		public void OpenConnectionWithInvalidData()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("999.888.85.141"), 999999999);

			ConnectionHandler.OpenConnection(endpoint);

			Assert.AreEqual(true, ConnectionHandler.TcpSocketClient.TcpClient.Connected);

			ConnectionHandler.TcpSocketClient.Disconnect();
		}
	}
}