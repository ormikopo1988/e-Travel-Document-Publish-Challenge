using System;
using System.Collections.Generic;

namespace ETravel.Common
{
    public class LoggingService
    {
        public static void WriteToConsole(List<ILoggable> changedItems)
        {
            foreach (var item in changedItems)
            {
                Console.WriteLine(item.Log());
            }
        }
    }
}
