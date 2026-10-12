using Cysharp.Threading.Tasks;
using FairyGUI;
using Framework;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    public class GameInit : MonoBehaviour
    {
        private async void Awake()
        {
            //初始化
            Application.targetFrameRate = 120;
            UISystem.Instance.AddCommonPackage("Main");
            UISystem.Instance.AddCommonPackage("Item");
            UISystem.Instance.AddCommonPackage("Stall");
            UISystem.Instance.AddCommonPackage("Dialogue");
            UISystem.Instance.AddCommonPackage("Vegetable");
            UISystem.Instance.AddCommonPackage("Bookstore");
            UISystem.Instance.AddCommonPackage("Gift");
            await UISystem.Instance.Initialize();
            StageCamera.main.backgroundColor = Color.black;

            //显示加载界面
            LoadingManager.Instance.LoadingProcess = 0f;
            UIManager.Instance.ShowPanelSync<UI_Panel_Loading>();

            //初始化
            await ConfigManager.Initialize();
            InputSystem.Instance.Initialize();
            RecipeSystem.Instance.Initialize();
            SceneUIManager.Instance.Initialize();
            DialogueSystem.Instance.Initialize();
            AudioManager.Instance.Initialize();

            bool hasRecord = RecordManager.Instance.ReadAll().Count > 0;
            hasRecord = false; //测试用代码
            if (hasRecord)
            {
                await SceneManager.LoadSceneAsync("Start", LoadSceneMode.Single);
            }
            else
            {
                var record = RecordManager.Instance.Create();
                await GameStart.EnterGameInternal(record);
            }

            //隐藏加载界面
            LoadingManager.Instance.LoadingProcess = 1;
            await LoadingManager.Instance.LoadingCompleted();
            UIManager.Instance.HidePanel<UI_Panel_Loading>();

            if (!hasRecord)
            {
                DialogueSystem.Instance.StartDialogue(1001);
            }
        }
    }
}
