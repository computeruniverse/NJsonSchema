using NJsonSchema.CodeGeneration.CSharp;
using System.Threading.Tasks;
using Xunit;

namespace NJsonSchema.CodeGeneration.CSharp.Tests;

public class ClassAttributesValueTest
{
    [Fact]
    public async Task When_classAttributes_exits_add_Attributes_to_Class()
    {
        var json =
            @"{
                ""title"": ""Product"",
                ""type"": ""object"",
                ""classAttributes"": [
                    ""[Attribute1(\""admin,updateCatalog1\"")]"",
                    ""[Attribute2]""
                ],
                ""properties"": {
                    ""Name"": {
                        ""type"": ""string"",
                        ""description"": ""Name of the product.""
                    }
                },
                ""description"": ""This model describes all product-data that should rarely change.""
            }";

        var schema = await JsonSchema.FromJsonAsync(json);
        var generator = new CSharpGenerator(schema, new CSharpGeneratorSettings
        {
            SchemaType = SchemaType.Swagger2,
            ClassStyle = CSharpClassStyle.Poco
        });

        //// Act
        var code = generator.GenerateFile("MyClass");

        //// Assert
        Assert.Contains("[Attribute1(\"admin,updateCatalog1\")]", code);
        Assert.Contains("[Attribute2]", code);
    }
    
    [Fact]
    public async Task When_classAttributes_not_exits_dont_add_Attributes_to_Class()
    {
        var json =
            @"{
                ""title"": ""Product"",
                ""type"": ""object"",
                ""properties"": {
                    ""Name"": {
                        ""type"": ""string"",
                        ""description"": ""Name of the product.""
                    }
                },
                ""description"": ""This model describes all product-data that should rarely change.""
            }";

        var schema = await JsonSchema.FromJsonAsync(json);
        var generator = new CSharpGenerator(schema, new CSharpGeneratorSettings
        {
            SchemaType = SchemaType.Swagger2,
            ClassStyle = CSharpClassStyle.Poco
        });

        //// Act
        var code = generator.GenerateFile("MyClass");

        //// Assert
        Assert.DoesNotContain("[Attribute1(\"admin,updateCatalog1\")]", code);
    }
}