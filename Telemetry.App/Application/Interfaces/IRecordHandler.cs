using System.Collections.Generic;
using Telemetry.App.Model;
using Telemetry.Domain;

namespace Telemetry.App.Application.Interfaces
{
	public interface IRecordHandler
	{
		int[] GetRecordsIndexToScan(StartupParameters request, ushort[] recordsStatus);
		IEnumerable<RecordContent> GetRecordsContent(IConnectionHandler connectionHandler, int[] indexRange);
	}
}