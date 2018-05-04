using System;
using System.Net;
using System.Threading.Tasks;
using Telemetry.App.Application.Interfaces;
using Telemetry.App.Repository.Interfaces;
using Telemetry.Domain.Frame;
using Telemetry.Domain.Frame.Builders;

namespace Telemetry.App.Aplication
{
	public class ConnectionHandler : IConnectionHandler
	{
		public ITcpSocketClient TcpSocketClient { get;  private set; }

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
	}
}
