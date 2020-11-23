using NUnit.Framework;
using System;

namespace CleanCode.ReplaceMagicNumber
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
            //Verifica se a distancia informada é válida
            if (kilometers > 0)
            {
                //Verifica se a data informada é válida
                if (date > DateTime.MinValue)
                {
                    decimal freightAmount;
                    //Verifica se o horário é noturno
                    if (date.Hour > OvernightStart || date.Hour < OvernightEnd)
                    {
                        freightAmount = (decimal)(kilometers * OvernightFare);
                    }
                    else
                    {
                        //Verifica se o dia é sábado
                        if (date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            freightAmount = (decimal)(kilometers * SaturdayFare);
                        }
                        else
                        {
                            //Verifica se o dia é domingo
                            if (date.DayOfWeek == DayOfWeek.Sunday)
                            {
                                freightAmount = (decimal)(kilometers * SundayFare);
                            }
                            else
                            {
                                //Verifica se o dia é segunda
                                if (date.DayOfWeek == DayOfWeek.Monday)
                                {
                                    freightAmount = (decimal)(kilometers * MondayFare);
                                }
                                else
                                {
                                    freightAmount = (decimal)(kilometers * NormalFare);
                                }
                            }
                        }
                    }

                    //Verifica se a distancia é elevada para cobrar valor adicional
                    if (kilometers > AdditionalFareKilometersStart)
                    {
                        freightAmount = freightAmount + (decimal)((kilometers - AdditionalFareKilometersStart) * AdditionalFarePerKilometers);
                    }

                    return freightAmount;
                }
                else
                {
                    return -1; //Retorno de erro
                }
            }
            else
            {
                return -1; //Retorno de erro
            }
        }

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
                var freightAmount = CalculateFreight(10, DateTime.MinValue);
                Assert.AreEqual(-1, freightAmount);
            }

            [Test]
            public void Freight_InvalidDistance()
            {
                var freightAmount = CalculateFreight(-10, new DateTime(2020, 11, 18, 15, 0, 0));
                Assert.AreEqual(-1, freightAmount);
            }
        }
    }
}
