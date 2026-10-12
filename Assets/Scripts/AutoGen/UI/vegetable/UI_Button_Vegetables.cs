/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Button_Vegetables : GButton
    {
        public Controller ctrl_priceChange;
        public Controller ctrl_soldOut;
        public Controller ctrl_specialItem;
        public Controller ctrl_affordable;
        public GLoader lod_item;
        public GTextField txt_price;
        public GTextField txt_num;
        public GTextField txt_name;
        public const string URL = "ui://46wq0b21oitix3v";
        public const string PKG_NAME = "Vegetable";
        public const string RES_NAME = "Button_Vegetables";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Button_Vegetables));
            UIRegisterInfoManager.Instance.Register<UI_Button_Vegetables>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_priceChange = GetController("ctrl_priceChange");
            ctrl_soldOut = GetController("ctrl_soldOut");
            ctrl_specialItem = GetController("ctrl_specialItem");
            ctrl_affordable = GetController("ctrl_affordable");
            lod_item = (GLoader)GetChild("lod_item");
            txt_price = (GTextField)GetChild("txt_price");
            txt_num = (GTextField)GetChild("txt_num");
            txt_name = (GTextField)GetChild("txt_name");
        }
    }
}