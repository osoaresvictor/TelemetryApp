using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telemetry.App.Application.Interfaces;
using Telemetry.Domain;

namespace Telemetry.App.Aplication
{
	public class LogWritter : ILogWritter
	{
		public string SaveCSVFile(string serialNumber, IEnumerable<RecordContent> recordsContent)
		{
			if (String.IsNullOrEmpty(serialNumber) || (recordsContent == null || recordsContent.Count() == 0))
			{
				throw new Exception("Numero Serial nulo ou não há registros para serem gravados em arquivo!");
			}

			var fileName = $"\\LOG_{ System.DateTime.Now.ToString()}.csv".Replace(':', '-')
																			.Replace(' ', '_')
																			.Replace('/', '-');

			var projectPath = Directory.GetCurrentDirectory() + $"{fileName}";

			Console.WriteLine($"{System.DateTime.Now} - Registrando em arquivo: {projectPath} ");

			var lines = new List<string>();
			lines.Add(serialNumber);

			foreach (var record in recordsContent)
			{
				try
				{
					var formattedDateTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", record.DateTime.GetDateTime());
					var Status = Math.Round(record.Energy.GetEnergyValue(), 2, MidpointRounding.ToEven);

					lines.Add($"{record.Index};{formattedDateTime};{Status}");
				}
				catch
				{
					continue;
				}
			}

			File.WriteAllLines(projectPath, lines);

			return projectPath;
		}
	}
}
