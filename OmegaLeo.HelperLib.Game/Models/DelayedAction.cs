using System;

namespace OmegaLeo.HelperLib.Game.Models
{
    internal class DelayedAction
    {
        public float TimeRemaining { get; set; }
        public float InitialDelay { get; }
        public Action Action { get; }
        public bool IsRepeating { get; }
        public bool IsCancelled { get; set; }

        public DelayedAction(float delay, Action action, bool isRepeating)
        {
            TimeRemaining = delay;
            InitialDelay = delay;
            Action = action;
            IsRepeating = isRepeating;
            IsCancelled = false;
        }
    }
}