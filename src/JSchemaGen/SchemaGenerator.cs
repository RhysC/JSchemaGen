using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Schema;

namespace JSchemaGen
{
    public class SchemaGenerator<T> where T : ObjectDefinition, new()
    {
        public List<T> ObjectDefinitions { get; private set; }
        public SchemaGenerator(JSchema schema)
        {
            ObjectDefinitions = new List<T>();
            ConvertJsonSchemaToModel(schema);
        }

        private void ConvertJsonSchemaToModel(JSchema schema)
        {
            if (schema.Type == null)
                throw new Exception("Schema does not specify a type.");

            switch (schema.Type)
            {
                case JSchemaType.Object:
                    CreateTypeFromSchema(schema, "root");
                    break;

                case JSchemaType.Array:
                    foreach (var item in schema.Items.Where(x => x.Type.HasValue && x.Type == JSchemaType.Object))
                    {
                        CreateTypeFromSchema(item);
                    }
                    break;
            }
        }

        private T CreateTypeFromSchema(JSchema schema, string name = null)
        {
            var def = new T
            {
                Name = name ?? String.Format("Poco_{0}", Guid.NewGuid().ToString().Replace("-", String.Empty)),
                Properties = schema.Properties.ToDictionary(item => item.Key.Trim(), item => GetTypeFromSchema(schema, item.Value, item.Key))
            };

            ObjectDefinitions.Add(def);
            return def;
        }

        private string GetTypeFromSchema(JSchema parent, JSchema jsonSchema, string name = null)
        {
            if (jsonSchema.Type != JSchemaType.Object)
                return new T().GetTypeFromSchema(parent, jsonSchema, name);
            var def = CreateTypeFromSchema(jsonSchema, name);
            return def.Name;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var objectDefinition in ObjectDefinitions)
            {
                sb.AppendLine(objectDefinition.ToString());
            }
            return sb.ToString();
        }
    }
}
