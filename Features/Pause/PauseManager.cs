using System.Collections.Generic;

namespace Pause
{
    public class PauseManager : IPauseHandler
    {
        public bool IsPaused { get; private set; }
        private readonly List<IPauseHandler> m_Handlers = new();

        public void SetPaused(bool isPaused)
        {
            IsPaused = isPaused;
            foreach (var handler in m_Handlers)
            {
                handler.SetPaused(isPaused);
            }
        }

        public void Register(IPauseHandler handler) => m_Handlers.Add(handler);
        public void UnRegister(IPauseHandler handler) => m_Handlers.Remove(handler);
    }
}