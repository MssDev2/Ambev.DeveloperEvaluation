using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var enumType = context.Type;
            var enumValues = Enum.GetValues(enumType).Cast<Enum>();

            schema.Enum.Clear();
            foreach (var enumValue in enumValues)
            {
                var memberInfo = enumType.GetMember(enumValue.ToString()).FirstOrDefault();
                var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();
                var description = displayAttribute?.Description ?? enumValue.ToString();
                schema.Enum.Add(new OpenApiString($"{Convert.ToInt32(enumValue)}:{enumValue} - {description}"));
            }
        }
    }
}
