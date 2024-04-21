using NUnit.Framework;
using CalculatorApi.Models;
using NUnit.Framework.Legacy;
using Newtonsoft.Json.Linq;

namespace CalculatorApi.Tests.Unit
{
    [TestFixture]
    public class Unit_Subtraction
    {
        // Testing with no values
        [Test]
        public void Empty_Subtraction_Returns_0()
        {
            SubOperation subOperation = new SubOperation
            {
                Id = "Subtraction",
                Value = []
            };

            double result = subOperation.Calculate();

            /*
            Calculation: EMPTY
            Expected Result: 0
            */
            ClassicAssert.AreEqual(result, 0);
        }


        // Testing with single value
        [Test]
        public void Simple_Subtraction_Returns_5()
        {
            SubOperation subOperation = new SubOperation
            {
                Id = "Subtraction",
                Value = [5]
            };

            double result = subOperation.Calculate();

            /*
            Calculation: 5
            Expected Result: 5
            */
            ClassicAssert.AreEqual(result, 5);
        }


        // Testing with multiple values
        [Test]
        public void Simple_Subtraction_Returns_Minus12()
        {
            SubOperation subOperation = new SubOperation
            {
                Id = "Subtraction",
                Value = [19, 16, 15]
            };

            double result = subOperation.Calculate();

            /*
            Calculation: 19 - 16 - 15
            Expected Result: -12
            */
            ClassicAssert.AreEqual(result, -12);
        }


        // Testing large negative number with multiple values
        [Test]
        public void Simple_Subtraction_Returns_Minus8999802()
        {
            SubOperation subOperation = new SubOperation
            {
                Id = "Subtraction",
                Value = [99, 3000000, 5151254, 848647]
            };

            double result = subOperation.Calculate();

            /*
            Calculation: 99 - 3000000 - 5151254 - 848647
            Expected Result: -8999802
            */
            ClassicAssert.AreEqual(result, -8999802);
        }


        // Testing composite with small negative number
        [Test]
        public void Composite_Subtraction_Returns_Minus57()
        {
            SubOperation subOperation = new SubOperation
            {
                Id = "Subtraction",
                Value = [20, 19, 12, 5, 6, 17, 29, 2],
                Operations = new List<ArithmeticOperationBase>
                {
                    new SubOperation {
                        Id = "Subtraction",
                        Value = [19, 25, 4]
                    },
                    new SubOperation {
                        Id = "Subtraction",
                        Value = [2, 0]
                    },
                    new SubOperation {
                        Id = "Subtraction",
                        Value = [1]
                    }
                }
            };

            double result = subOperation.Calculate();

            /*
            Calculation:

            20 - 19 - 12 - 5 - 6 - 17 - 29 - 2 - 
            ((19 - 25 - 4) - (2 - 0) - (1))

            Expected Result: -57
            */
            ClassicAssert.AreEqual(result, -57);
        }


        // Testing deeply nested composite with large number
        [Test]
        public void Composite_Subtraction_Returns_4148345967()
        {
            SubOperation subOperation = new SubOperation
            {
                Id = "Subtraction",
                Value = [26340, 19346, 12, 578435, 6, 13467, 29, 2],
                Operations = new List<ArithmeticOperationBase>
                {
                    new SubOperation {
                        Id = "Subtraction",
                        Value = [19],
                        Operations = new List<ArithmeticOperationBase>
                        {
                            new SubOperation {
                                Id = "Subtraction",
                                Value = [19, 2500, 4],
                                Operations = new List<ArithmeticOperationBase>
                                {
                                    new SubOperation {
                                        Id = "Subtraction",
                                        Value = [194, 25, 4124125166]
                                    }
                                }
                            }
                        }
                    },
                    new SubOperation {
                        Id = "Subtraction",
                        Value = [2, 3460],
                        Operations = new List<ArithmeticOperationBase>
                        {
                            new SubOperation {
                                Id = "Subtraction",
                                Value = [19, 25000000, 4]
                            },
                            new SubOperation {
                                Id = "Subtraction",
                                Value = [4],
                                Operations = new List<ArithmeticOperationBase>
                                {
                                    new SubOperation {
                                        Id = "Subtraction",
                                        Value = [190000, 25, 4]
                                    }
                                }
                            },
                            new SubOperation {
                                Id = "Subtraction",
                                Value = [1900, 25, 4]
                            }
                        }
                    },
                    new SubOperation {
                        Id = "Subtraction",
                        Value = []
                    }
                }
            };

            double result = subOperation.Calculate();

            /*
            Calculation:

            26340 - 19346 - 12 - 578435 - 6 - 13467 - 29 - 2 - 
            ((19 - (19 - 2500 - 4 - (194 - 25 - 4124125166))) - 
            (2 - 3460 - ((19 - 25000000 - 4) - 
            (4 - (190000 - 25 - 4)) - (1900 - 25 - 4))))

            Expected Result: 4148345967
            */
            ClassicAssert.AreEqual(result, 4148345967);
        }
    }
}