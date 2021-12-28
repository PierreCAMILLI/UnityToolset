namespace Toolset
{
    [System.Serializable]
    public struct Temperature : System.IComparable, System.IComparable<Temperature>, System.IEquatable<Temperature>
    {
        [UnityEngine.SerializeField]
        private double _kelvin;

        public const double AbsoluteZeroCelsius = -273.15;
        public const double AbsoluteZeroFahrenheit = -459.67;

        public static readonly Temperature ZeroKelvin = Temperature.FromKelvin(0.0);
        public static readonly Temperature ZeroCelsius = Temperature.FromCelsius(0.0);
        public static readonly Temperature ZeroFahrenheit = Temperature.FromFahrenheit(0.0);

        public double Kelvin
        {
            get => _kelvin;
            set
            {
                if (value >= 0.0)
                {
                    _kelvin = value;
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException(string.Format($"{0} is less than absolute zero.", value));
                }
            }
        }

        public double Celsius
        {
            get => KelvinToCelsius(_kelvin);
            set => Kelvin = CelsiusToKelvin(value);
        }

        public double Fahrenheit
        {
            get => KelvinToFahrenheit(_kelvin);
            set => Kelvin = FahrenheitToKelvin(value);
        }

        public static Temperature FromKelvin(double kelvin)
        {
            Temperature t = new Temperature();
            t.Kelvin = kelvin;
            return t;
        }

        public static Temperature FromCelsius(double celsius)
        {
            Temperature t = new Temperature();
            t.Celsius = celsius;
            return t;
        }

        public static Temperature FromFahrenheit(double fahrenheit)
        {
            Temperature t = new Temperature();
            t.Fahrenheit = fahrenheit;
            return t;
        }

        public static double KelvinToCelsius(double kelvin)
        {
            return kelvin + AbsoluteZeroCelsius;
        }

        public static double CelsiusToKelvin(double celsius)
        {
            return celsius - AbsoluteZeroCelsius;
        }

        public static double KelvinToFahrenheit(double kelvin)
        {
            return (kelvin * 1.8) + AbsoluteZeroFahrenheit;
        }

        public static double FahrenheitToKelvin(double fahrenheit)
        {
            return (fahrenheit - AbsoluteZeroFahrenheit) / 1.8;
        }

        public static double CelsiusToFahrenheit(double celsius)
        {
            return (celsius * 1.8) + 32.0;
        }

        public static double FahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32.0) / 1.8;
        }

        public static implicit operator double(Temperature temperature) => temperature._kelvin;
        public static implicit operator Temperature(double temperature) => FromKelvin(temperature);

        public int CompareTo(object obj)
        {
            if (obj is Temperature)
            {
                return _kelvin.CompareTo((double)obj);
            }
            return _kelvin.CompareTo(obj);
        }

        public int CompareTo(Temperature other)
        {
            return _kelvin.CompareTo(other._kelvin);
        }

        public bool Equals(Temperature other)
        {
            return _kelvin.Equals(other._kelvin);
        }
    }
}
