/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Button_Option : GButton
    {
        public GTextField txt_option;
        public const string URL = "ui://g0c90zefq9tg2x";
        public const string PKG_NAME = "Dialogue";
        public const string RES_NAME = "Button_Option";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Button_Option));
            UIRegisterInfoManager.Instance.Register<UI_Button_Option>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_option = (GTextField)GetChild("txt_option");
        }
    }
}