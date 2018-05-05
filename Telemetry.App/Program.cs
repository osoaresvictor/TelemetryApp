using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Telemetry.App.Application.Interfaces;
using Telemetry.App.IoC;
using Telemetry.App.Model;
using Telemetry.App.Utils.Interfaces;
using Telemetry.Domain.Frame;

namespace TelemetryApp
{
	public class Program
	{
		private static IContainer IocContainer;
		private static StartupParameters[] RequestsList;
		private static List<Socket> ActiveConnections;

		public static void Main(string[] args)
		{
			try
			{
				IocContainer = new ContainerFactory().Create();
				var initializer = IocContainer.Resolve<IInitializer>();

				ActiveConnections = new List<Socket>();
				RequestsList = default(StartupParameters[]);

				RequestsList = initializer.GetRequests(args).ToArray();

				var requestTasks = new List<Task>();
				Parallel.ForEach(RequestsList, request => { requestTasks.Add(ProcessRequest(request)); });
				Task.WaitAll(requestTasks.ToArray());
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{System.DateTime.Now} - Erro: {ex.Message} \n {ex.StackTrace}");
			}
			finally
			{
				ActiveConnections.ForEach(connection => connection.Dispose());
				Console.WriteLine($"\n{System.DateTime.Now} - Execução Finalizada! Pressione qualquer tecla para sair...");
				Console.ReadKey();
			}
		}

		private static async Task ProcessRequest(StartupParameters request)
		{
			var recordHandler = IocContainer.Resolve<IRecordHandler>();
			var logWritter = IocContainer.Resolve<ILogWritter>();
			var connectionHandler = default(IConnectionHandler);

			if (ActiveConnections.FirstOrDefault(connection => connection.RemoteEndPoint == request.EndPoint) == null)
			{
				connectionHandler = IocContainer.Resolve<IConnectionHandler>();
				ActiveConnections.Add(connectionHandler.OpenConnection(request.EndPoint).TcpClient.Client);
			}

			var serialNumberFrame = await connectionHandler.SendRequest<SerialNumber>(new SerialNumber());
			var serialNumber = String.Join("", serialNumberFrame.GetAsciiCharacters());

			var recordsStatusFrame = await connectionHandler.SendRequest<Status>(new Status());
			var recordsStatus = recordsStatusFrame.GetValue();
			Console.WriteLine($"{request.EndPoint.ToString()} Registros Disponíveis: {recordsStatus[1] - recordsStatus[0]} [{recordsStatus[0]},{recordsStatus[1]}]");

			var indexRangeToScan = recordHandler.GetRecordsIndexToScan(request, recordsStatus);
			Console.WriteLine($"{request.EndPoint.ToString()} Registros Selecionados: {indexRangeToScan[1] - indexRangeToScan[0]} [{indexRangeToScan[0]},{indexRangeToScan[1]}]");

			var recordsContent = recordHandler.GetRecordsContent(connectionHandler, indexRangeToScan);

			logWritter.SaveCSVFile(serialNumber, recordsContent);
		}
	}
}