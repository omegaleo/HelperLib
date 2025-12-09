#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OmegaLeo.HelperLib.Game.UI
{
    public class UnityConsoleUI : MonoBehaviour
    {
        [SerializeField] private GameObject consolePanel;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI outputText;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private KeyCode toggleKey = KeyCode.BackQuote;

        private CommandConsole _console;
        private int _historyIndex = -1;

        private void Awake()
        {
            _console = new CommandConsole();
            _console.RegisterDefaultCommands();
            _console.OnOutput += AddOutput;
            _console.OnError += AddError;

            RegisterUnityCommands();
            consolePanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                ToggleConsole();
            }

            if (consolePanel.activeSelf && inputField.isFocused)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ExecuteInput();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    NavigateHistory(-1);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    NavigateHistory(1);
                }
                else if (Input.GetKeyDown(KeyCode.Tab))
                {
                    AutoComplete();
                }
            }
        }

        private void ToggleConsole()
        {
            consolePanel.SetActive(!consolePanel.activeSelf);
            if (consolePanel.activeSelf)
            {
                inputField.ActivateInputField();
            }
        }

        private void ExecuteInput()
        {
            string input = inputField.text;
            if (!string.IsNullOrWhiteSpace(input))
            {
                AddOutput($"> {input}");
                _console.ExecuteCommand(input);
                inputField.text = "";
                _historyIndex = -1;
            }
            inputField.ActivateInputField();
        }

        private void NavigateHistory(int direction)
        {
            if (_console.History.Count == 0) return;

            _historyIndex += direction;
            _historyIndex = Mathf.Clamp(_historyIndex, 0, _console.History.Count - 1);
            inputField.text = _console.History[_console.History.Count - 1 - _historyIndex];
            inputField.caretPosition = inputField.text.Length;
        }

        private void AutoComplete()
        {
            string[] suggestions = _console.GetAutoComplete(inputField.text);
            if (suggestions.Length == 1)
            {
                inputField.text = suggestions[0];
                inputField.caretPosition = inputField.text.Length;
            }
            else if (suggestions.Length > 1)
            {
                AddOutput("Suggestions: " + string.Join(", ", suggestions));
            }
        }

        private void AddOutput(string message)
        {
            outputText.text += message + "\n";
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }

        private void AddError(string message)
        {
            AddOutput($"<color=red>ERROR: {message}</color>");
        }

        private void RegisterUnityCommands()
        {
            _console.RegisterCommand("timescale", "Sets Time.timeScale", args =>
            {
                if (float.TryParse(args[0], out float scale))
                {
                    Time.timeScale = scale;
                    _console.Output($"Time scale set to {scale}");
                }
                else
                {
                    _console.Error("Invalid number");
                }
            }, 1, 1);

            _console.RegisterCommand("quit", "Quits the application", _ =>
            {
                Application.Quit();
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            });

            _console.RegisterCommand("scene", "Loads a scene by name", args =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(args[0]);
            }, 1, 1);
        }

        public CommandConsole Console => _console;
    }
}
#endif