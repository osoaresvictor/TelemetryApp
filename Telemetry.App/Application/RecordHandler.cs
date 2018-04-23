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
		public int[] GetRecordsIndexToScan(StartupParameters request, ushort[] recordsStatus)
		{
			var avaliableRecordsRange = Enumerable.Range(recordsStatus[0], recordsStatus[1]);

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
			else
			{
				throw new Exception("O Intervalo informado está fora dos limites disponíveis para leitura!");
			}
		}

		public IEnumerable<RecordContent> GetRecordsContent(IConnectionHandler connectionHandler, int[] indexRange)
		{
			Console.WriteLine($"{System.DateTime.Now} - Iniciando processamento, índices: {indexRange[0]}-{indexRange[1]} ");

			var recordsContent = new List<RecordContent>();

			for (var index = indexRange[0]; index <= indexRange[1]; index++)
			{
				Console.WriteLine($"   > Processando registro {index} de {indexRange[1]}");
				var content = GetRecordContent(connectionHandler, index).Result;

				if (content == null) continue;
				else recordsContent.Add(content);
			}

			return recordsContent;
		}

		private async Task<RecordContent> GetRecordContent(IConnectionHandler connectionHandler, int index)
		{
			var recordIndex = connectionHandler.SendRequest<Index>(new Index(index)).Result;

			if (recordIndex.IsValid())
			{
				var dateTime = await connectionHandler.SendRequest<Domain.Frame.DateTime>(new Domain.Frame.DateTime());
				var energy = await connectionHandler.SendRequest<Energy>(new Energy());

				return new RecordContent
				{
					Index = index,
					DateTime = dateTime,
					Energy = energy
				};
			}
			else
			{
				return null;
			}
		}
	}
}
