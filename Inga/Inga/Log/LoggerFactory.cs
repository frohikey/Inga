using System;
using Inga.Configuration;

namespace Inga.Log
{
    public static class LoggerFactory
    {
        public static ILogger Create(IngaSection section)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            switch (section.LogType)
            {
                case Enums.LoggerType.SimpleLogFile:
                    return new SimpleLogFileLogger(section.LogFile);

                default:
                    throw new Exception($"Logger of type {section.LogType} cannot be created.");
            }
        }
    }
}
