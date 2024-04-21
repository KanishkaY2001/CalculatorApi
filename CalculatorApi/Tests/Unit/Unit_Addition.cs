using NUnit.Framework;
using CalculatorApi.Models;
using NUnit.Framework.Legacy;
using Newtonsoft.Json.Linq;

namespace CalculatorApi.Tests.Unit
{
    [TestFixture]
    public class Unit_Addition
    {
        // Testing with no values
        [Test]
        public void Empty_Addition_Returns_0()
        {
            AddOperation addOperation = new AddOperation
            { 
                Id = "Addition", 
                Value = [] 
            };
            
            double result = addOperation.Calculate();

            /*
            Calculation: EMPTY
            Expected Result: 0
            */
            ClassicAssert.AreEqual(result, 0);
        }


        // Testing with single value
        [Test]
        public void Simple_Addition_Returns_5()
        {
            AddOperation addOperation = new AddOperation 
            { 
                Id = "Addition", 
                Value = [5] 
            };

            double result = addOperation.Calculate();

            /*
            Calculation: 5
            Expected Result: 5
            */
            ClassicAssert.AreEqual(result, 5);
        }


        // Testing with multiple values
        [Test]
        public void Simple_Addition_Returns_50()
        {
            AddOperation addOperation = new AddOperation 
            { 
                Id = "Addition", 
                Value = [19, 16, 15] 
            };

            double result = addOperation.Calculate();

            /*
            Calculation: 19 + 16 + 15
            Expected Result: 50
            */
            ClassicAssert.AreEqual(result, 50);
        }


        // Testing large number with multiple values
        [Test]
        public void Simple_Addition_Returns_9000000()
        {
            AddOperation addOperation = new AddOperation 
            { 
                Id = "Addition", 
                Value = [99, 3000000, 5151254, 848647] 
            };

            double result = addOperation.Calculate();

            /*
            Calculation: 99 + 3000000 + 5151254 + 848647
            Expected Result: 9000000
            */
            ClassicAssert.AreEqual(result, 9000000);
        }


        // Testing composite with small number
        [Test]
        public void Composite_Addition_Returns_161()
        {
            AddOperation addOperation = new AddOperation
            {
                Id = "Addition",
                Value = [20, 19, 12, 5, 6, 17, 29, 2],
                Operations = new List<ArithmeticOperationBase>
                {
                    new AddOperation {
                        Id = "Addition",
                        Value = [19, 25, 4] 
                    },
                    new AddOperation {
                        Id = "Addition",
                        Value = [2, 0]
                    },
                    new AddOperation {
                        Id = "Addition",
                        Value = [1]
                    }
                }
            };

            double result = addOperation.Calculate();

            /*
            Calculation:

            20 + 19 + 12 + 5 + 6 + 17 + 29 + 2 + 
            ((19 + 25 + 4) + (2 + 0) + (1))

            Expected Result: 4149961011
            */
            ClassicAssert.AreEqual(result, 161);
        }


        // Testing deeply nested composite with large number
        [Test]
        public void Composite_Addition_Returns_4149961011()
        {
            AddOperation addOperation = new AddOperation
            {
                Id = "Addition",
                Value = [26340, 19346, 12, 578435, 6, 13467, 29, 2],
                Operations = new List<ArithmeticOperationBase>
                {
                    new AddOperation {
                        Id = "Addition",
                        Value = [19],
                        Operations = new List<ArithmeticOperationBase>
                        {
                            new AddOperation {
                                Id = "Addition",
                                Value = [19, 2500, 4],
                                Operations = new List<ArithmeticOperationBase>
                                {
                                    new AddOperation {
                                        Id = "Addition",
                                        Value = [194, 25, 4124125166]
                                    }
                                }
                            }
                        }
                    },
                    new AddOperation {
                        Id = "Addition",
                        Value = [2, 3460],
                        Operations = new List<ArithmeticOperationBase>
                        {
                            new AddOperation {
                                Id = "Addition",
                                Value = [19, 25000000, 4]
                            },
                            new AddOperation {
                                Id = "Addition",
                                Value = [4],
                                Operations = new List<ArithmeticOperationBase>
                                {
                                    new AddOperation {
                                        Id = "Addition",
                                        Value = [190000, 25, 4]
                                    }
                                }
                            },
                            new AddOperation {
                                Id = "Addition",
                                Value = [1900, 25, 4]
                            }
                        }
                    },
                    new AddOperation {
                        Id = "Addition",
                        Value = []
                    }
                }
            };

            double result = addOperation.Calculate();

            /*
            Calculation:

            26340 + 19346 + 12 + 578435 + 6 + 13467 + 29 + 2 + 
            (19 + (19 + 2500 + 4 + (194 + 25 + 4124125166))) + 
            (2 + 3460 + (19 + 25000000 + 4) + (4 + (190000 + 25 + 4)) + 
            (1900 + 25 + 4))

            Expected Result: 4149961011
            */
            ClassicAssert.AreEqual(result, 4149961011);
        }
    }
}