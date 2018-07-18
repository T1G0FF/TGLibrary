using System;
using System.Linq;

namespace TGLibrary {
    public static class Maths {
        public static double StandDev(double[] dNumbers, int round = 0) {
            double[] tempNumbers = new double[dNumbers.Length];
            double mean = dNumbers.Average();

            for (int x = 0; x < dNumbers.Length; x++) {
                tempNumbers[x] = Math.Pow((dNumbers[x] - mean), 2D);
            }

            if (round > 0) {
                return Math.Round(Math.Sqrt(tempNumbers.Average()), round, MidpointRounding.AwayFromZero);
            }
            else {
                return Math.Sqrt(tempNumbers.Average());
            }
        }

        public static uint SumUpTo(uint n) {
            return (n * (n + 1)) / 2;
        }
    }
}
