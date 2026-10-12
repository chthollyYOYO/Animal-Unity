/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_StallEnd : GComponent
    {
        public GTextField txt;
        public const string URL = "ui://ywqkltiamftvx3v";
        public const string PKG_NAME = "Stall";
        public const string RES_NAME = "Panel_StallEnd";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_StallEnd));
            UIRegisterInfoManager.Instance.Register<UI_Panel_StallEnd>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt = (GTextField)GetChild("txt");
        }
    }
}