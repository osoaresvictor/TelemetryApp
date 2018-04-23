using System.Collections.Generic;
using Telemetry.App.Model;

namespace Telemetry.App.Application.Interfaces
{
	public interface IInitializer
	{
		IEnumerable<StartupParameters> GetRequests(string[] MainArgs);
	}
}