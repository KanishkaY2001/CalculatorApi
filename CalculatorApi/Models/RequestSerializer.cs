using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace CalculatorApi.Models
{

    public static class RequestSerializer
    {
        // Manually parses the JSON object recursively to fit the Operation model
        private static void ParseJson(JToken mathsToken)
        {
            try
            {
                // Cannot recurse further into the JToken property (null)
                if (!(mathsToken is JObject mathsObject)) return;

                // Iterating through each property in the current object
                List<JProperty> properties = mathsObject.Properties().ToList();
                for (int i = 0; i < properties.Count; ++i)
                {
                    JProperty property = properties[i];
                    var propertyValue = property.Value;
                    bool doubleCastable = false;
                    string newName = "";

                    // Current logic creates a new '_Operations_' array object to store all operations
                    // If a property with this name already exists, the 'Replace' logic may behave unexpectedly
                    if (property.Name.Equals("_Operations_"))
                        throw new DuplicateNameException("Operation objects cannot be called \"_Operations_\", please rename.");

                    // Math object is the child of the root object (null)
                    // Operation objects are children of the Maths / Operation object
                    if (propertyValue is JObject propertyObject)
                        newName = mathsObject.Parent == null ? "Maths" : "Operation";

                    // ID objects have string value
                    else if (propertyValue.Type is JTokenType.String)
                    {
                        string castedValue = (string)propertyValue!;
                        doubleCastable = double.TryParse(castedValue, out _) || castedValue.Equals("");
                        newName = doubleCastable ? "Value" : "@ID";
                    }
                        
                    // Value objects have array value
                    else if (propertyValue.Type is JTokenType.Array)
                        newName = "Value";

                    // Replace old property with updated name and original value
                    // Only update property name if it's invalid for deserialization
                    if (!(property.Name.Equals(newName)) || doubleCastable)
                    {
                        // For XML, when a single value is passed, Newton automatically parses into string
                        // This will correct the automatic parsing and ensure that Value is an array
                        if (doubleCastable && !(property.Value is JArray))
                            propertyValue = ((string)propertyValue!).Equals("") 
                                ? new JArray() 
                                : new JArray(propertyValue);

                        JProperty newProperty = new JProperty(newName, propertyValue);
                        property.Replace(newProperty);
                        property = newProperty;
                    }

                    // Recurse through JSON object to fix input spellings and collect operations
                    ParseJson(property.Value);

                    // Group operations together into a JSON array
                    if (newName.Equals("Operation"))
                    {
                        // Maths object will only have 1 operation object, array not required
                        if (mathsObject.Parent is JProperty mathsParent) 
                            if (mathsParent.Name.Equals("Maths")) return;

                        // Create operations array at the recursion depth to collect adjacent operations
                        if (mathsObject["_Operations_"] == null)
                            mathsObject.Add(new JProperty("_Operations_", new JArray()));

                        // Add the operation info into array and delete original operation object
                        JArray operations = (JArray)mathsObject["_Operations_"]!;
                        operations.Add(property.Value);
                        property.Remove();
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"Could not parse the JSON object: {exception.Message}");
            }
        }


        // Deserializes the JSON object using custom model (Operation) and settings
        public static MathsRoot Deserialize(JObject mathsObject)
        {
            try
            {
                // Perform custom JSON deserialization (different inputs / structure)
                ParseJson(mathsObject);

                // Create a new maths root object to calculate result
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new OperationConverter());
                MathsRoot? mathsRoot = mathsObject.ToObject<MathsRoot>(JsonSerializer.Create(settings));

                if (mathsRoot == null || mathsRoot.Maths == null)
                    throw new NullReferenceException("Request could not be deserialized, invalid formatting.");

                return mathsRoot;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Could not create MathsRoot object: {exception.Message}");
            }
        }
    }
}