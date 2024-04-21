using CalculatorApi.Models;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CalculatorApi.Tests.Unit
{
    public class Unit_Division
    {
        // Testing with no values
        [Test]
        public void Empty_Division_Returns_0()
        {
            DivOperation divOperation = new DivOperation
            {
                Id = "Division",
                Value = []
            };

            double result = divOperation.Calculate();

            /*
            Calculation: EMPTY
            Expected Result: 0
            */
            ClassicAssert.AreEqual(result, 0);
        }


        // Testing with single value
        [Test]
        public void Simple_Division_Returns_5()
        {
            DivOperation divOperation = new DivOperation
            {
                Id = "Division",
                Value = [5]
            };

            double result = divOperation.Calculate();

            /*
            Calculation: 5
            Expected Result: 5
            */
            ClassicAssert.AreEqual(result, 5);
        }


        // Testing with multiple values
        [Test]
        public void Simple_Division_Returns_0_07916()
        {
            DivOperation divOperation = new DivOperation
            {
                Id = "Division",
                Value = [19, 16, 15]
            };

            double result = divOperation.Calculate();

            /*
            Calculation: 19 / 16 / 15
            Expected Result: 0.079166666666666663
            */
            ClassicAssert.AreEqual(result, 0.079166666666666663);
        }


        // Testing division by zero for values
        [Test]
        public void Simple_Division_Returns_Exception()
        {
            try
            {
                DivOperation divOperation = new DivOperation
                {
                    Id = "Division",
                    Value = [19, 16, 15, 0, 24]
                };

                double result = divOperation.Calculate();

                /*
                Calculation: 19 / 16 / 15
                Expected Result: 0.079166666666666663
                */
                Assert.Fail();
                
            }
            catch (Exception exception)
            {
                ClassicAssert.AreEqual(exception.Message, "Cannot divide by zero.");
            }
        }


        // Testing larger number with multiple values
        [Test]
        public void Simple_Division_Returns_209898_0277()
        {
            DivOperation divOperation = new DivOperation
            {
                Id = "Division",
                Value = [99, 24, 0.515, 0.848, 0.000045]
            };

            double result = divOperation.Calculate();

            /*
            Calculation: 99 / 24 / 0.515 / 0.848 / 0.000045
            Expected Result: 209898.02772180495
            */
            ClassicAssert.AreEqual(result, 209898.02772180495);
        }


        // Testing composite with small number
        [Test]
        public void Composite_Division_Returns_0_001488()
        {
            DivOperation divOperation = new DivOperation
            {
                Id = "Division",
                Value = [4, 2, 3, 5, 6, 7, 8, 2],
                Operations = new List<ArithmeticOperationBase>
                {
                    new DivOperation {
                        Id = "Division",
                        Value = [2, 5, 1]
                    },
                    new DivOperation {
                        Id = "Division",
                        Value = [6, 2]
                    },
                }
            };

            double result = divOperation.Calculate();

            /*
            Calculation: 
            
            4 / 2 / 3 / 5 / 6 / 7 / 8 / 2 / 
            ((2 / 5 / 1) / (6 / 2))

            Expected Result: 0.001488095238095238
            */
            ClassicAssert.AreEqual(result, 0.001488095238095238);
        }


        // Testing division by zero for composite
        [Test]
        public void Composite_Division_Returns_Exception()
        {
            try
            {
                DivOperation divOperation = new DivOperation
                {
                    Id = "Division",
                    Value = [4, 2, 3, 5, 6, 7, 8, 2],
                    Operations = new List<ArithmeticOperationBase>
                    {
                        new DivOperation {
                            Id = "Division",
                            Value = [2, 5, 1],
                            Operations = new List<ArithmeticOperationBase>
                            {
                                new DivOperation {
                                    Id = "Division",
                                    Value = [2, 0, 1]
                                },
                                new DivOperation {
                                    Id = "Division",
                                    Value = [6, 2]
                                },
                            }
                        },
                        new DivOperation {
                            Id = "Division",
                            Value = [6, 2]
                        },
                    }
                };

                double result = divOperation.Calculate();

                /*
                Calculation:

                4 / 2 / 3 / 5 / 6 / 7 / 8 / 2 / 
                ((2 / 5 / 1 / ((2 / 0 / 1) / (6 / 2))) / (6 / 2))

                Expected Result: Exception, as shown below
                */
                Assert.Fail();

            }
            catch (Exception exception)
            {
                ClassicAssert.AreEqual(exception.Message, "Cannot divide by zero.");
            }
        }


        // Testing composite with very small number
        [Test]
        public void Composite_Division_Returns_0_0003143()
        {
            DivOperation divOperation = new DivOperation
            {
                Id = "Division",
                Value = [2, 2, 1, 4, 0.25, 9, 0.0034, 1242],
                Operations = new List<ArithmeticOperationBase>
                {
                    new DivOperation {
                        Id = "Division",
                        Value = [2521, 5215, 23.0215],
                        Operations = new List<ArithmeticOperationBase>
                        {
                            new DivOperation {
                                Id = "Division",
                                Value = [.0012, 3e+5, 2],
                                Operations = new List<ArithmeticOperationBase>
                                {
                                    new DivOperation {
                                        Id = "Division",
                                        Value = [0.4, 223, 1]
                                    }
                                }
                            }
                        }
                    },
                    new DivOperation {
                        Id = "Division",
                        Value = [6, 0.2]
                    },
                    new DivOperation {
                        Id = "Division",
                        Value = [3],
                        Operations = new List<ArithmeticOperationBase>
                        {
                            new DivOperation {
                                Id = "Division",
                                Value = [2, 5, 1]
                            }
                        }
                    }
                }
            };

            double result = divOperation.Calculate();

            /*
            Calculation: 
            
            2 / 2 / 1 / 4 / 0.25 / 9 / 0.0034 / 1242 / 
            ((2521 / 5215 / 23.0215 / (.0012 / 3 / 10^5 / 2 / (0.4 / 223 / 1))) 
            / (6 / 0.2) / (3 / (2 / 5 / 1)))

            Expected Result: 0.00031436156891071144
            */
            ClassicAssert.AreEqual(result, 0.00031436156891071144);
        }
    }
}