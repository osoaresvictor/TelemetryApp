using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telemetry.App.Application.Interfaces;
using Telemetry.App.Model;
using Telemetry.Domain;
using Telemetry.Domain.Frame;

namespace Telemetry.App.Aplication
{
	public class RecordHandler : IRecordHandler
	{
		private List<RecordContent> RecordsContent;

		public int[] GetRecordsIndexToScan(StartupParameters request, ushort[] recordsStatus)
		{
			if (request == null || recordsStatus.Length < 1) { throw new Exception("Invalid arguments for 'GetRecordsIndexToScan' method"); }

			var avaliableRecordsRange = Enumerable.Range(recordsStatus[0], (recordsStatus[1] - recordsStatus[0] + 1));

			var firstIndexRequestExists = avaliableRecordsRange.Contains(request.FirstIndex);
			var lastIndexRequestExists = avaliableRecordsRange.Contains(request.LastIndex);

			if (firstIndexRequestExists && lastIndexRequestExists)
			{
				return new int[] { request.FirstIndex, request.LastIndex };
			}
			else if (firstIndexRequestExists == false && lastIndexRequestExists)
			{
				return new int[] { recordsStatus[0], request.LastIndex };
			}
			else if (firstIndexRequestExists && lastIndexRequestExists == false)
			{
				return new int[] { request.FirstIndex, recordsStatus[1] };
			}
			else if (request.FirstIndex < recordsStatus[0] && request.LastIndex > recordsStatus[1])
			{
				return new int[] { recordsStatus[0], recordsStatus[1] };
			}
			else
			{
				throw new Exception("O Intervalo informado está fora dos limites disponíveis para leitura!");
			}
		}

		public IEnumerable<RecordContent> GetRecordsContent(IConnectionHandler connectionHandler, int[] indexRange)
		{
			if (connectionHandler.TcpSocketClient?.TcpClient == null) throw new ArgumentNullException(nameof(connectionHandler));

			Console.WriteLine($"{connectionHandler.TcpSocketClient.TcpClient.Client.RemoteEndPoint} - " +
							  $"Iniciando processamento, índices: {indexRange[0]}-{indexRange[1]} ");

			this.RecordsContent = new List<RecordContent>();

			for (var index = indexRange[0]; index <= indexRange[1]; index++)
			{
				Console.WriteLine($"{connectionHandler.TcpSocketClient.TcpClient.Client.RemoteEndPoint} - Processando registro {index} de {indexRange[1]}");
				GetRecordContent(connectionHandler, index).Wait();
			};

			return this.RecordsContent;
		}

		private async Task GetRecordContent(IConnectionHandler connectionHandler, int index)
		{
			var recordIndex = await connectionHandler.SendRequest<Index>(new Index(index));

			if (recordIndex.IsValid())
			{
				var dateTime = await connectionHandler.SendRequest<Domain.Frame.DateTime>(new Domain.Frame.DateTime());
				var energy = await connectionHandler.SendRequest<Energy>(new Energy());

				this.RecordsContent.Add(
					new RecordContent
					{
						Index = index,
						DateTime = dateTime,
						Energy = energy
					});
			}
			else return;
		}
	}
}
