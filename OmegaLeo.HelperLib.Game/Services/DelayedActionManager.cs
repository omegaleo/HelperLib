using System;
using System.Collections.Generic;
using NetFlow.DocumentationHelper.Library.Attributes;
using OmegaLeo.HelperLib.Game.Handlers;
using OmegaLeo.HelperLib.Game.Models;

namespace OmegaLeo.HelperLib.Game.Services
{
    public class DelayedActionManager
    {
        private readonly List<DelayedAction> _pendingActions = new List<DelayedAction>();
        private readonly List<DelayedAction> _actionsToRemove = new List<DelayedAction>();

        [Documentation(nameof(RunAfter), "Schedules an action to run after a specified delay.")]
        public void RunAfter(float delaySeconds, Action action)
        {
            _pendingActions.Add(new DelayedAction(delaySeconds, action, false));
        }

        [Documentation(nameof(RunRepeating), "Schedules an action to run repeatedly at specified intervals.")]
        public void RunRepeating(float intervalSeconds, Action action)
        {
            _pendingActions.Add(new DelayedAction(intervalSeconds, action, true));
        }

        [Documentation(nameof(RunAfterCancellable), "Schedules an action to run after a delay and returns a handler to cancel it if needed.")]
        public DelayedActionHandler RunAfterCancellable(float delaySeconds, Action action)
        {
            var delayedAction = new DelayedAction(delaySeconds, action, false);
            _pendingActions.Add(delayedAction);
            return new DelayedActionHandler(delayedAction);
        }

        public void Update(float deltaTime)
        {
            _actionsToRemove.Clear();

            foreach (var action in _pendingActions)
            {
                action.TimeRemaining -= deltaTime;

                if (action.TimeRemaining <= 0)
                {
                    if (!action.IsCancelled)
                        action.Action?.Invoke();

                    if (action.IsRepeating && !action.IsCancelled)
                    {
                        action.TimeRemaining = action.InitialDelay;
                    }
                    else
                    {
                        _actionsToRemove.Add(action);
                    }
                }
            }

            foreach (var action in _actionsToRemove)
            {
                _pendingActions.Remove(action);
            }
        }

        [Documentation(nameof(CancelAll), "Cancels all pending delayed actions.")]
        public void CancelAll()
        {
            _pendingActions.Clear();
        }

        [Documentation(nameof(PendingActionsCount), "Gets the count of pending delayed actions.")]
        public int PendingActionsCount => _pendingActions.Count;
    }
}