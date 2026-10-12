using Cysharp.Threading.Tasks;
using Framework;
using System;
using UI;

namespace GameLogic
{
    public enum SceneName
    {
        旧街场景,
        经营场景,
    }

    public partial class PlayerController
    {
        public event Action onSceneChanged;
        public event Action onSceneMoveStart;
        public event Action onSceneMoveEnd;

        public SceneName currentScene
        {
            get => _currentScene;
            set
            {
                if (_currentScene != value)
                {
                    _currentScene = value;
                    onSceneChanged?.Invoke();
                }
            }
        }
        private SceneName _currentScene;

        private void InitScene()
        {
            currentScene = SceneName.旧街场景;
        }

        public async void MoveToStallScene()
        {
            InputSystem.Instance.enabled.SetValue("PlayerTransfer", false);
            onSceneMoveStart?.Invoke();

            bool isCompleted = false;
            UIManager.Instance.ShowPanelSync<UI_Panel_Anim_Moving, Action>(() => isCompleted = true);
            await UniTask.WaitUntil(() => isCompleted);
            UIManager.Instance.HidePanel<UI_Panel_Anim_Moving>();

            InputSystem.Instance.enabled.SetValue("PlayerTransfer", true);

            transform.position = SceneFlagManager.Instance.GetFlag(SceneFlagType.摊位_传送点).transform.position;
            //Helper.SetMainCameraEnable(CameraManager.Instance.StreetCamera, false);
            //Helper.SetMainCameraEnable(CameraManager.Instance.StallCamera, true);
            Helper.ChangeMainCamera(CameraManager.Instance.StallCamera);

            currentScene = SceneName.经营场景;
            onSceneMoveEnd?.Invoke();
            UIManager.Instance.ShowPanelSync<UI_Panel_StallSystem>();
        }

        public async void MoveToShopScene()
        {
            InputSystem.Instance.enabled.SetValue("PlayerTransfer", false);
            onSceneMoveStart?.Invoke();

            bool isCompleted = false;
            UIManager.Instance.ShowPanelSync<UI_Panel_Anim_Moving, Action>(() => isCompleted = true);
            await UniTask.WaitUntil(() => isCompleted);
            UIManager.Instance.HidePanel<UI_Panel_Anim_Moving>();

            InputSystem.Instance.enabled.SetValue("PlayerTransfer", true);

            transform.position = SceneFlagManager.Instance.GetFlag(SceneFlagType.房车_传送点).transform.position;
            //Helper.SetMainCameraEnable(CameraManager.Instance.StallCamera, false);
            //Helper.SetMainCameraEnable(CameraManager.Instance.StreetCamera, true);
            Helper.ChangeMainCamera(CameraManager.Instance.StreetCamera);

            currentScene = SceneName.旧街场景;
            onSceneMoveEnd?.Invoke();
            UIManager.Instance.HidePanel<UI_Panel_StallSystem>();
        }

        public void MoveStreet0ToStreet1()
        {
            transform.position = SceneFlagManager.Instance.GetFlag(SceneFlagType.街道0_街道1_传送到达点).transform.position;
            //Helper.SetMainCameraEnable(CameraManager.Instance.StreetCamera, false);
            //Helper.SetMainCameraEnable(CameraManager.Instance.StreetCamera1, true);
            Helper.ChangeMainCamera(CameraManager.Instance.StreetCamera1);
        }

        public void MoveStreet1ToStreet0()
        {
            transform.position = SceneFlagManager.Instance.GetFlag(SceneFlagType.街道1_街道0_传送到达点).transform.position;
            //Helper.SetMainCameraEnable(CameraManager.Instance.StreetCamera1, false);
            //Helper.SetMainCameraEnable(CameraManager.Instance.StreetCamera, true);
            Helper.ChangeMainCamera(CameraManager.Instance.StreetCamera);
        }
    }
}