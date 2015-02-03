using System.Collections.Generic;
using Newtonsoft.Json.Schema;

namespace JSchemaGen
{
    public abstract class ObjectDefinition
    {
        public string Name { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public abstract string GetTypeFromSchema(JSchema parent, JSchema jsonSchema, string name = null);
    }
}