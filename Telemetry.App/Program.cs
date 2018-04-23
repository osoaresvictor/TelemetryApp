using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using Telemetry.App.Application.Interfaces;
using Telemetry.App.IoC;
using Telemetry.App.Model;
using Telemetry.Domain;
using Telemetry.Domain.Frame;

namespace TelemetryApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var iocContainer = new ContainerFactory().Create();
			var initializer = iocContainer.Resolve<IInitializer>();
			var recordHandler = iocContainer.Resolve<IRecordHandler>();
			var logWritter = iocContainer.Resolve<ILogWritter>();
			var connectionHandler = iocContainer.Resolve<IConnectionHandler>();

			var requestsList = default(StartupParameters[]);
			var recordsContent = default(IEnumerable<RecordContent>);

			try { requestsList = initializer.GetRequests(args).ToArray(); }
			catch (Exception ex)
			{
				Console.WriteLine($"{System.DateTime.Now} Erro: {ex.Message} \n {ex.StackTrace} \n\nPressione qualquer tecla para sair...");
				Console.ReadKey();
				Environment.Exit(0);
			}

			for (var requestIndex = 0; requestIndex < requestsList.Length; requestIndex++)
			{
				try
				{
					if (connectionHandler.TcpSocketClient.TcpClient == null) connectionHandler.OpenConnection(requestsList[requestIndex].EndPoint);

					Console.WriteLine($"\nIniciando Requisição {requestIndex + 1} de {requestsList.Length}");

					var serialNumber = String.Join("", connectionHandler.SendRequest<SerialNumber>(new SerialNumber()).Result.GetAsciiCharacters());
					var recordsStatus = connectionHandler.SendRequest<Status>(new Status()).Result.GetValue();
					var indexRangeToScan = recordHandler.GetRecordsIndexToScan(requestsList[requestIndex], recordsStatus);

					recordsContent = recordHandler.GetRecordsContent(connectionHandler, indexRangeToScan);

					connectionHandler.CloseSocketConnection(requestsList, requestIndex);

					logWritter.SaveCSVFile(serialNumber, recordsContent);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"{System.DateTime.Now} - Erro: {ex.Message} \n {ex.StackTrace}");
					continue;
				}
			}

			Console.WriteLine($"{System.DateTime.Now} - Execução Finalizada! Pressione qualquer tecla para sair...");
			Console.ReadKey();
		}
	}
}