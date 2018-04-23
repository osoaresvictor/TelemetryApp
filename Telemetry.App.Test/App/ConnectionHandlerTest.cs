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
		[TestMethod]
		[ExpectedException(typeof(System.FormatException))]
		public void OpenConnectionWithInvalidData()
		{
			var tcpSocketClient = new TcpSocketClient();
			var connectionHandler = new ConnectionHandler(tcpSocketClient);
			var endpoint = new IPEndPoint(IPAddress.Parse("999.888.85.141"), 999999999);

			connectionHandler.OpenConnection(endpoint);

			Assert.AreEqual(true, connectionHandler.TcpSocketClient.TcpClient.Connected);

			connectionHandler.TcpSocketClient.Disconnect();
		}
		
		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void CloseSocketConnection_NextRequestIsOtherEndPoint()
		{
			var endpoint1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);
			var endpoint2 = new IPEndPoint(IPAddress.Parse("192.168.0.10"), 30004);
			var connectionHandler = new ConnectionHandler(new TcpSocketClient());

			var requests = new StartupParameters[2];
			requests[0] = new StartupParameters { EndPoint = endpoint1 };
			requests[1] = new StartupParameters { EndPoint = endpoint2 };

			//An exception will be thrown because no connection was opened to be closed
			connectionHandler.CloseSocketConnection(requests, 0);
		}

		[TestMethod]
		public void CloseSocketConnection_NextRequestIsTheSameEndPoint()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);
			var connectionHandler = new ConnectionHandler(new TcpSocketClient());

			var requests = new StartupParameters[2];
			requests[0] = new StartupParameters { EndPoint = endpoint };
			requests[1] = new StartupParameters { EndPoint = endpoint };

			connectionHandler.CloseSocketConnection(requests, 0);
			//If success, connection should not be closed and no exception will be thrown
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void CloseSocketConnection_AtLastRequest()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);
			var connectionHandler = new ConnectionHandler(new TcpSocketClient());

			var requests = new StartupParameters[1];
			requests[0] = new StartupParameters { EndPoint = endpoint };

			//An exception will be thrown because no connection was opened to be closed
			connectionHandler.CloseSocketConnection(requests, 0);
		}
	}
}