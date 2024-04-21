# CalculatorApi
CalculatorApi is a web API project that enables the user to process arithmetic calculations through JSON and XML requests
- The project was through .NET Visual Studio, using the ASP .NET Core Web API framework
- The project also supports OpenAPI support for Swagger
- The solution is highly generalized, enabling property names and overall structure to be diverse / different
# Considerations
Although the calculator can theoretically process *any* type of expression, the structure only allows for **Value** followed by **n** number of **operations**. This *drawback* is evident in an expression such as `(4*5) / 3`.
- However, this can be overcome by defining `3` as another operation, effectively transforming the expression into `(4*5) / (3)`. An example of how to format JSON/XML is given in the `Integration_Composite` test file.
# Request Examples
The full list of examples are available in the `Tests` directory, showcasing a variety of unit and integration tests and incorporating numerous operations to calculate diverse equations.

## Ex1 → $2 + 3 + (4 + 5) = 25$

### XML Request Example
```xml
<?xml version="1.0" encoding="UTF-8"?>
<Maths>
    <Operation ID="Addition">
    <Value>2</Value>
    <Value>3</Value>
    <Operation ID="Multiplication">
        <Value>4</Value>
        <Value>5</Value>
    </Operation>
    </Operation>
</Maths>
```

### JSON Request Example
```json
{
    "Maths": 
    {
        "Operation": 
        {
            "@ID": "Addition",
            "Value": ["2", "3"],
            "Operation": 
            {
                "@ID": "Multiplication",
                "Value": ["4", "5"],
            }
        }
    }
}
```

## Ex2 → $(8 + 15) * (2 + 4) = 138$

### XML Request Example

```xml
<?xml version="1.0" encoding="UTF-8"?>
<Maths>
    <Operation ID="Multiplication">
    <Operation1 ID="Addition">
        <Value>8</Value>
        <Value>15</Value>
    </Operation1>
    <Operation2 ID="Addition">
        <Value>2</Value>
        <Value>4</Value>
    </Operation2>
    </Operation>
</Maths>
```

### JSON Request Example

```json
{
    "Maths": 
    {
        "Operation": 
        {
            "@ID": "Multiplication",
            "Operation1": 
            {
                "@ID": "Addition",
                "Value": ["8", "15"]
            },
            "Operation2": 
            {
                "@ID": "Addition",
                "Value": ["2", "4"]
            }
        }
    }
}
```

## Ex3 → $100 - (9 + 6 + (4 * 5) + (2 * 5) + (20 / 4)) = 50$

### XML Request Example
```xml
<?xml version="1.0" encoding="UTF-8"?>
<Maths>
    <Operation ID="Subtraction">
    <Value>100</Value>
    <Operation ID="Addition">
        <Value>9</Value>
        <Value>6</Value>
        <Operation1 ID="Multiplication">
            <Value>4</Value>
            <Value>5</Value>
        </Operation1>
        <Operation2 ID="Multiplication">
            <Value>2</Value>
            <Value>5</Value>
        </Operation2>
        <Operation3 ID="Division">
            <Value>20</Value>
            <Value>4</Value>
        </Operation3>
    </Operation>
    </Operation>
</Maths>
```

### JSON Request Example
```json
{
    "Maths": 
    {
        "Operation0":
        {
            "@ID": "Subtraction",
            "Value": ["100"],
            "Operation1": 
            {
                "@ID": "Addition",
                "Value": ["9", "6"],
                "Operation2": 
                {
                    "@ID": "Multiplication",
                    "Value": ["4", "5"],
                },
                "Operation3": 
                {
                    "@ID": "Multiplication",
                    "Value": ["2","5"],
                },
                "Operation4": 
                {
                    "@ID": "Division",
                    "Value": ["20","4"],
                }
            }
        }
    }
}
```
