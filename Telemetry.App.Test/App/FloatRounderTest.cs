using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telemetry.App.Utils;

namespace Telemetry.App.Test.App
{
	[TestClass]
    public class FloatRounderTest
    {
		public static FloatRounder FloatRounder { get; set; }

		[ClassInitialize]
		public static void Initialize(TestContext testContext)
		{
			FloatRounder = new FloatRounder();
		}

		[TestMethod]
		public void CheckIfRoundingsAreCorrect()
		{
			Assert.AreEqual(1.01F, FloatRounder.RoundWithHalfToPair(1.014F));
			Assert.AreEqual(1.02F, FloatRounder.RoundWithHalfToPair(1.015F));
			Assert.AreEqual(1.02F, FloatRounder.RoundWithHalfToPair(1.016F));
			Assert.AreEqual(1.02F, FloatRounder.RoundWithHalfToPair(1.025F));
		}
	}
}
