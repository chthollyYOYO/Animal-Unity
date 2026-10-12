using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public class GameMonoSingleton : MonoSingleton<GameMonoSingleton>
    {
        public event UnityAction<float> onUpdate;
        public event UnityAction onLateUpdate;
        public event UnityAction onGUI;

        public float gameSpeed = 1f;
        public event UnityAction<float> onLogicUpdate;
        public event UnityAction<float> onLogicFixedUpdate;

        private void Update()
        {
            onUpdate?.Invoke(Time.deltaTime);
            onLogicUpdate?.Invoke(Time.deltaTime * gameSpeed);
        }

        private void LateUpdate()
        {
            onLateUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            onLogicFixedUpdate?.Invoke(Time.fixedDeltaTime * gameSpeed);
        }

        private void OnGUI()
        {
            onGUI?.Invoke();
        }
    }
}
