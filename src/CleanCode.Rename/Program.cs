using NUnit.Framework;
using System;


namespace CleanCode.Rename
{
    class Program
    {
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
                    if (date.Hour > 21 || date.Hour < 7)
                    {
                        freightAmount = (decimal)(kilometers * 4);
                    }
                    else
                    {
                        //Verifica se o dia é sábado
                        if (date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            freightAmount = (decimal)(kilometers * 2);
                        }
                        else
                        {
                            //Verifica se o dia é domingo
                            if (date.DayOfWeek == DayOfWeek.Sunday)
                            {
                                freightAmount = (decimal)(kilometers * 3);
                            }
                            else
                            {
                                //Verifica se o dia é segunda
                                if (date.DayOfWeek == DayOfWeek.Monday)
                                {
                                    freightAmount = (decimal)(kilometers * 0.5);
                                }
                                else
                                {
                                    freightAmount = (decimal)(kilometers * 1);
                                }
                            }
                        }
                    }

                    //Verifica se a distancia é elevada para cobrar valor adicional
                    if (kilometers > 50)
                    {
                        freightAmount = freightAmount + (decimal)((kilometers - 50) * 0.75);
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
