using CalculatorApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Xml.Linq;

namespace CalculatorApi.Tests.Integration
{
    [TestFixture]
    public class Integration_Composite
    {
        private OperationController controller { get; set; } = new OperationController();

        // Testing a composite expression, (example) as given in the assessment email
        [Test]
        public void Composite_Addition_Returns_25()
        {
            /*
            Calculation: 2 + 3 + (4 * 5)
            Expected Result: 25
            */

            // Corresponding JSON formatted request
            string jsonRequest = @"{
            ""Math"":
                {
                    ""Op"":
                    {
                        ""id"": ""Addition"",
                        ""Val"": [""2"", ""3""],
                        ""Op"":
                        {
                            ""id"": ""Multiplication"",
                            ""Val"": [""4"", ""5""]
                        }
                    }
                }
            }";

            ContentResult result = (ContentResult)controller.Post(jsonRequest);
            ClassicAssert.AreEqual(result.Content, "{\"Result\":\"25\"}");

            // Corresponding XML formatted request
            string xmlRequest = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <Maths>
                <Operation ID=""Addition"">
                <Value>2</Value>
                <Value>3</Value>
                <Operation ID=""Multiplication"">
                    <Value>4</Value>
                    <Value>5</Value>
                </Operation>
                </Operation>
            </Maths>
            ";

            XDocument xmlDocument = XDocument.Parse(xmlRequest);
            result = (ContentResult)controller.Post(xmlDocument.ToString());
            ClassicAssert.AreEqual(result.Content, "<Result>25</Result>");
        }


        // Testing a composite expression with more nesting and same-level operations
        [Test]
        public void Composite_Addition_Returns_50()
        {
            /*
            Calculation: 100 - (9 + 6 + (4 * 5) + (2 * 5) + (20 / 4))
            Expected Result: 50
            */

            // Corresponding JSON formatted request
            string jsonRequest = @"{
            ""Math"":
                {
                    ""Op"":
                    {
                        ""id"": ""Subtraction"",
                        ""Val"": [""100""],
                        ""Op1"":
                        {
                            ""id"": ""Addition"",
                            ""Val"": [""9"", ""6""],
                            ""Op2"":
                            {
                                ""id"": ""Multiplication"",
                                ""Val"": [""4"", ""5""]
                            },
                            ""Op3"":
                            {
                                ""id"": ""Multiplication"",
                                ""Val"": [""2"", ""5""]
                            },
                            ""Op4"":
                            {
                                ""id"": ""Division"",
                                ""Val"": [""20"", ""4""]
                            }
                        }
                    }
                }
            }";

            ContentResult result = (ContentResult)controller.Post(jsonRequest);
            ClassicAssert.AreEqual(result.Content, "{\"Result\":\"50\"}");

            // Corresponding XML formatted request
            string xmlRequest = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <Maths>
                <Operation ID=""Subtraction"">
                <Value>100</Value>
                <Operation ID=""Addition"">
                    <Value>9</Value>
                    <Value>6</Value>
                    <Operation1 ID=""Multiplication"">
                        <Value>4</Value>
                        <Value>5</Value>
                    </Operation1>
                    <Operation2 ID=""Multiplication"">
                        <Value>2</Value>
                        <Value>5</Value>
                    </Operation2>
                    <Operation3 ID=""Division"">
                        <Value>20</Value>
                        <Value>4</Value>
                    </Operation3>
                </Operation>
                </Operation>
            </Maths>
            ";

