using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Telemetry.App.Repository.Interfaces
{
	public interface ITcpSocketClient
	{
		TcpClient TcpClient { get; }

		ITcpSocketClient Connect(IPEndPoint endpoint);
		Task<byte[]> SendRequest(byte[] requestData);
		void Disconnect();
	}
}