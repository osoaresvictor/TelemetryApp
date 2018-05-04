using System.Net;
using System.Threading.Tasks;
using Telemetry.App.Repository.Interfaces;
using Telemetry.Domain.Frame;

namespace Telemetry.App.Application.Interfaces
{
	public interface IConnectionHandler
	{
		ITcpSocketClient TcpSocketClient { get; }

		ITcpSocketClient OpenConnection(IPEndPoint endpoint);
		Task<T> SendRequest<T>(IFrame request) where T : IFrame, new();
	}
}