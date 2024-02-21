namespace ConsoleApp_Writers.Utils.Writers
{
    public class FileWriter : IWriter
    {
        private string path;

        public FileWriter(string path) => this.path = path;

        public void Write(object? data)
        {
            using (var writer = new StreamWriter(this.path)) {
                writer.Write(data!.ToString());
            }
        }
    }
}
