using Cysharp.Threading.Tasks;
using Framework;
using System;
using UnityEngine;

namespace GameLogic
{
    public class GuideSystem : Singleton<GuideSystem>
    {
        public event Action onGuideIdChanged;
        public int guideId
        {
            get => _guideId;
            set
            {
                _guideId = value;
                onGuideIdChanged?.Invoke();
            }
        }
        private int _guideId;

        public void Initialize()
        {
            PlayerController.Instance.onSceneChanged += OnSceneChanged;
        }

        public void Destroy()
        {
            PlayerController.Instance.onSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged()
        {
            if (PlayerController.Instance.currentScene == SceneName.经营场景)
            {
                if (!Record.current.IsCompletedDialogue(1007))
                {
                    Process1();
                }
            }
        }

        private async void Process1()
        {
            guideId = 1;
            InputSystem.Instance.enabled.SetValue("Guide", false);
            StallSystem.Instance.InitTables();
            StallSystem.Instance.isStalling = true;

            var diner = StallSystem.Instance.CreateDiner(int.MaxValue, 308);
            diner.scale = 0.6f;
            diner.gameObject.transform.localScale = Vector3.one * 0.6f;
            await UniTask.WaitUntil(() => diner.currAction == DinerAction.候座);

            InputSystem.Instance.enabled.SetValue("Guide", true);
            DialogueSystem.Instance.StartDialogue(1007);

            await UniTask.WaitUntil(() => diner.currAction == DinerAction.离开);

            diner.isPause = true;
            await DialogueSystem.Instance.StartDialogueTask(1008);

            diner.isPause = false;
            StallSystem.Instance.isStalling = false;
            guideId = 0;
        }
    }
}
