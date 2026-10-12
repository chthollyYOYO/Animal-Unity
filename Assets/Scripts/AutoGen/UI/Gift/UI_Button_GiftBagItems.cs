/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Button_GiftBagItems : GButton
    {
        public GLoader lod_item;
        public const string URL = "ui://ebg9ug78e6f03";
        public const string PKG_NAME = "Gift";
        public const string RES_NAME = "Button_GiftBagItems";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Button_GiftBagItems));
            UIRegisterInfoManager.Instance.Register<UI_Button_GiftBagItems>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lod_item = (GLoader)GetChild("lod_item");
        }
    }
}