using SqlDoctor.Schema;

namespace SqlDoctor.Generator
{
    public interface IDocGenerator
    {
        string Generate(SchemaInfo schema, Options options); 
    }
}