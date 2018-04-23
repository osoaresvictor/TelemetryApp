using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Telemetry.Domain.Frame;

namespace Telemetry.App.Test.Domain
{
	[TestClass]
	public class EnergyTest
	{
		[TestMethod]
		public void GetDefaultEnergyFrameRequest()
		{
			var energy = new Energy();

			Assert.AreEqual(0x7D, energy.FrameHeader);
			Assert.AreEqual(0x00, energy.Lenght);
			Assert.AreEqual(0x05, energy.FunctionCode);
			Assert.AreEqual(null, energy.Data);
			Assert.AreEqual(0x05, (byte)energy.Checksum);
		}
		
		[TestMethod]
		public void GetEnergyValueFromValidEnergyFrame()
		{
			var energyFrame = new Energy
			{
				FrameHeader = 0x7D,
				Lenght = 0x04,
				FunctionCode = 0x85,
				Data = new byte[] { 0x41, 0x20, 0x00, 0x00 },
				Checksum = 0xE0
			};

			var expected = 10.0F;

			Assert.AreEqual(expected, energyFrame.GetEnergyValue());
		}

		[TestMethod]
		public void GetEnergyValueFromInvalidEnergyFrame()
		{
			var energyFrame = new Energy
			{
				FrameHeader = 0x7D,
				Lenght = 0x06,
				FunctionCode = 0x85,
				Data = new byte[] { 0xFF, 0x41, 0x20, 0x00, 0x00 },
				Checksum = 0xE0
			};
			
			Assert.ThrowsException<Exception>(() => energyFrame.GetEnergyValue());
		}
	}
}
