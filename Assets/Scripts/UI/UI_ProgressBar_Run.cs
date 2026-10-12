using Framework;

namespace UI
{
    public partial class UI_ProgressBar_Run
    {
        public void Update(float value)
        {
            prog.max = ConfigManager.GetGameCfg(0).EpMax;
            prog.value = value;
        }
    }
}
