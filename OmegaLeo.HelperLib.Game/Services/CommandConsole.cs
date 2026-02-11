using System;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.HelperLib.Shared.Attributes;
using OmegaLeo.HelperLib.Game.Models;

namespace OmegaLeo.HelperLib.Game.Services
{
    public class CommandConsole
    {
        private readonly Dictionary<string, ConsoleCommand> _commands = new Dictionary<string, ConsoleCommand>(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> _history = new List<string>();
        private int _maxHistorySize = 100;

        public event Action<string> OnOutput;
        public event Action<string> OnError;

        public IReadOnlyList<string> History => _history.AsReadOnly();
        public IReadOnlyDictionary<string, ConsoleCommand> Commands => _commands;

        [Documentation(nameof(RegisterCommand), "Registers a new command with the console.")]
        public void RegisterCommand(string name, string description, Action<string[]> action, int minArgs = 0, int maxArgs = -1)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Command name cannot be empty", nameof(name));

            var command = new ConsoleCommand(name, description, action, minArgs, maxArgs);
            _commands[name] = command;
        }

        [Documentation(nameof(RegisterCommand), "Registers a new command with the console using a delegate.")]
        public void RegisterCommand(string name, string description, Func<string[], string> func, int minArgs = 0, int maxArgs = -1)
        {
            RegisterCommand(name, description, args =>
            {
                string result = func(args);
                if (!string.IsNullOrEmpty(result))
                    Output(result);
            }, minArgs, maxArgs);
        }

        [Documentation(nameof(UnregisterCommand), "Removes a command from the console.")]
        public bool UnregisterCommand(string name)
        {
            return _commands.Remove(name);
        }

        [Documentation(nameof(ExecuteCommand), "Executes a command string.")]
        public bool ExecuteCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            _history.Add(input);
            if (_history.Count > _maxHistorySize)
                _history.RemoveAt(0);

            string[] parts = ParseCommandLine(input);
            if (parts.Length == 0)
                return false;

            string commandName = parts[0];
            string[] args = parts.Skip(1).ToArray();

            if (!_commands.TryGetValue(commandName, out var command))
            {
                Error($"Unknown command: {commandName}");
                return false;
            }

            if (command.MinArgs > 0 && args.Length < command.MinArgs)
            {
                Error($"Command '{commandName}' requires at least {command.MinArgs} argument(s)");
                return false;
            }

            if (command.MaxArgs >= 0 && args.Length > command.MaxArgs)
            {
                Error($"Command '{commandName}' accepts at most {command.MaxArgs} argument(s)");
                return false;
            }

            try
            {
                command.Action(args);
                return true;
            }
            catch (Exception ex)
            {
                Error($"Error executing command '{commandName}': {ex.Message}");
                return false;
            }
        }

        [Documentation(nameof(Output), "Outputs a message to the console.")]
        public void Output(string message)
        {
            OnOutput?.Invoke(message);
        }

        [Documentation(nameof(Error), "Outputs an error message to the console.")]
        public void Error(string message)
        {
            OnError?.Invoke(message);
        }

        [Documentation(nameof(ClearHistory), "Clears command history.")]
        public void ClearHistory()
        {
            _history.Clear();
        }

        [Documentation(nameof(GetAutoComplete), "Gets autocomplete suggestions for partial input.")]
        public string[] GetAutoComplete(string partial)
        {
            if (string.IsNullOrWhiteSpace(partial))
                return _commands.Keys.ToArray();

            return _commands.Keys
                .Where(cmd => cmd.StartsWith(partial, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        private string[] ParseCommandLine(string input)
        {
            var result = new List<string>();
            bool inQuotes = false;
            string current = "";

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (char.IsWhiteSpace(c) && !inQuotes)
                {
                    if (current.Length > 0)
                    {
                        result.Add(current);
                        current = "";
                    }
                }
                else
                {
                    current += c;
                }
            }

            if (current.Length > 0)
                result.Add(current);

            return result.ToArray();
        }

        [Documentation(nameof(RegisterDefaultCommands), "Registers built-in commands.")]
        public void RegisterDefaultCommands()
        {
            RegisterCommand("help", "Lists all available commands", args =>
            {
                if (args.Length == 0)
                {
                    Output("Available commands:");
                    foreach (var cmd in _commands.Values.OrderBy(c => c.Name))
                    {
                        Output($"  {cmd.Name} - {cmd.Description}");
                    }
                }
                else
                {
                    string cmdName = args[0];
                    if (_commands.TryGetValue(cmdName, out var cmd))
                    {
                        Output($"{cmd.Name}: {cmd.Description}");
                        Output($"Min args: {cmd.MinArgs}, Max args: {(cmd.MaxArgs >= 0 ? cmd.MaxArgs.ToString() : "unlimited")}");
                    }
                    else
                    {
                        Error($"Unknown command: {cmdName}");
                    }
                }
            }, 0, 1);

            RegisterCommand("clear", "Clears command history", _ => ClearHistory());

            RegisterCommand("echo", "Echoes back the input", args =>
            {
                Output(string.Join(" ", args));
            }, 1);

            RegisterCommand("commands", "Lists all registered commands", _ =>
            {
                Output($"Total commands: {_commands.Count}");
                Output(string.Join(", ", _commands.Keys.OrderBy(k => k)));
            });
        }
    }
}