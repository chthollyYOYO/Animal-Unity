using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        public UnityAction OnLoadingProcessChanged;
        public const float ANIM_TIME = 1f;

        public float LoadingProcess 
        {
            get
            {
                return loadingProcess;
            }
            set
            {
                if (value < loadingProcess)
                {
                    loadingProcess = value;
                    OnLoadingProcessChanged?.Invoke();
                }
                else
                {
                    GameMonoSingleton.Instance.onUpdate -= OnUpdate;
                    GameMonoSingleton.Instance.onUpdate += OnUpdate;
                    sourceProcess = loadingProcess;
                    targetProcess = value;
                    time = ANIM_TIME;
                    duration = 0;
                }
            }
        }

        private float loadingProcess;
        private float sourceProcess;
        private float targetProcess;
        private float time;
        private float duration;

        public async UniTask LoadingCompleted()
        {
            await UniTask.WaitUntil(() => duration >= time);
        }

        private void OnUpdate(float deltaTime)
        {
            duration += deltaTime;
            if (duration >= time)
            {
                duration = time;
                GameMonoSingleton.Instance.onUpdate -= OnUpdate;
            }
            loadingProcess = Mathf.Lerp(sourceProcess, targetProcess, duration / time);
            OnLoadingProcessChanged?.Invoke();
        }
    }
}
