/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Thank : GComponent
    {
        public GButton btn_exit;
        public const string URL = "ui://jnnt3xizo1fkx4u";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Panel_Thank";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Thank));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Thank>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_exit = (GButton)GetChild("btn_exit");
        }
    }
}