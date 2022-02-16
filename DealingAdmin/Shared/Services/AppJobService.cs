using DotNetCoreDecorators;
using System;
using System.Threading.Tasks;

namespace DealingAdmin.Shared.Services
{
    public static class AppJobService
    {
        private static readonly TaskTimer PriceUpdateTaskTimer
            = new TaskTimer(TimeSpan.FromSeconds(3));

        private static readonly TaskTimer QuotesUpdateTaskTimer
            = new TaskTimer(TimeSpan.FromMilliseconds(400));

        private static readonly TaskTimer CommonUpdateTaskTimer
            = new TaskTimer(TimeSpan.FromMinutes(10));

        public static void Init()
        {
            Console.WriteLine($"{DateTime.Now} AppJobService Init");

            PriceUpdateTaskTimer.Register("Price Update", () =>
            {
                PriceUpdateEvent?.Invoke();
                return new ValueTask();
            });

            QuotesUpdateTaskTimer.Register("Quotes Update", () =>
            {
                QuotesUpdateEvent?.Invoke();
                return new ValueTask();
            });


            CommonUpdateTaskTimer.Register("Common Update", () =>
            {
                CommonUpdateEvent?.Invoke();
                SysUtils.CollectGarbage();
                return new ValueTask();
            });
        }

        public static void Start()
        {
            Console.WriteLine($"{DateTime.Now} AppJobService Start");
            PriceUpdateTaskTimer.Start();
            QuotesUpdateTaskTimer.Start();
        }

        public static event Action PriceUpdateEvent;

        public static event Action QuotesUpdateEvent;

        public static event Action CommonUpdateEvent;
    }
}
