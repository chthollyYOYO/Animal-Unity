/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_GiftBag : GComponent
    {
        public GList list_BagItems;
        public GButton btn_close;
        public const string URL = "ui://ebg9ug78e6f02";
        public const string PKG_NAME = "Gift";
        public const string RES_NAME = "Panel_GiftBag";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_GiftBag));
            UIRegisterInfoManager.Instance.Register<UI_Panel_GiftBag>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            list_BagItems = (GList)GetChild("list_BagItems");
            btn_close = (GButton)GetChild("btn_close");
        }
    }
}