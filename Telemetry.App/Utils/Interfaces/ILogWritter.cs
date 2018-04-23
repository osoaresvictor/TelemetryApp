using System.Collections.Generic;
using Telemetry.Domain;

namespace Telemetry.App.Utils.Interfaces
{
	public interface ILogWritter
	{
		string SaveCSVFile(string serialNumber, IEnumerable<RecordContent> recordsContent);
	}
}