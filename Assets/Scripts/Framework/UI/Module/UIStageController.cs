using FairyGUI;

namespace Framework
{
    public class UIStageController : Singleton<UIStageController>
    {
        public const int stageOffsetX = -300;

        public void Initialize()
        {
            GRoot.inst.SetContentScaleFactor(UISystem.UI_DESIGN_WIDTH, UISystem.UI_DESIGN_HEIGHT);
            Stage.inst.onStageResized.Add(OnStageResized);
            UpdateStage();
        }

        private void OnStageResized()
        {
            UpdateStage();
        }

        private void UpdateStage()
        {
            Stage.inst.gameObject.transform.position = new(stageOffsetX, 0, 0);
        }
    }
}
