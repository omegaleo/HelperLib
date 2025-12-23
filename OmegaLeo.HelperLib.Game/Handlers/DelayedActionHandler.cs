using NetFlow.DocumentationHelper.Library.Attributes;
using OmegaLeo.HelperLib.Game.Models;

namespace OmegaLeo.HelperLib.Game.Handlers
{
    [Documentation(nameof(DelayedActionHandler), "Handler for managing delayed actions, allowing cancellation and status checking.", null, @"```csharp
var action = new DelayedAction(...);
var handler = new DelayedActionHandler(action);
```")]
    public class DelayedActionHandler
    {
        private readonly DelayedAction _action;

        internal DelayedActionHandler(DelayedAction action)
        {
            _action = action;
        }

        [Documentation(nameof(Cancel), "Cancels the delayed action if it has not yet executed.", null, @"```csharp
handler.Cancel();
```")]
        public void Cancel()
        {
            if (_action != null)
                _action.IsCancelled = true;
        }

        public bool IsCancelled => _action?.IsCancelled ?? true;
    }
}