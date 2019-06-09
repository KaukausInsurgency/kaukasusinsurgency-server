using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace TAWKI_TCPServer.Implementations
{
    class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {
        }

        void ILogger.Log(string data)
        {
            Console.WriteLine(data);
        }
    }
}
