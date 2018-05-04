using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Telemetry.App.Aplication;
using Telemetry.App.Application.Interfaces;
using Telemetry.App.Model;
using Telemetry.Domain.Frame;

namespace Telemetry.App.Test.App
{
	[TestClass]
	public class RecordHandlerTest
	{
		public static RecordHandler RecordHandler { get; set; }

		[ClassInitialize]
		public static void Initialize(TestContext testContext)
		{
			RecordHandler = new RecordHandler();
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetRecordsIndexToScan_RequestedIndexBeforeOfAvaliableRange()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10002);
			var startupParameters = new StartupParameters { EndPoint = endpoint, FirstIndex = 0, LastIndex = 100 };
			var avaliableIndex = new ushort[] { 300, 500 };

			var range = RecordHandler.GetRecordsIndexToScan(startupParameters, avaliableIndex);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetRecordsIndexToScan_StartupParametersIsNull()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10002);
			var startupParameters = new StartupParameters { EndPoint = endpoint, FirstIndex = 0, LastIndex = 100 };
			var avaliableIndex = new ushort[] { 300, 500 };

			var range = RecordHandler.GetRecordsIndexToScan(null, avaliableIndex);
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void GetRecordsIndexToScan_AvaliableIndexIsNull()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10002);
			var startupParameters = new StartupParameters { EndPoint = endpoint, FirstIndex = 0, LastIndex = 100 };
			var avaliableIndex = new ushort[] { 300, 500 };

			var range = RecordHandler.GetRecordsIndexToScan(startupParameters, null);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetRecordsIndexToScan_RequestedIndexAfterOfAvaliableRange()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10002);
			var startupParameters = new StartupParameters { EndPoint = endpoint, FirstIndex = 900, LastIndex = 1000 };
			var avaliableIndex = new ushort[] { 300, 500 };

			var range = RecordHandler.GetRecordsIndexToScan(startupParameters, avaliableIndex);
		}

		[TestMethod]
		public void GetRecordsIndexToScan_AvaliableRangeContainsPartOfRequestedIndex()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10002);
			var startupParameters = new StartupParameters { EndPoint = endpoint, FirstIndex = 400, LastIndex = 1000 };
			var avaliableIndex = new ushort[] { 300, 500 };

			var range = RecordHandler.GetRecordsIndexToScan(startupParameters, avaliableIndex);

			Assert.AreEqual(400, range[0]);
			Assert.AreEqual(500, range[1]);
		}

		[TestMethod]
		public void GetRecordsIndexToScan_RequestedIndexInsideOfAvaliableRange()
		{
			var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10002);
			var startupParameters = new StartupParameters { EndPoint = endpoint, FirstIndex = 420, LastIndex = 470 };
			var avaliableIndex = new ushort[] { 300, 500 };

			var range = RecordHandler.GetRecordsIndexToScan(startupParameters, avaliableIndex);

			Assert.AreEqual(420, range[0]);
			Assert.AreEqual(470, range[1]);
		}

		[TestMethod]
		public void GetRecordsContent_AllRecordIndexAreValid()
		{
			var energyMockResponse = new Energy
			{
				Lenght = 0x05,
				FunctionCode = 0x84,
				Data = new byte[] { 0x7D, 0xE1, 0xBC, 0x59, 0x2B },
				Checksum = 0xD3
			};

			var dateTimeMockResponse = new Telemetry.Domain.Frame.DateTime
			{
				FrameHeader = 0x7D,
				Lenght = 0x05,
				FunctionCode = 0x84,
				Data = new byte[] { 0x7D, 0xE1, 0xBC, 0x59, 0x2B },
				Checksum = 0xD3
			};

			var indexMockResponse = new Index
			{
				Lenght = 0x01,
				FunctionCode = 0x83,
				Data = new byte[] { 0x00 }, //valid record
				Checksum = 0x82
			};

			var connectionHandlerMock = Substitute.For<IConnectionHandler>();
			connectionHandlerMock.TcpSocketClient.TcpClient.Returns(new TcpClient());
			connectionHandlerMock.SendRequest<Energy>(Arg.Any<Energy>()).Returns(Task.FromResult(energyMockResponse));
			connectionHandlerMock.SendRequest<Telemetry.Domain.Frame.DateTime>(Arg.Any<Telemetry.Domain.Frame.DateTime>()).Returns(Task.FromResult(dateTimeMockResponse));
			connectionHandlerMock.SendRequest<Index>(Arg.Any<Index>()).Returns(Task.FromResult(indexMockResponse));

			var records = RecordHandler.GetRecordsContent(connectionHandlerMock, new int[] { 0, 1 });
			Assert.AreEqual(2, records.Count());
		}

		[TestMethod]
		public void GetRecordsContent_InvalidContent_NullDateTime()
		{
			var energyMockResponse = new Energy
			{
				Lenght = 0x05,
				FunctionCode = 0x84,
				Data = new byte[] { 0x7D, 0xE1, 0xBC, 0x59, 0x2B },
				Checksum = 0xD3
			};

			var indexMockResponse = new Index
			{
				Lenght = 0x01,
				FunctionCode = 0x83,
				Data = new byte[] { 0x00 }, //valid record
				Checksum = 0x82
			};

			var connectionHandlerMock = Substitute.For<IConnectionHandler>();
			connectionHandlerMock.TcpSocketClient.TcpClient.Returns(new TcpClient());
			connectionHandlerMock.SendRequest<Energy>(Arg.Any<Energy>()).Returns(Task.FromResult(energyMockResponse));
			connectionHandlerMock.SendRequest<Index>(Arg.Any<Index>()).Returns(Task.FromResult(indexMockResponse));

			var records = RecordHandler.GetRecordsContent(connectionHandlerMock, new int[] { 0, 1 });
			Assert.AreEqual(2, records.Count());
		}

		[TestMethod]
		public void GetRecordsContent_NoValidRecordIndex()
		{
			var indexMockResponse = new Index
			{
				Lenght = 0x01,
				FunctionCode = 0x83,
				Data = new byte[] { 0xFA }, //invalid record
				Checksum = 0x78
			};

			var connectionHandlerMock = Substitute.For<IConnectionHandler>();
			connectionHandlerMock.TcpSocketClient.TcpClient.Returns(new TcpClient());
			connectionHandlerMock.SendRequest<Index>(Arg.Any<Index>()).Returns(Task.FromResult(indexMockResponse));

			var records = RecordHandler.GetRecordsContent(connectionHandlerMock, new int[] { 0, 1 });
			Assert.AreEqual(0, records.Count());
		}
	}
}
