using System;

namespace EasterEggs
{
    public interface IEasterEgg
    {
        public event Action Detonate;
        public bool EnableEsterEgg { get; set; }
    }
}