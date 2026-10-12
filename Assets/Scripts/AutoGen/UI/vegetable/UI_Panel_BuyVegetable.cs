/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_BuyVegetable : GComponent
    {
        public GImage shelf;
        public GList list_shelfItems;
        public GTextField txt_money;
        public GTextField txt_Resource;
        public GButton btn_close;
        public const string URL = "ui://46wq0b21l232x4o";
        public const string PKG_NAME = "Vegetable";
        public const string RES_NAME = "Panel_BuyVegetable";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_BuyVegetable));
            UIRegisterInfoManager.Instance.Register<UI_Panel_BuyVegetable>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            shelf = (GImage)GetChild("shelf");
            list_shelfItems = (GList)GetChild("list_shelfItems");
            txt_money = (GTextField)GetChild("txt_money");
            txt_Resource = (GTextField)GetChild("txt_Resource");
            btn_close = (GButton)GetChild("btn_close");
        }
    }
}