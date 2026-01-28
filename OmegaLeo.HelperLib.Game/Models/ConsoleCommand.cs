using System;

namespace OmegaLeo.HelperLib.Game.Models
{
    public class ConsoleCommand
    {
        public string Name { get; }
        public string Description { get; }
        public Action<string[]> Action { get; }
        public int MinArgs { get; }
        public int MaxArgs { get; }

        public ConsoleCommand(string name, string description, Action<string[]> action, int minArgs = 0, int maxArgs = -1)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? "No description";
            Action = action ?? throw new ArgumentNullException(nameof(action));
            MinArgs = minArgs;
            MaxArgs = maxArgs;
        }
    }
}