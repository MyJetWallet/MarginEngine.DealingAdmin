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
            LiveDemoModeChanged?.Invoke();
        }

        public void SetDemo()
        {
            IsLive = false;
            LiveDemoModeChanged?.Invoke();
        }

        public event Action LiveDemoModeChanged;
    }
}
