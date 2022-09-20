namespace TracerDemonstrationApp
{
    public class ResultWriter: IResultWriter
    {
        /// <summary>
        /// writes string to the stream but doesn't close it
        /// </summary>
        /// <param name="result"></param>
        /// <param name="stream"></param>
        public void Write(string result, TextWriter stream)
        {
            stream.WriteLine(result);
        }
        public void Write(string result, string fileName)
        {
            TextWriter streamWriter = new StreamWriter(fileName);
            this.Write(result, streamWriter);
            streamWriter.Close();
        }
    }
}
