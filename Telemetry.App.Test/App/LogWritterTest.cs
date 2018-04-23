using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Telemetry.App.Aplication;
using Telemetry.Domain;

namespace Telemetry.App.Test.App
{
	[TestClass]
	public class LogWritterTest
	{
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void SaveFileWithNoSerialNumber()
		{
			new LogWritter().SaveCSVFile("", new List<RecordContent>() { new RecordContent { } });
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void SaveFileWithInvalidRecordContentList()
		{
			new LogWritter().SaveCSVFile("ASUHDIFU023341", new List<RecordContent>());
		}

		[TestMethod]
		public void SaveFileWithValidInputData()
		{
			var defaultLogFilePath = Directory.GetCurrentDirectory() + $"\\LOG_{ System.DateTime.Now.ToString()}.csv".Replace(':', '-')
																					.Replace(' ', '_')
																					.Replace('/', '-');

			var pathGeneratedFile = new LogWritter().SaveCSVFile("ASUHDIFU023341", new List<RecordContent>() { null, null, null});

			File.Delete(pathGeneratedFile);

			Assert.AreEqual(defaultLogFilePath.Length, pathGeneratedFile.Length);
		}

	}
}