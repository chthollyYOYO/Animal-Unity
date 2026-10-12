/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_StallSystem : GComponent
    {
        public Controller TimerPanel;
        public Controller ctrl_showR;
        public Controller ctrl_showRecipe;
        public GTextField txt_timer;
        public GProgressBar ProgressBar_Ulty;
        public GButton btn_recipe;
        public const string URL = "ui://ywqkltiaeda1x3q";
        public const string PKG_NAME = "Stall";
        public const string RES_NAME = "Panel_StallSystem";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_StallSystem));
            UIRegisterInfoManager.Instance.Register<UI_Panel_StallSystem>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            TimerPanel = GetController("TimerPanel");
            ctrl_showR = GetController("ctrl_showR");
            ctrl_showRecipe = GetController("ctrl_showRecipe");
            txt_timer = (GTextField)GetChild("txt_timer");
            ProgressBar_Ulty = (GProgressBar)GetChild("ProgressBar_Ulty");
            btn_recipe = (GButton)GetChild("btn_recipe");
        }
    }
}