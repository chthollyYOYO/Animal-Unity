using Cysharp.Threading.Tasks;
using Framework;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    public class GameStart : MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance.ShowPanelSync<UI_Panel_Start>();
        }

        public static async void EnterGame(Record record)
        {
            LoadingManager.Instance.LoadingProcess = 0f;
            UIManager.Instance.ShowPanelSync<UI_Panel_Loading>();

            await EnterGameInternal(record);

            LoadingManager.Instance.LoadingProcess = 1;
            await LoadingManager.Instance.LoadingCompleted();
            UIManager.Instance.HidePanel<UI_Panel_Loading>();
        }

        public static async UniTask EnterGameInternal(Record record)
        {
            Record.current = record;
            await SceneManager.LoadSceneAsync("Main", LoadSceneMode.Single);

            //Init
            PointManager.Instance.Initialize();
            InteractableTipManager.Instance.Init();
            BgmManager.Instance.Initialize();
            GuideSystem.Instance.Initialize();
            UIManager.Instance.ShowPanelSync<UI_Panel_Main>();
        }

        public static async void ExitGame()
        {
            LoadingManager.Instance.LoadingProcess = 0f;
            UIManager.Instance.ShowPanelSync<UI_Panel_Loading>();

            Record.current = null;
            //Destroy
            InteractableTipManager.Instance.Destroy();
            BgmManager.Instance.Destroy();
            GuideSystem.Instance.Destroy();
            await SceneManager.LoadSceneAsync("Start", LoadSceneMode.Single);

            LoadingManager.Instance.LoadingProcess = 1;
            await LoadingManager.Instance.LoadingCompleted();
            UIManager.Instance.HidePanel<UI_Panel_Loading>();
        }
    }
}