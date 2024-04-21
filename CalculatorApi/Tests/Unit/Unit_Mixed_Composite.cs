using CalculatorApi.Models;
using NUnit.Framework.Legacy;
using NUnit.Framework;

namespace CalculatorApi.Tests.Unit
{
    [TestFixture]
    public class Unit_Mixed_Composite
    {
        // Testing mixed composite expression given in the assessment email
        [Test]
        public void Composite_Mixed_Returns_25()
        {
            AddOperation addOperation = new AddOperation
            {
                Id = "Addition",
                Value = [2, 3],
                Operations = new List<ArithmeticOperationBase>
                {
                    new MulOperation {
                        Id = "Multiplication",
                        Value = [4, 5]
                    }
                }
            };

            double result = addOperation.Calculate();

            /*
            Calculation: 2 + 3 + (4 * 5)
            Expected Result: 25
            */
            ClassicAssert.AreEqual(result, 25);
        }


        // Testing mixed composite with deep nesting only
        [Test]
        public void Composite_Mixed_Returns_6278_2()
        {
            AddOperation addOperation = new AddOperation
            {
                Id = "Addition",
                Value = [4, 2, 35, 5, 6224, 7, 8, 2],
                Operations = new List<ArithmeticOperationBase>
                {
                    new MulOperation {
                        Id = "Multiplication",
                        Value = [22, 5, 1],
                        Operations = new List<ArithmeticOperationBase>
                        {
                            new DivOperation {
                                Id = "Division",
                                Value = [24, 5, 1],
                                Operations = new List<ArithmeticOperationBase>
                                {
                                    new SubOperation {
                                        Id = "Subtraction",
                                        Value = [2, 51, 11]
                                    }
                                }
                            }
                        }
                    }
                }
            };

            double result = addOperation.Calculate();

            /*
            Calculation: 
            
            4 + 2 + 35 + 5 + 6224 + 7 + 8 + 2 + 
            (22 * 5 * 1 * (24 / 5 / 1 / (2 - 51 - 11)))

            Expected Result: 6278.2
            */
            ClassicAssert.AreEqual(result, 6278.2);
        }


        // Testing mixed composite a list of various operations
        [Test]
        public void Composite_Mixed_Returns_Minus1969_5()
        {
            SubOperation subOperation = new SubOperation
            {
                Id = "Subtraction",
                Value = [20, 19, 12, 5, 6, 17, 29, 2],
                Operations = new List<ArithmeticOperationBase>
                {
                    new MulOperation {
                        Id = "Multiplication",
                        Value = [19, 25, 4]
                    },
                    new DivOperation {
                        Id = "Division",
                        Value = [2, 4]
                    },
                    new AddOperation {
                        Id = "Addition",
                        Value = []
                    }
                }
            };

            double result = subOperation.Calculate();

            /*
            Calculation:

            20 - 19 - 12 - 5 - 6 - 17 - 29 - 2 - 
            ((19 * 25 * 4) - (2 / 4) - (0))

            Expected Result: -1969.5
            */
            ClassicAssert.AreEqual(result, -1969.5);
        }


        // Testing mixed division by zero for composite
        [Test]
        public void Composite_Mixed_Returns_Exception()
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
                                new AddOperation {
                                    Id = "Addition",
                                    Value = [2, 0, -2]
                                },
                                new SubOperation {
                                    Id = "Subtraction",
                                    Value = [6, 2]
                                },
                            }
                        },
                        new MulOperation {
                            Id = "Multiplication",
                            Value = [6, 2]
                        },
                        new MulOperation {
                            Id = "Multiplication",
                            Value = [6, 2]
                        },
                    }
                };

                double result = divOperation.Calculate();

                /*
                Calculation:

                4 / 2 / 3 / 5 / 6 / 7 / 8 / 2 / 
                ((2 / 5 / 1 / (2 + 0 + -2) / (6 - 2)) / (6 * 2) / (6 * 2))

                Expected Result: Exception, as shown below
                */
                Assert.Fail();

            }
            catch (Exception exception)
            {
                ClassicAssert.AreEqual(exception.Message, "Cannot divide by zero.");
            }
        }


        // Testing composite with no values in Maths root
        [Test]
        public void Composite_Mixed_Returns_8310_8737()
        {
            AddOperation addOperation = new AddOperation
            {
                Id = "Addition",
                Operations = new List<ArithmeticOperationBase>
                {
                    new AddOperation {
                        Id = "Addition",
                        Value = [2521, 5215, 23.0215],
                    },
                    new SubOperation {
                        Id = "Subtraction",
                        Value = [235, 52, 125, 11]
                    },
                    new MulOperation {
                        Id = "Multiplication",
                        Value = [4, 63, 2]
                    },
                    new DivOperation {
                        Id = "Division",
                        Value = [6, 4, 2, 0.88]
                    }
                }
            };

            double result = addOperation.Calculate();

            /*
            Calculation: 
            
            (2521 + 5215 + 23.0215) + (235 - 52 - 125 - 11) + 
            (4 * 63 * 2) + (6 / 4 / 2 / 0.88)

            Expected Result: 8310.873772727271
            */
            ClassicAssert.AreEqual(result, 8310.873772727271);
        }
    }
}