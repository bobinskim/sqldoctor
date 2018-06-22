namespace SqlDoctor.Generator
{
    public interface IOutputWriter
    {
        void WriteOutput(string output, Options options);
    }
}