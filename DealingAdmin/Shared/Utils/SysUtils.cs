using System;

namespace DealingAdmin
{
    public static class SysUtils
    {
        public static void CollectGarbage()
        {
            try
            {
                Console.WriteLine(
                    "CollectGarbage - Memory used before GC: {0:N0} MB",
                    GC.GetTotalMemory(false) / (1024 * 1024));

                GC.Collect();

                Console.WriteLine(
                   "CollectGarbage - Memory used after full GC: {0:N0} MB",
                    GC.GetTotalMemory(true) / (1024 * 1024));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("CollectGarbage - failed to run garbage collection");
            }
        }
    }
}