using System.IO;
using JSchemaGen.Implementations;
using Newtonsoft.Json.Schema;

namespace JSchemaGen.Console
{
    class Program
    {
        static void Main()
        {
            string schemaText;
            using (var r = new StreamReader(@"C:\Users\rhys.campbell\Desktop\MercurySubmissionSchema.txt"))
            {
                schemaText = r.ReadToEnd();
            }
            var jsonSchema = JSchema.Parse(schemaText);

            if (jsonSchema != null)
            {
                var def = new SchemaGenerator<KnockOutJsObjectDefinition>(jsonSchema);
                var code = def.ToString();
                System.Console.WriteLine(code);
            }
            System.Console.ReadLine();
        }
    }
}
