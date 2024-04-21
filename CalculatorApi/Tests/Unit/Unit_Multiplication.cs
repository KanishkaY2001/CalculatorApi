using CalculatorApi.Models;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CalculatorApi.Tests.Unit
{
    public class Unit_Multiplication
    {
        // Testing with no values
        [Test]
        public void Empty_Multiplication_Returns_0()
        {
            MulOperation mulOperation = new MulOperation 
            { 
                Id = "Multiplication", 
                Value = [] 
            };

            double result = mulOperation.Calculate();

            /*
            Calculation: EMPTY
            Expected Result: 1
            */
            ClassicAssert.AreEqual(result, 1);
        }


        // Testing with single value
        [Test]
        public void Simple_Multiplication_Returns_5()
        {
            MulOperation mulOperation = new MulOperation
            {
                Id = "Multiplication",
                Value = [5]
            };

            double result = mulOperation.Calculate();

            /*
            Calculation: 5
            Expected Result: 5
            */
            ClassicAssert.AreEqual(result, 5);
        }


        // Testing with multiple values
        [Test]
        public void Simple_Multiplication_Returns_4560()
        {
            MulOperation mulOperation = new MulOperation
            {
                Id = "Multiplication",
                Value = [19, 16, 15]
            };

            double result = mulOperation.Calculate();

            /*
            Calculation: 19 * 16 * 15
            Expected Result: 4560
            */
            ClassicAssert.AreEqual(result, 4560);
        }


        // Testing larger number with multiple values
        [Test]
        public void Simple_Multiplication_Returns_1_40082()
        {
            MulOperation mulOperation = new MulOperation
            {
                Id = "Multiplication",
                Value = [.3e+4, 99, 24, 515, 848, 0.45]
            };

            double result = mulOperation.Calculate();

            /*
            Calculation: .3 * 10^4 * 99 * 24 * 515 * 848 * 0.45
            Expected Result: 1.400823072e+12
            */
            ClassicAssert.AreEqual(result, 1.400823072e+12);
        }


        // Testing composite with large number
        [Test]
        public void Composite_Multiplication_Returns_9676800()
        {
            MulOperation mulOperation = new MulOperation
            {
                Id = "Multiplication",
                Value = [4, 2, 3, 5, 6, 7, 8, 2],
                Operations = new List<ArithmeticOperationBase>
                {
                    new MulOperation {
                        Id = "Multiplication",
                        Value = [2, 5, 1]
                    },
                    new MulOperation {
                        Id = "Multiplication",
                        Value = [6, 2]
                    },
                    new MulOperation {
                        Id = "Multiplication",
                        Value = [1]
                    }
                }
            };

            double result = mulOperation.Calculate();

            /*
            Calculation: 
            
            4 * 2 * 3 * 5 * 6 * 7 * 8 * 2 * 
            ((2 * 5 * 1) * (6 * 2) * (1))

            Expected Result: 9676800
            */
            ClassicAssert.AreEqual(result, 9676800);
        }


        // Testing composite with very large number
        [Test]
        public void Composite_Multiplication_Returns_35460298430498536()
        {
            MulOperation mulOperation = new MulOperation
            {
                Id = "Multiplication",
                Value = [2, 2, 1, 4, 0.25, 9, 0.0034, 1242],
                Operations = new List<ArithmeticOperationBase>
                {
                    new MulOperation {
                        Id = "Multiplication",
                        Value = [2521, 5215, 23.0215],
                        Operations = new List<ArithmeticOperationBase>
                        {
                            new MulOperation {
                                Id = "Multiplication",
                                Value = [.0012, 3e+5, 2],
                                Operations = new List<ArithmeticOperationBase>
                                {
                                    new MulOperation {
                                        Id = "Multiplication",
                                        Value = [0.4, 223, 1]
                                    }
                                }
                            }
                        }
                    },
                    new MulOperation {
                        Id = "Multiplication",
                        Value = [6, 0.2]
                    },
                    new MulOperation {
                        Id = "Multiplication",
                        Value = [],
                        Operations = new List<ArithmeticOperationBase>
                        {
                            new MulOperation {
                                Id = "Multiplication",
                                Value = [2, 5, 1]
                            }
                        }
                    }
                }
            };

            double result = mulOperation.Calculate();

            /*
            Calculation: 
            
            2 * 2 * 1 * 4 * 0.25 * 9 * 0.0034 * 1242 * 
            ((2521 * 5215 * 23.0215 * (.0012 * 3 * 10^5 * 2 * (0.4 * 223 * 1)))
            * (6 * 0.2) * ((2 * 5 * 1)))

            Expected Result: 9676800
            */
            ClassicAssert.AreEqual(result, 35460298430498536);
        }
    }
}