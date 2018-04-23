using System;
using Telemetry.App.Utils.Interfaces;

namespace Telemetry.App.Utils
{
	public class FloatRounder : IFloatRounder
	{
		public float RoundWithHalfToPair(float numberToRound)
		{
			var numberWithThreeDecimalDigitRound = Math.Round(numberToRound, 3);

			var integerDigits = Math.Truncate(numberWithThreeDecimalDigitRound);
			var threeDigitsOfDecimalPart = Math.Round((numberWithThreeDecimalDigitRound - integerDigits) * 1000);

			var unit = (threeDigitsOfDecimalPart % 10);

			if (unit >= 5)
			{
				var howFarFrom10 = 10 - unit;
				var roundedUnit = (threeDigitsOfDecimalPart + howFarFrom10) / 10;

				if (this.IsPair(roundedUnit) == false)
				{
					roundedUnit--;
					roundedUnit = roundedUnit / 100;

					return (float)(integerDigits + roundedUnit);
				}
				else
				{
					return (float)(integerDigits + (roundedUnit / 100));
				}
			}
			else
			{
				return (float)Math.Round(numberToRound, 2);
			}
		}

		private bool IsPair(double roundedUnit)
		{
			return roundedUnit % 2 == 0;
		}
	}
}
