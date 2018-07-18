using System;

namespace TGLibrary {
    /// <remarks>
    /// Various temperature scale conversion methods.
    /// </remarks>
    public class Temperature {
        /// <summary>
        /// Converts a given temperature from degrees Celsius to degrees Fahrenheit.
        /// </summary>
        /// <param name="celsius">The temperature in degrees Celsius to convert.</param>
        /// <returns>The temperature in degrees Fahrenheit equivalent to the given temperature.</returns>
        public static double CelsiusToFahrenheit(double celsius) {
            return ((celsius * 9) / 5) + 32;
        }

        /// <summary>
        /// Converts a given temperature from degrees Celsius to degrees Kelvin.
        /// </summary>
        /// <param name="celsius">The temperature in degrees Celsius to convert.</param>
        /// <returns>The temperature in degrees Kelvin equivalent to the given temperature.</returns>
        public static double CelsiusToKelvin(double celsius) {
            return celsius + 273.15D;
        }

        /// <summary>
        /// Converts a given temperature from degrees Fahrenheit to degrees Celsius.
        /// </summary>
        /// <param name="fahrenheit">The temperature in degrees Fahrenheit to convert.</param>
        /// <returns>The temperature in degrees Celsius equivalent to the given temperature.</returns>
        public static double FahrenheitToCelsius(double fahrenheit) {
            return ((fahrenheit - 32) * 5) / 9;
        }

        /// <summary>
        /// Converts a given temperature from degrees Fahrenheit to degrees Kelvin.
        /// </summary>
        /// <param name="fahrenheit">The temperature in degrees Fahrenheit to convert.</param>
        /// <returns>The temperature in degrees Kelvin equivalent to the given temperature.</returns>
        public static double FahrenheitToKelvin(double fahrenheit) {
            return CelsiusToKelvin(FahrenheitToCelsius(fahrenheit));
        }

        /// <summary>
        /// Converts a given temperature from degrees Kelvin to degrees Celsius.
        /// </summary>
        /// <param name="kelvin">The temperature in degrees Kelvin to convert.</param>
        /// <returns>The temperature in degrees Celsius equivalent to the given temperature.</returns>
        public static double KelvinToCelsius(double kelvin) {
            return kelvin - 273.15D;
        }

        /// <summary>
        /// Converts a given temperature from degrees Kelvin to degrees Fahrenheit.
        /// </summary>
        /// <param name="kelvin">The temperature in degrees Kelvin to convert.</param>
        /// <returns>The temperature in degrees Fahrenheit equivalent to the given temperature.</returns>
        public static double KelvinToFahrenheit(double kelvin) {
            return CelsiusToFahrenheit(KelvinToCelsius(kelvin));
        }
    }
}
