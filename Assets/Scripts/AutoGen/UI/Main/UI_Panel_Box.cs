/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Box : GComponent
    {
        public Controller ctrl_style;
        public UI_Component_ItemInfo com_itemInfo;
        public GList list_boxItem;
        public GButton btn_close;
        public const string URL = "ui://jnnt3xizvu0s2q";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Panel_Box";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Box));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Box>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_style = GetController("ctrl_style");
            com_itemInfo = (UI_Component_ItemInfo)GetChild("com_itemInfo");
            list_boxItem = (GList)GetChild("list_boxItem");
            btn_close = (GButton)GetChild("btn_close");
        }
    }
}