            XDocument xmlDocument = XDocument.Parse(xmlRequest);
            result = (ContentResult)controller.Post(xmlDocument.ToString());
            ClassicAssert.AreEqual(result.Content, "<Result>50</Result>");
        }


        // Test to showcase a drawback with my current calculator system
        // Value is always processed before any Operation(s). So to do something like:
        // (10*10) / 50 = 20, you would require 3 operations as shown below
        [Test]
        public void Composite_Addition_Returns_2()
        {
            /*
            Calculation: (10 * 10) / 50
            Modified Calculation: (10 * 10) / (50)
            Expected Result: 2
            */

            // Corresponding JSON formatted request
            string jsonRequest = @"{
            ""Maths"":
                {
                    ""Operation"":
                    {
                        ""@ID"": ""Division"",
                        ""Operation1"":
                        {
                            ""@ID"": ""Multiplication"",
                            ""Value"": [""10"", ""10""],
                        },
                        ""Operation2"":
                        {
                            ""@ID"": ""Addition"", // This doesn't matter in these cases
                            ""Value"": [""50""],
                        }
                    }
                }
            }";

            ContentResult result = (ContentResult)controller.Post(jsonRequest);
            ClassicAssert.AreEqual(result.Content, "{\"Result\":\"2\"}");

            // Corresponding XML formatted request
            string xmlRequest = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <Maths>
                <Operation ID=""Division"">
                <Operation1 ID=""Multiplication"">
                    <Value>10</Value>
                    <Value>10</Value>
                </Operation1>
                <Operation2 ID=""Addition"">
                    <Value>50</Value>
                </Operation2>
                </Operation>
            </Maths>
            ";

            XDocument xmlDocument = XDocument.Parse(xmlRequest);
            result = (ContentResult)controller.Post(xmlDocument.ToString());
            ClassicAssert.AreEqual(result.Content, "<Result>2</Result>");
        }


        // Showcasing a deeply nested operation with empty value arrays
        [Test]
        public void Composite_Addition_Returns_6278_2()
        {
            /*
            Calculation: 
            
            4 + 2 + 35 + 5 + 6224 + 7 + 8 + 2 + 
            (22 * 5 * 1 * (24 / 5 / 1 / (2 - 51 - 11)))

            Expected Result: 6278.2
            */

            // Corresponding JSON formatted request
            string jsonRequest = @"{
            ""Maths"":
                {
                    ""Operation"":
                    {
                        ""@ID"": ""Addition"",
                        ""Value"": [""4"", ""2"", ""35"", ""5"", ""6224"", ""7"", ""8"", ""2""],
                        ""Operation"":
                        {
                            ""@ID"": ""Multiplication"",
                            ""Value"": [""22"", ""5"", ""1""],
                            ""Operation"":
                            {
                                ""@ID"": ""Division"",
                                ""Value"": [""24"", ""5"", ""1""],
                                ""Operation"":
                                {
                                    ""@ID"": ""Subtraction"",
                                    ""Value"": [""2"", ""51"", ""11""],
                                    ""Operation"":
                                    {
                                        ""@ID"": ""Addition"",
                                        ""Value"": [], // Showcasing Value with no numbers
                                    }
                                }
                            }
                        },
                    }
                }
            }";

            ContentResult result = (ContentResult)controller.Post(jsonRequest);
            ClassicAssert.AreEqual(result.Content, "{\"Result\":\"6278.2\"}");

            // Corresponding XML formatted request
            string xmlRequest = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <Maths>
                <Operation ID=""Addition"">
                <Value>4</Value>
                <Value>2</Value>
                <Value>35</Value>
                <Value>5</Value>
                <Value>6224</Value>
                <Value>7</Value>
                <Value>8</Value>
                <Value>2</Value>
                <Operation1 ID=""Multiplication"">
                    <Value>22</Value>
                    <Value>5</Value>
                    <Value>1</Value>
                    <Operation1 ID=""Division"">
                        <Value>24</Value>
                        <Value>5</Value>
                        <Value>1</Value>
                        <Operation1 ID=""Subtraction"">
                            <Value>2</Value>
                            <Value>51</Value>
                            <Value>11</Value>
                            <Operation1 ID=""Addition"">
                                <Value></Value>
                            </Operation1>
                        </Operation1>
                    </Operation1>
                </Operation1>
                </Operation>
            </Maths>
            ";

            XDocument xmlDocument = XDocument.Parse(xmlRequest);
            result = (ContentResult)controller.Post(xmlDocument.ToString());
            ClassicAssert.AreEqual(result.Content, "<Result>6278.2</Result>");
        }
    }    
}