using CalculatorApi.Models;
using CalculatorApi.Controllers;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;
using System;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml;

namespace CalculatorApi.Tests.Integration
{
    [TestFixture]
    public class Integration_Simple
    {
        private OperationController controller { get; set; } = new OperationController();

        // Testing large number with multiple values
        [Test]
        public void Simple_Addition_Returns_9000000()
        {
            /*
            Calculation: 99 + 3000000 + 5151254 + 848647
            Expected Result: 9000000
            */

            // Corresponding JSON formatted request
            string jsonRequest = @"{
             ""Math"":
                {
                    ""Op"":
                    {
                        ""id"": ""Addition"",
                        ""Val"": [""99"", ""3000000"", ""5151254"", ""848647""]
                    }
                }
            }";

            ContentResult result = (ContentResult)controller.Post(jsonRequest);
            ClassicAssert.AreEqual(result.Content, "{\"Result\":\"9000000\"}");

            // Corresponding XML formatted request
            string xmlRequest = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <Maths>
                <Operation ID = ""Addition"">
                    <Value>99</Value>
                    <Value>3000000</Value>
                    <Value>5151254</Value>
                    <Value>848647</Value>
                </Operation>
            </Maths>
            ";

            XDocument xmlDocument = XDocument.Parse(xmlRequest);
            result = (ContentResult)controller.Post(xmlDocument.ToString());
            ClassicAssert.AreEqual(result.Content, "<Result>9000000</Result>");
        }


        // Testing with multiple values
        [Test]
        public void Simple_Multiplication_Returns_4560()
        {
            /*
            Calculation: 19 * 16 * 15
            Expected Result: 4560
            */

            // Corresponding JSON formatted request
            string jsonRequest = @"{
             ""MyMaths"":
                {
                    ""MyOperation"":
                    {
                        ""id"": ""Multiplication"",
                        ""Value"": [""19"", ""16"", ""15""]
                    }
                }
            }";

            ContentResult result = (ContentResult)controller.Post(jsonRequest);
            ClassicAssert.AreEqual(result.Content, "{\"Result\":\"4560\"}");

            // Corresponding XML formatted request
            string xmlRequest = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <Maths>
                <Operation ID = ""Multiplication"">
                    <Value>19</Value>
                    <Value>16</Value>
                    <Value>15</Value>
                </Operation>
            </Maths>
            ";

            XDocument xmlDocument = XDocument.Parse(xmlRequest);
            result = (ContentResult)controller.Post(xmlDocument.ToString());
            ClassicAssert.AreEqual(result.Content, "<Result>4560</Result>");
        }


        // Testing large negative number with multiple values
        [Test]
        public void Simple_Subtraction_Returns_Minus8999802()
        {
            /*
            Calculation: 99 - 3000000 - 5151254 - 848647
            Expected Result: -8999802
            */

            // Corresponding JSON formatted request
            string jsonRequest = @"{
             ""2MyMaths"":
                {
                    ""MyOperation2"":
                    {
                        ""id5"": ""Subtraction"",
                        ""2Value"": [""99"", ""3000000"", ""5151254"", ""848647""]
                    }
                }
            }";

            ContentResult result = (ContentResult)controller.Post(jsonRequest);
            ClassicAssert.AreEqual(result.Content, "{\"Result\":\"-8999802\"}");

            // Corresponding XML formatted request
            string xmlRequest = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <Maths>
                <Operation ID = ""Subtraction"">
                    <Value>99</Value>
                    <Value>3000000</Value>
                    <Value>5151254</Value>
                    <Value>848647</Value>
                </Operation>
            </Maths>
            ";

            XDocument xmlDocument = XDocument.Parse(xmlRequest);
            result = (ContentResult)controller.Post(xmlDocument.ToString());
            ClassicAssert.AreEqual(result.Content, "<Result>-8999802</Result>");
        }


        // Testing larger number with multiple values
        [Test]
        public void Simple_Division_Returns_209898_0277()
        {
            /*
            Calculation: 99 / 24 / 0.515 / 0.848 / 0.000045
            Expected Result: 209898.02772180495
            */

            // Corresponding JSON formatted request
            string jsonRequest = @"{
             ""M"":
                {
                    ""O"":
                    {
                        ""I"": ""Division"",
                        ""V"": [""99"", ""24"", ""0.515"", ""0.848"", ""0.000045""]
                    }
                }
            }";

            ContentResult result = (ContentResult)controller.Post(jsonRequest);
            ClassicAssert.AreEqual(result.Content, "{\"Result\":\"209898.02772180495\"}");

            // Corresponding XML formatted request
            string xmlRequest = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <Maths>
                <Operation ID = ""Division"">
                    <Value>99</Value>
                    <Value>24</Value>
                    <Value>0.515</Value>
                    <Value>0.848</Value>
                    <Value>0.000045</Value>
                </Operation>
            </Maths>
            ";

            XDocument xmlDocument = XDocument.Parse(xmlRequest);
            result = (ContentResult)controller.Post(xmlDocument.ToString());
            ClassicAssert.AreEqual(result.Content, "<Result>209898.02772180495</Result>");
        }
    }
}