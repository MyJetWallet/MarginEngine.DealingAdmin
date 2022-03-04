using System;

namespace DealingAdmin.Shared.Services
{
    public class StateManager
    {
        public bool IsLive { get; private set; }

        public StateManager()
        {
            IsLive = true;
        }

        public void SetLive()
        {
            IsLive = true;
            Console.WriteLine($"LiveDemo -> SetLive");

            try
            {
                LiveDemoModeChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LiveDemoModeChanged Error: {ex.Message}");
            }
        }

        public void SetDemo()
        {
            IsLive = false;
            Console.WriteLine($"LiveDemo -> SetDemo");

            try
            {
                LiveDemoModeChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LiveDemoModeChanged Error: {ex.Message}");
            }
        }

        public event Action LiveDemoModeChanged;
    }
}
