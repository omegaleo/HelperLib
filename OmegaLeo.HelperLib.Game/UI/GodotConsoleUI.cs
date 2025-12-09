// File: `OmegaLeo.HelperLib.Game/Commands/GodotConsoleUI.cs`
#if GODOT
using Godot;
using System;

namespace OmegaLeo.HelperLib.Game.UI
{
    public partial class GodotConsoleUI : Control
    {
        private CommandConsole _console;
        private LineEdit _inputField;
        private RichTextLabel _outputText;
        private ScrollContainer _scrollContainer;
        private int _historyIndex = -1;

        public override void _Ready()
        {
            _console = new CommandConsole();
            _console.RegisterDefaultCommands();
            _console.OnOutput += AddOutput;
            _console.OnError += AddError;

            SetupUI();
            RegisterGodotCommands();
            Visible = false;
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey keyEvent && keyEvent.Pressed)
            {
                if (keyEvent.Keycode == Key.Quoteleft)
                {
                    ToggleConsole();
                    GetViewport().SetInputAsHandled();
                }
                else if (Visible && _inputField.HasFocus())
                {
                    if (keyEvent.Keycode == Key.Enter)
                    {
                        ExecuteInput();
                        GetViewport().SetInputAsHandled();
                    }
                    else if (keyEvent.Keycode == Key.Up)
                    {
                        NavigateHistory(-1);
                        GetViewport().SetInputAsHandled();
                    }
                    else if (keyEvent.Keycode == Key.Down)
                    {
                        NavigateHistory(1);
                        GetViewport().SetInputAsHandled();
                    }
                }
            }
        }

        private void SetupUI()
        {
            // Create a dark panel background
            var panel = new Panel();
            AddChild(panel);
            panel.SetAnchorsPreset(LayoutPreset.FullRect);

            var vbox = new VBoxContainer();
            AddChild(vbox);
            vbox.SetAnchorsPreset(LayoutPreset.FullRect);

            _scrollContainer = new ScrollContainer();
            _scrollContainer.SizeFlagsVertical = SizeFlags.ExpandFill;
            vbox.AddChild(_scrollContainer);

            _outputText = new RichTextLabel();
            _outputText.BbcodeEnabled = true;
            _outputText.ScrollFollowing = true;
            _scrollContainer.AddChild(_outputText);

            _inputField = new LineEdit();
            _inputField.PlaceholderText = "Enter command...";
            vbox.AddChild(_inputField);
        }

        private void ToggleConsole()
        {
            Visible = !Visible;
            if (Visible)
            {
                _inputField.GrabFocus();
            }
        }

        private void ExecuteInput()
        {
            string input = _inputField.Text;
            if (!string.IsNullOrWhiteSpace(input))
            {
                AddOutput($"> {input}");
                _console.ExecuteCommand(input);
                _inputField.Text = "";
                _historyIndex = -1;
            }
            _inputField.GrabFocus();
        }

        private void NavigateHistory(int direction)
        {
            if (_console.History.Count == 0) return;

            _historyIndex = Mathf.Clamp(_historyIndex + direction, 0, _console.History.Count - 1);
            _inputField.Text = _console.History[_console.History.Count - 1 - _historyIndex];
            _inputField.CaretColumn = _inputField.Text.Length;
        }

        private void AddOutput(string message)
        {
            _outputText.Text += message + "\n";
        }

        private void AddError(string message)
        {
            AddOutput($"[color=red]ERROR: {message}[/color]");
        }

        private void RegisterGodotCommands()
        {
            _console.RegisterCommand("timescale", "Sets Engine.TimeScale", args =>
            {
                if (float.TryParse(args[0], out float scale))
                {
                    Engine.TimeScale = scale;
                    _console.Output($"Time scale set to {scale}");
                }
                else
                {
                    _console.Error("Invalid number");
                }
            }, 1, 1);

            _console.RegisterCommand("quit", "Quits the application", _ =>
            {
                GetTree().Quit();
            });

            _console.RegisterCommand("scene", "Changes to a scene", args =>
            {
                GetTree().ChangeSceneToFile(args[0]);
            }, 1, 1);
        }

        public CommandConsole Console => _console;
    }
}
#endif
