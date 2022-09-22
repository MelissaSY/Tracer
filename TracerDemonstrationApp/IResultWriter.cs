using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracerDemonstrationApp
{
    public interface IResultWriter
    {
        /// <summary>
        /// Writes the string to the stream provided
        /// </summary>
        /// <param name="result"></param>
        /// <param name="stream"></param>
        public void Write(string result, TextWriter stream);
    }
}
