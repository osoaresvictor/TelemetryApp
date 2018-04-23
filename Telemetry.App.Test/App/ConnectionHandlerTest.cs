using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using Telemetry.App.Aplication;
using Telemetry.App.Model;
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
		
		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void CloseSocketConnection_NextRequestIsOtherEndPoint()
		{
			var endpoint1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);
			var endpoint2 = new IPEndPoint(IPAddress.Parse("192.168.0.10"), 30004);
			
			var requests = new StartupParameters[2];
			requests[0] = new StartupParameters { EndPoint = endpoint1 };
			requests[1] = new StartupParameters { EndPoint = endpoint2 };

			//An exception will be thrown because no connection was opened to be closed
			ConnectionHandler.CloseSocketConnection(requests, 0);
		}

		[TestMethod]
		public void CloseSocketConnection_NextRequestIsTheSameEndPoint()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);
			
			var requests = new StartupParameters[2];
			requests[0] = new StartupParameters { EndPoint = endpoint };
			requests[1] = new StartupParameters { EndPoint = endpoint };

			ConnectionHandler.CloseSocketConnection(requests, 0);
			//If success, connection should not be closed and no exception will be thrown
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void CloseSocketConnection_AtLastRequest()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);

			var requests = new StartupParameters[1];
			requests[0] = new StartupParameters { EndPoint = endpoint };

			//An exception will be thrown because no connection was opened to be closed
			ConnectionHandler.CloseSocketConnection(requests, 0);
		}
	}
}