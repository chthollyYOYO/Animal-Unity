/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_BeatmapTutorial : GComponent
    {
        public GButton btn_close;
        public const string URL = "ui://jnnt3xizei0px4u";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Panel_BeatmapTutorial";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_BeatmapTutorial));
            UIRegisterInfoManager.Instance.Register<UI_Panel_BeatmapTutorial>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_close = (GButton)GetChild("btn_close");
        }
    }
}