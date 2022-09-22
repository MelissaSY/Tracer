namespace TracerDemonstrationApp
{
    public class ResultWriter: IResultWriter
    {
        /// <summary>
        /// writes string to the stream
        /// </summary>
        /// <param name="result"></param>
        /// <param name="stream"></param>
        public void Write(string result, TextWriter stream)
        {
            stream.WriteLine(result);
        }
    }
}
