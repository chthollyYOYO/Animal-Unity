/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Bookstore : GComponent
    {
        public GButton btn_close;
        public GTextField txt_money;
        public Transition entryBookStore;
        public const string URL = "ui://zyfv00glievq6";
        public const string PKG_NAME = "Bookstore";
        public const string RES_NAME = "Panel_Bookstore";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Bookstore));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Bookstore>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_close = (GButton)GetChild("btn_close");
            txt_money = (GTextField)GetChild("txt_money");
            entryBookStore = GetTransition("entryBookStore");
        }
    }
}