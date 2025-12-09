namespace OmegaLeo.HelperLib.Game.Providers
{
#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;

namespace OmegaLeo.HelperLib.DelayedActions
{
    public class UnityDelayedActionRunner : MonoBehaviour
    {
        private static UnityDelayedActionRunner _instance;
        public static DelayedActionManager Manager { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_instance == null)
            {
                var go = new GameObject("[DelayedActionRunner]");
                _instance = go.AddComponent<UnityDelayedActionRunner>();
                DontDestroyOnLoad(go);
                Manager = new DelayedActionManager();
            }
        }

        private void Update()
        {
            Manager?.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                Manager?.CancelAll();
                _instance = null;
            }
        }
    }
}
#endif
}