using NUnit.Framework;
using System;

namespace CleanCode.UnityTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Calculate(10.5, DateTime.Now);

            Console.WriteLine($"Result: R$ {result:F}");
            Console.ReadKey();
        }

        public static decimal Calculate(double dist, DateTime dt)
        {
            //Verifica se a distancia informada é válida
            if (dist > 0)
            {
                //Verifica se a data informada é válida
                if (dt > DateTime.MinValue)
                {
                    decimal result;
                    //Verifica se o horário é noturno
                    if (dt.Hour > 21 || dt.Hour < 7)
                    {
                        result = (decimal)(dist * 4);
                    }
                    else
                    {
                        //Verifica se o dia é sábado
                        if (dt.DayOfWeek == DayOfWeek.Saturday)
                        {
                            result = (decimal)(dist * 2);
                        }
                        else
                        {
                            //Verifica se o dia é domingo
                            if (dt.DayOfWeek == DayOfWeek.Sunday)
                            {
                                result = (decimal)(dist * 3);
                            }
                            else
                            {
                                //Verifica se o dia é segunda
                                if (dt.DayOfWeek == DayOfWeek.Monday)
                                {
                                    result = (decimal)(dist * 0.5);
                                }
                                else
                                {
                                    result = (decimal)(dist * 1);
                                }
                            }
                        }
                    }

                    //Verifica se a distancia é elevada para cobrar valor adicional
                    if (dist > 50)
                    {
                        result = result + (decimal)((dist - 50) * 0.75);
                    }

                    return result;
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
                var freightAmount = Calculate(10, new DateTime(2020, 11, 18, 15, 0, 0));
                Assert.AreEqual(10, freightAmount);
            }

            [Test]
            public void Freight_Sunday()
            {
                var freightAmount = Calculate(10, new DateTime(2020, 11, 22, 15, 0, 0));
                Assert.AreEqual(30, freightAmount);
            }

            [Test]
            public void Freight_Saturday()
            {
                var freightAmount = Calculate(10, new DateTime(2020, 11, 21, 15, 0, 0));
                Assert.AreEqual(20, freightAmount);
            }

            [Test]
            public void Freight_Monday()
            {
                var freightAmount = Calculate(10, new DateTime(2020, 11, 23, 15, 0, 0));
                Assert.AreEqual(5, freightAmount);
            }

            [Test]
            public void Freight_Overnight()
            {
                var freightAmount = Calculate(10, new DateTime(2020, 11, 18, 22, 0, 0));
                Assert.AreEqual(40, freightAmount);
            }

            [Test]
            public void Freight_NormalDayWithAdditionalByDistance()
            {
                var freightAmount = Calculate(60, new DateTime(2020, 11, 18, 15, 0, 0));
                Assert.AreEqual(67.5, freightAmount);
            }

            [Test]
            public void Freight_InvalidDate()
            {
                var freightAmount = Calculate(10, DateTime.MinValue);
                Assert.AreEqual(-1, freightAmount);
            }

            [Test]
            public void Freight_Distance()
            {
                var freightAmount = Calculate(-10, new DateTime(2020, 11, 18, 15, 0, 0));
                Assert.AreEqual(-1, freightAmount);
            }
        }
    }
}
