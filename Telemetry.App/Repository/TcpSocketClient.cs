using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Telemetry.App.Repository.Interfaces;

namespace Telemetry.App.Repository
{
	public class TcpSocketClient : ITcpSocketClient
	{
		//Frame Header (1 Byte) + Lenght (1 Byte) + Function Code (1 Byte) +  Data (until 255 Bytes) + Checksum (1 Byte) = 259 Bytes
		private const int BUFFER_SIZE = 259;

		public TcpClient TcpClient { get; private set; }

		public ITcpSocketClient Connect(IPEndPoint endpoint)
		{
			try
			{
				var address = endpoint.Address.ToString();
				var port = endpoint.Port;

				this.TcpClient = new TcpClient(address, port);

				Console.WriteLine($"\n{System.DateTime.Now.ToString()} - Socket conectado: {address}:{port} ");

				return this;
			}
			catch (ArgumentNullException)
			{
				throw new Exception("O endereço IP e PORTA não podem ser nulos!");
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new Exception("O número da PORTA não é válido!");
			}
			catch (SocketException ex)
			{
				throw new Exception($"Erro de conexão: {ex.Message} \n {ex.StackTrace}");
			}
			catch (Exception ex)
			{
				throw new Exception($"{ex.Message} \n {ex.StackTrace}");
			}
		}

		public async Task<byte[]> SendRequest(byte[] requestData)
		{
			var data = requestData;
			var stream = this.TcpClient.GetStream();

			await stream.WriteAsync(data, 0, data.Length);
			data = new byte[BUFFER_SIZE];
			var bytesQuantity = await stream.ReadAsync(data, 0, data.Length);

			return data.Take(bytesQuantity).ToArray();
		}

		public void Disconnect()
		{
			this.TcpClient.Dispose();
		}
	}
}
