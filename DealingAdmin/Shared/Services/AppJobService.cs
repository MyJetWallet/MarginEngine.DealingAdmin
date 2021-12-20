using DotNetCoreDecorators;
using System;
using System.Threading.Tasks;

namespace DealingAdmin.Shared.Services
{
    public static class AppJobService
    {
        private static readonly TaskTimer PriceUpdateTaskTimer
            = new TaskTimer(TimeSpan.FromSeconds(3));

        public static void Init()
        {
            Console.WriteLine($"{DateTime.Now} AppJobService Init");

            PriceUpdateTaskTimer.Register("Price Update", () =>
            {
                PriceUpdateEvent?.Invoke();
                return new ValueTask();
            });
        }

        public static void Start()
        {
            Console.WriteLine($"{DateTime.Now} AppJobService Start");
            PriceUpdateTaskTimer.Start();
        }

        public static event Action PriceUpdateEvent;
    }
}
