using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telemetry.App.Application.Interfaces;
using Telemetry.App.Model;
using Telemetry.App.Repository.Interfaces;
using Telemetry.Domain.Frame;
using Telemetry.Domain.Frame.Builders;

namespace Telemetry.App.Aplication
{
	public class ConnectionHandler : IConnectionHandler
	{
		public ITcpSocketClient TcpSocketClient { get; private set; }

		public ConnectionHandler(ITcpSocketClient TcpSocketClient)
		{
			this.TcpSocketClient = TcpSocketClient;
		}

		public ITcpSocketClient OpenConnection(IPEndPoint endpoint)
		{
			return this.TcpSocketClient.Connect(endpoint);
		}

		public async Task<T> SendRequest<T>(IFrame request) where T : IFrame, new()
		{
			var requestData = request.ToByteArray();
			var response = await this.TcpSocketClient.SendRequest(requestData);

			var responseObject = response.ToFrameObject<T>();

			if (responseObject.Checksum != responseObject.VerifyChecksum())
			{
				Console.WriteLine($"{this.TcpSocketClient.TcpClient.Client.RemoteEndPoint} - Erro na resposta do servidor! Repetindo transmissão... ");

				var errorFrame = new Error().ToByteArray();
				response = await this.TcpSocketClient.SendRequest(errorFrame);
				return response.ToFrameObject<T>();
			}

			return responseObject;
		}

		public void CloseSocketConnection(StartupParameters[] requestsList, int currentIndex)
		{
			var lastIndex = requestsList.Count() - 1;
			if (currentIndex == lastIndex)
			{
				this.TcpSocketClient.Disconnect();
				Console.WriteLine($"{System.DateTime.Now} - Conexão encerrada: {requestsList[currentIndex].EndPoint.ToString()} ");
				return;
			}

			var currentEndpoint = requestsList[currentIndex].EndPoint;
			var nextEndPoint = requestsList[currentIndex + 1].EndPoint;
			if ((currentEndpoint.Address != nextEndPoint.Address) && (currentEndpoint.Port != nextEndPoint.Port))
			{
				this.TcpSocketClient.Disconnect();
				Console.WriteLine($"{System.DateTime.Now} - Conexão encerrada: {requestsList[currentIndex]} ");
			}
		}
	}
}
