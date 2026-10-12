using Framework;
using GameLogic;
using UnityEngine;

namespace UI
{
    public partial class UI_Panel_StallSystem : ICreate, IShow, IHide
    {
        private InputController_UI controller = new();

        private float gameSpeedCache;

        public void OnCreate()
        {
            controller.onClickClose += HideRecipe;
            btn_recipe.onClick.Add(ShowRecipe);
        }

        public void OnShow()
        {
            StallSystem.Instance.onTimeChanged += UpdateTimer;
            UltySystem.Instance.onUltyValueChanged += UpdateEnergy;
            PlayerController.Instance.onInputUpdate += OnPlayerInputUpdate;
            UpdateEnergy();
        }

        public void OnHide()
        {
            StallSystem.Instance.onTimeChanged -= UpdateTimer;
            UltySystem.Instance.onUltyValueChanged -= UpdateEnergy;
            PlayerController.Instance.onInputUpdate -= OnPlayerInputUpdate;
        }

        public void UpdateTimer()
        {
            if (TimerPanel.selectedIndex == 0)
                TimerPanel.selectedIndex = 1;

            var time = StallSystem.Instance.remainTime;
            int minute = (int)(time / 60);
            int second = (int)(time % 60);
            if (minute < 10)
                txt_timer.SetVar("min", "0" + minute.ToString()).FlushVars();
            else
                txt_timer.SetVar("min", minute.ToString()).FlushVars();
            if (second < 10)
                txt_timer.SetVar("sec", "0" + second.ToString()).FlushVars();
            else
                txt_timer.SetVar("sec", second.ToString()).FlushVars();

            if (minute == 0 && second == 0)
                TimerPanel.selectedIndex = 0;
        }

        public void UpdateEnergy()
        {
            ProgressBar_Ulty.max = UltySystem.Instance.ultyMax;
            ProgressBar_Ulty.value = UltySystem.Instance.ultyValue;
            ctrl_showR.SetSelectedBool(UltySystem.Instance.IsFull());
        }

        private void OnPlayerInputUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                AudioManager.Instance.PlaySound("Music/jiaohu/dakairiji");
                ShowRecipe();
            }
        }

        private void ShowRecipe()
        {
            ctrl_showRecipe.SetSelectedBool(true);
            InputSystem.Instance.RegisterController(controller);
            gameSpeedCache = GameMonoSingleton.Instance.gameSpeed;
            GameMonoSingleton.Instance.gameSpeed = 0;
        }

        private void HideRecipe()
        {
            ctrl_showRecipe.SetSelectedBool(false);
            InputSystem.Instance.UnRegisterController(controller);
            GameMonoSingleton.Instance.gameSpeed = gameSpeedCache;
        }
    }
}