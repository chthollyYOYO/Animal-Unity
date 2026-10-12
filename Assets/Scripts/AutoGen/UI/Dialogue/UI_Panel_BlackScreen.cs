/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_BlackScreen : GComponent
    {
        public GGraph img_bg;
        public const string URL = "ui://g0c90zefhsf80";
        public const string PKG_NAME = "Dialogue";
        public const string RES_NAME = "Panel_BlackScreen";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_BlackScreen));
            UIRegisterInfoManager.Instance.Register<UI_Panel_BlackScreen>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            img_bg = (GGraph)GetChild("img_bg");
        }
    }
}