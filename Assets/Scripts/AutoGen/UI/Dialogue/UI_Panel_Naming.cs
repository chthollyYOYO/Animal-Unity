/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Naming : GComponent
    {
        public GTextInput input_name;
        public GButton btn_confirm;
        public const string URL = "ui://g0c90zefoqil34";
        public const string PKG_NAME = "Dialogue";
        public const string RES_NAME = "Panel_Naming";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Naming));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Naming>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            input_name = (GTextInput)GetChild("input_name");
            btn_confirm = (GButton)GetChild("btn_confirm");
        }
    }
}