using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Telemetry.App.Utils;
using Telemetry.Domain;

namespace Telemetry.App.Test.App
{
	[TestClass]
	public class LogWritterTest
	{
		public static LogWritter LogWritter { get; set; }

		[ClassInitialize]
		public static void Initialize(TestContext testContext)
		{
			LogWritter = new LogWritter(new FloatRounder());
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void SaveFileWithNoSerialNumber()
		{
			LogWritter.SaveCSVFile("", new List<RecordContent>() { new RecordContent { } });
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void SaveFileWithInvalidRecordContentList()
		{
			LogWritter.SaveCSVFile("ASUHDIFU023341", new List<RecordContent>());
		}

		[TestMethod]
		public void SaveFileWithValidInputData()
		{
			var defaultLogFilePath = Directory.GetCurrentDirectory() + $"\\LOG_{ System.DateTime.Now.ToString()}.csv".Replace(':', '-')
																					.Replace(' ', '_')
																					.Replace('/', '-');

			var pathGeneratedFile = LogWritter.SaveCSVFile("ASUHDIFU023341", new List<RecordContent>() { null, null, null});

			File.Delete(pathGeneratedFile);

			Assert.AreEqual(defaultLogFilePath.Length, pathGeneratedFile.Length);
		}
	}
}