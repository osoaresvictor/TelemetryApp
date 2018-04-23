using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Telemetry.App.Application.Interfaces;
using Telemetry.App.Model;

namespace Telemetry.App.Aplication
{
	public class Initializer : IInitializer
	{
		public IEnumerable<StartupParameters> GetRequests(string[] mainArgs)
		{
			if (mainArgs.Length == 0) throw new Exception("Nenhum arquivo informado para inicialização!");

			var filePath = mainArgs[0];

			var requests = File.ReadAllLines(filePath);

			return this.BuildRequestsList(requests);
		}

		public IEnumerable<StartupParameters> BuildRequestsList(string[] requests)
		{
			var startupParameters = new List<StartupParameters>();

			foreach (var line in requests)
			{
				var parameters = line.Split(' ');

				var ip = parameters[0];
				var port = int.Parse(parameters[1]);
				var startIndex = int.Parse(parameters[2]);
				var endIndex = int.Parse(parameters[3]);

				if (startIndex >= endIndex) throw new Exception("Erro no range de indices informado na lista de pedidos!");

				startupParameters.Add(
					new StartupParameters
					{
						EndPoint = new IPEndPoint(IPAddress.Parse(ip), port),
						FirstIndex = startIndex,
						LastIndex = endIndex
					}
				);
			}

			return startupParameters;
		}
	}
}
