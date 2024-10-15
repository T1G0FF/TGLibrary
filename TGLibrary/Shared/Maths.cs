using System;
using System.Linq;

namespace TGLibrary {
	public static class MathTG {
		public static double StandDev(double[] dNumbers, int round = -1, MidpointRounding rounding = MidpointRounding.AwayFromZero) {
			double result = 0;
			double[] tempNumbers = new double[dNumbers.Length];
			double mean = dNumbers.Average();

			for (int x = 0; x < dNumbers.Length; x++) {
				tempNumbers[x] = Math.Pow((dNumbers[x] - mean), 2D);
			}

			result = Math.Sqrt(tempNumbers.Average());
			if (round > -1) {
				result = Math.Round(result, round, rounding);
			}

			return result;
		}

		public static uint SumUpTo(uint n) {
			return (n * (n + 1)) / 2;
		}

		public static bool IsCloseCompare(double a, double b, double threshold) {
			return a >= b - threshold && a <= b + threshold;
		}

		public static int CloseCompare(double a, double b, double threshold) {
			int result = 0;
			if (a >= b - threshold && a <= b + threshold) {
				result = 0;
			}
			else if (a > b + threshold) {
				result = 1;
			}
			else if (a < b - threshold) {
				result = -1;
			}
			return result;
		}

		public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T> {
			if (value.CompareTo(min) < 0)
				return min;
			else if (value.CompareTo(max) > 0)
				return max;
			else
				return value;
		}
	}
}
