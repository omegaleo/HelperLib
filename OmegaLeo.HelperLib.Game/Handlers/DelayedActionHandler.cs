using OmegaLeo.HelperLib.Game.Models;

namespace OmegaLeo.HelperLib.Game.Handlers
{
    public class DelayedActionHandler
    {
        private readonly DelayedAction _action;

        internal DelayedActionHandler(DelayedAction action)
        {
            _action = action;
        }

        public void Cancel()
        {
            if (_action != null)
                _action.IsCancelled = true;
        }

        public bool IsCancelled => _action?.IsCancelled ?? true;
    }
}