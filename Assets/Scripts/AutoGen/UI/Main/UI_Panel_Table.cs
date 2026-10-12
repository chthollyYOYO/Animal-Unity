/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Table : GComponent
    {
        public UI_Component_ItemInfo com_itemInfo;
        public GLoader lod_icon;
        public UI_Button_BoxItem btn_boxItem;
        public GButton btn_close;
        public const string URL = "ui://jnnt3xizvu0s2r";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Panel_Table";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Table));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Table>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            com_itemInfo = (UI_Component_ItemInfo)GetChild("com_itemInfo");
            lod_icon = (GLoader)GetChild("lod_icon");
            btn_boxItem = (UI_Button_BoxItem)GetChild("btn_boxItem");
            btn_close = (GButton)GetChild("btn_close");
        }
    }
}