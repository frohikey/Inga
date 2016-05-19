using System;

namespace Inga.Log
{
    public interface ILogger
    {
        void Log(string text);
        void Log(string text, Exception ex);
    }
}
