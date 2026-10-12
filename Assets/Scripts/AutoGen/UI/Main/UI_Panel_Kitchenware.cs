/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Kitchenware : GComponent
    {
        public Controller ctrl_style;
        public GList list_item;
        public UI_Button_KitchenwareItem btn_item;
        public GButton btn_close;
        public UI_Component_ItemInfo com_itemInfo;
        public const string URL = "ui://jnnt3xizgr001m";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Panel_Kitchenware";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Kitchenware));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Kitchenware>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_style = GetController("ctrl_style");
            list_item = (GList)GetChild("list_item");
            btn_item = (UI_Button_KitchenwareItem)GetChild("btn_item");
            btn_close = (GButton)GetChild("btn_close");
            com_itemInfo = (UI_Component_ItemInfo)GetChild("com_itemInfo");
        }
    }
}