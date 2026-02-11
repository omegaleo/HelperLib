namespace OmegaLeo.HelperLib.Game.Providers
{
    #if GODOT
    public partial class GodotDelayedActionRunner : Node
    {
        private static GodotDelayedActionRunner _instance;
        public static DelayedActionManager Manager { get; private set; }

        public override void _Ready()
        {
            if (_instance == null)
            {
                _instance = this;
                Manager = new DelayedActionManager();
            }
        }

        public override void _Process(double delta)
        {
            Manager?.Update((float)delta);
        }

        public override void _ExitTree()
        {
            if (_instance == this)
            {
                Manager?.CancelAll();
                _instance = null;
            }
        }
    }
    #endif
}