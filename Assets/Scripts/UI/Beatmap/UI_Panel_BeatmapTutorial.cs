using System;
using Framework;

namespace UI
{
    public struct BeatmapTutorialParam
    {
        public Action OnConfirm;
    }

    public partial class UI_Panel_BeatmapTutorial: IShow<BeatmapTutorialParam>,ICreate
    {
        private Action confirm;
        public void OnCreate()
        {
            btn_close.onClick.Add(Hide);
        }

        public void OnShow(BeatmapTutorialParam param)
        {
            confirm = param.OnConfirm;
        }

        private void Hide()
        {
            confirm?.Invoke();
            UIManager.Instance.HidePanel<UI_Panel_BeatmapTutorial>();

        }
    }
}
