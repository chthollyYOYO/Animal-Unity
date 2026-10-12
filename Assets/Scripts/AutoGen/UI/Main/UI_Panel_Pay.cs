/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Pay : GComponent
    {
        public GLoader lod_item;
        public GTextInput intxt_itemNum;
        public GButton btn_add;
        public GButton btn_minus;
        public GButton btn_confirm;
        public GButton btn_cancel;
        public GButton btn_add10;
        public GButton btn_minus10;
        public const string URL = "ui://jnnt3xiztx5sx3w";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Panel_Pay";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Pay));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Pay>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lod_item = (GLoader)GetChild("lod_item");
            intxt_itemNum = (GTextInput)GetChild("intxt_itemNum");
            btn_add = (GButton)GetChild("btn_add");
            btn_minus = (GButton)GetChild("btn_minus");
            btn_confirm = (GButton)GetChild("btn_confirm");
            btn_cancel = (GButton)GetChild("btn_cancel");
            btn_add10 = (GButton)GetChild("btn_add10");
            btn_minus10 = (GButton)GetChild("btn_minus10");
        }
    }
}