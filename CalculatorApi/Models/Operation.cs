using Newtonsoft.Json;

namespace CalculatorApi.Models
{
    // Interface used to define a contract for various operations
    // Designed for supporting additional operation types (future)
    public interface IOperation
    {
        double Calculate();
    }


    // Base class for arithmetic operations, containing properties to define an expression
    public abstract class ArithmeticOperationBase : IOperation
    {
        // The operator that's used to calculate values and operations
        [JsonProperty("@ID")]
        public required string Id { get; set; }

        // Optional array of values calculated before operations
        public double[]? Value { get; set; }

        // Optional list of nested operations for complex expressions
        [JsonProperty("_Operations_")]
        public List<ArithmeticOperationBase>? Operations { get; set; }

        // Calcualtes the operation result, implemented for derived operators
        public abstract double Calculate();
        protected void NoValueAndOperations()
        {
            if (Value == null && Operations == null)
                throw new InvalidOperationException("Must have value(s) or operation(s) for calculation.");
        }
        protected double SafeCalculate(ArithmeticOperationBase operation)
        {
            if (operation == null)
                throw new NullReferenceException("A nested operation in the expression is formatted incorrectly.");
            return operation.Calculate();
        }
    }


    // Wrapper class that contains the top-level operation for the expression
    public class Maths
    {
        public required ArithmeticOperationBase Operation { get; set; }
    }


    // Root class to encapsulate the calculation structure for deserialization
    public class MathsRoot
    {
        public required Maths Maths { get; set; }
    }


    // Concrete implementation for addition operations
    public class AddOperation : ArithmeticOperationBase
    {
        // Calculates the sum of all values and operations
        public override double Calculate()
        {
            double result = 0.0;

            // Sum of all direct values, if any
            if (Value != null)
                for (int i = 0; i < Value.Length; ++i)
                    result += Value[i];

            // Recursive sum of all operations, if any
            if (Operations != null)
            {
                double compositeResult = 0.0;
                for (int i = 0; i < Operations.Count; ++i)
                    compositeResult += SafeCalculate(Operations[i]);
                result += compositeResult;
            }
                

            return result;
        }
    }


    // Concrete implementation for subtraction operations
    public class SubOperation : ArithmeticOperationBase
    {
        // Calculates the difference of all values and operations
        public override double Calculate()
        {
            NoValueAndOperations();
            double result = 0;

            // Difference of all direct values, if any
            if (Value != null && Value.Length > 0)
            {
                result = Value[0];
                for (int i = 1; i < Value.Length; ++i)
                    result -= Value[i];
            }

            // Recursive quotient of all operations, if any
            if (Operations != null && Operations.Count > 0)
            {
                double compositeResult = SafeCalculate(Operations[0]);
                for (int i = 1; i < Operations.Count; ++i)
                    compositeResult -= SafeCalculate(Operations[i]);
                result -= compositeResult;
            }

            return (double)result!;
        }
    }


    // Concrete implementation for multiplication operations
    public class MulOperation : ArithmeticOperationBase
    {
        // Calculates the product of all values and operations
        public override double Calculate()
        {
            double result = 1.0;

            // Product of all direct values, if any
            if (Value != null)
                for (int i = 0; i < Value.Length; ++i)
                    result *= Value[i];

            // Recursive product of all operations, if any
            if (Operations != null)
            {
                double compositeResult = 1.0;
                for (int i = 0; i < Operations.Count; ++i)
                    compositeResult *= SafeCalculate(Operations[i]);
                result *= compositeResult;
            }

            return result;
        }
    }


    // Concrete implementation for division operations
    public class DivOperation : ArithmeticOperationBase
    {
        // Calculates the quotient of all values and operations
        public override double Calculate()
        {
            NoValueAndOperations();
            bool ValuesExist = Value != null && Value.Length > 0;
            double result = 0;

            // Quotient of all direct values, if any
            if (ValuesExist)
            {
                result = Value![0];
                for (int i = 1; i < Value.Length; ++i)
                {
                    if (Value[i] == 0)
                        throw new DivideByZeroException("Cannot divide by zero.");
                    result /= Value[i];
                }
            }

            // Recursive quotient of all operations, if any
            if (Operations != null && Operations.Count > 0)
            {
                double compositeResult = SafeCalculate(Operations[0]);
                for (int i = 1; i < Operations.Count; ++i)
                {
                    double safeCompositeResult = SafeCalculate(Operations[i]);

                    // Check for recursive calculations becoming zero (illegal)
                    if (safeCompositeResult == 0 || compositeResult == 0)
                        throw new DivideByZeroException("Cannot divide by zero.");
                    compositeResult /= safeCompositeResult;
                }

                // If Operation doesn't have Value, take first composite result as its Value
                result = !ValuesExist ? compositeResult : result / compositeResult;
            }

            return (double)result!;
        }
    }
}