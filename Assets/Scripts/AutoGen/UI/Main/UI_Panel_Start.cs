/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Start : GComponent
    {
        public GList list_menu;
        public const string URL = "ui://jnnt3xizgr001u";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Panel_Start";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Start));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Start>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            list_menu = (GList)GetChild("list_menu");
        }
    }
}