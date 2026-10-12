/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Button_Recipe : GButton
    {
        public Controller ctrl_affordable;
        public Controller ctrl_soldOut;
        public GTextField txt_Recipe;
        public GTextField txt_Price;
        public GLoader lod_item;
        public const string URL = "ui://zyfv00glsxl1e";
        public const string PKG_NAME = "Bookstore";
        public const string RES_NAME = "Button_Recipe";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Button_Recipe));
            UIRegisterInfoManager.Instance.Register<UI_Button_Recipe>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_affordable = GetController("ctrl_affordable");
            ctrl_soldOut = GetController("ctrl_soldOut");
            txt_Recipe = (GTextField)GetChild("txt_Recipe");
            txt_Price = (GTextField)GetChild("txt_Price");
            lod_item = (GLoader)GetChild("lod_item");
        }
    }
}