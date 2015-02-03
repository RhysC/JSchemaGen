using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Schema;

namespace JSchemaGen.Implementations
{
    public class KnockOutJsObjectDefinition : ObjectDefinition
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("var ");
            sb.Append(Name);
            sb.AppendLine(" = function() {");
            sb.AppendLine("var self = this;");

            foreach (var item in Properties)
            {
                sb.Append("self.");
                sb.Append(item.Key);
                sb.Append(item.Value);
                sb.AppendLine(";");
            }

            sb.AppendLine("};");
            return sb.ToString();
        }

        public override string GetTypeFromSchema(JSchema parent, JSchema jsonSchema, string name = null)
        {
            var str = " = ko.observable()";
            if (parent.Required.Any(r => r == name))
            {
                str += ".extend({ required: true })";
            }
            switch (jsonSchema.Type)
            {
                case JSchemaType.Array:
                    return " = ko.observableArray()";

                case JSchemaType.Float:
                    str += ".extend({ number: true })";
                    break;

                case JSchemaType.Integer:
                    str += ".extend({ digit: true })";
                    break;

                case JSchemaType.Object:
                    throw new ArgumentException("Object type should be mnage by the base type");
            }
            return str;
        }
    }
}
