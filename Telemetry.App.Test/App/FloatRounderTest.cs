using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telemetry.App.Utils;

namespace Telemetry.App.Test.App
{
	[TestClass]
    public class FloatRounderTest
    {
		[TestMethod]
		public void CheckIfRoundingsAreCorrect()
		{
			var rounder = new FloatRounder();

			Assert.AreEqual(1.01F, rounder.RoundWithHalfToPair(1.014F));
			Assert.AreEqual(1.02F, rounder.RoundWithHalfToPair(1.015F));
			Assert.AreEqual(1.02F, rounder.RoundWithHalfToPair(1.016F));
			Assert.AreEqual(1.02F, rounder.RoundWithHalfToPair(1.025F));
		}
	}
}
