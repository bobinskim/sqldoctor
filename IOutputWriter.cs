namespace SqlDoctor
{
    public interface IOutputWriter
    {
        void WriteOutput(string output, Options options);
    }
}