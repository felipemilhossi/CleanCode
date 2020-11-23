using NUnit.Framework;
using System;

namespace CleanCode.SimplifyingConditional
{
    class Program
    {
        private const int OvernightStart = 21;
        private const int OvernightEnd = 7;
        private const double OvernightFare = 4;
        private const double SaturdayFare = 2;
        private const double SundayFare = 3;
        private const double MondayFare = 0.5;
        private const double NormalFare = 1;
        private const double AdditionalFarePerKilometers = 0.75;
        private const int AdditionalFareKilometersStart = 50;

        static void Main(string[] args)
        {
            var result = CalculateFreight(10.5, DateTime.Now);

            Console.WriteLine($"Result: R$ {result:F}");
            Console.ReadKey();
        }

        public static decimal CalculateFreight(double kilometers, DateTime date)
        {
            if (!IsValidKilometers(kilometers))
                throw new ArgumentException($"Kilometers must be greater than zero");

            if (!IsValidDate(date))
                throw new ArgumentException($"{date} is not a valid date time");

            decimal additionalFare = 0;

            if (HasAdditionalFare(kilometers))
                additionalFare = (decimal)((kilometers - AdditionalFareKilometersStart) * AdditionalFarePerKilometers);

            if (IsOvernight(date))
                return (decimal)(kilometers * OvernightFare) + additionalFare;

            if (IsSaturday(date))
                return (decimal)(kilometers * SaturdayFare) + additionalFare; ;

            if (IsSunday(date))
                return (decimal)(kilometers * SundayFare) + additionalFare; ;

            if (IsMonday(date))
                return (decimal)(kilometers * MondayFare) + additionalFare; ;

            return (decimal)(kilometers * NormalFare) + additionalFare; ;
        }

        private static bool IsValidKilometers(double kilometers) => kilometers > 0;

        private static bool IsValidDate(DateTime date) => date > DateTime.MinValue;

        private static bool HasAdditionalFare(double kilometers) => kilometers > AdditionalFareKilometersStart;

        private static bool IsOvernight(DateTime date) => date.Hour > OvernightStart || date.Hour < OvernightEnd;

        private static bool IsSaturday(DateTime date) => date.DayOfWeek == DayOfWeek.Saturday;

        private static bool IsSunday(DateTime date) => date.DayOfWeek == DayOfWeek.Sunday;

        private static bool IsMonday(DateTime date) => date.DayOfWeek == DayOfWeek.Monday;

        [TestFixture]
        public class Tests
        {
            [Test]
            public void Freight_NormalDay()
            {
                var freightAmount = CalculateFreight(10, new DateTime(2020, 11, 18, 15, 0, 0));
                Assert.AreEqual(10, freightAmount);
            }

            [Test]
            public void Freight_Sunday()
            {
                var freightAmount = CalculateFreight(10, new DateTime(2020, 11, 22, 15, 0, 0));
                Assert.AreEqual(30, freightAmount);
            }

            [Test]
            public void Freight_Saturday()
            {
                var freightAmount = CalculateFreight(10, new DateTime(2020, 11, 21, 15, 0, 0));
                Assert.AreEqual(20, freightAmount);
            }

            [Test]
            public void Freight_Monday()
            {
                var freightAmount = CalculateFreight(10, new DateTime(2020, 11, 23, 15, 0, 0));
                Assert.AreEqual(5, freightAmount);
            }

            [Test]
            public void Freight_Overnight()
            {
                var freightAmount = CalculateFreight(10, new DateTime(2020, 11, 18, 22, 0, 0));
                Assert.AreEqual(40, freightAmount);
            }

            [Test]
            public void Freight_NormalDayWithAdditionalByDistance()
            {
                var freightAmount = CalculateFreight(60, new DateTime(2020, 11, 18, 15, 0, 0));
                Assert.AreEqual(67.5, freightAmount);
            }

            [Test]
            public void Freight_InvalidDate()
            {
                Assert.Throws<ArgumentException>(() => CalculateFreight(10, DateTime.MinValue));
            }

            [Test]
            public void Freight_InvalidDistance()
            {
                Assert.Throws<ArgumentException>(() => CalculateFreight(-10, new DateTime(2020, 11, 18, 15, 0, 0)));
            }
        }
    }
}
