using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CalculatorApi.Models
{
    // Custom JSON converter  for deserializing various operation types
    public class OperationConverter : JsonConverter
    {
        private readonly Dictionary<string, Type> _TypeMap;
        public override bool CanWrite => false;

        // A map of known identifiers to their corresponding operations
        public OperationConverter()
        {
            _TypeMap = new Dictionary<string, Type>()
            {
                { "Addition", typeof(AddOperation) },
                { "Subtraction", typeof(SubOperation) },
                { "Multiplication", typeof(MulOperation) },
                { "Division", typeof(DivOperation) }
            };
        }


        // Check if type of object can be converted into existing _TypeMap
        public override bool CanConvert(Type mathType)
        {
            return typeof(IOperation).IsAssignableFrom(mathType);
        }


        // Reads JSON (Operations array) input and converts it into appropriate IOperation instance, based on Id
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject operationObject = JObject.Load(reader);
            string operationId = (string)operationObject["@ID"]!;

            // Check that the operation has valid Id before converting into instance
            if (_TypeMap.TryGetValue(operationId, out Type? operationType))
            {
                IOperation operationBase = (IOperation)Activator.CreateInstance(operationType)!;
                serializer.Populate(operationObject.CreateReader(), operationBase);
                return operationBase;
            }

            throw new JsonSerializationException("Invalid operator [ Addition | Subtraction | Multiplication | Division ].");
        }


        // Writing is disabled, this method should not be called
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Method should not be called; CanWrite is false.");
        }
    }
}
