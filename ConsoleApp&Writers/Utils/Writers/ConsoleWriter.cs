namespace ConsoleApp_Writers.Utils.Writers
{
    public class ConsoleWriter : IWriter
    {
        public void Write(object? data)
        {
            Console.WriteLine(data!.ToString());
        }
    }
}
