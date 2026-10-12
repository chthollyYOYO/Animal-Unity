/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Button_KitchenwareItem : GButton
    {
        public GLoader lod_bg;
        public GLoader lod_icon;
        public const string URL = "ui://jnnt3xizhcuo2k";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Button_KitchenwareItem";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Button_KitchenwareItem));
            UIRegisterInfoManager.Instance.Register<UI_Button_KitchenwareItem>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lod_bg = (GLoader)GetChild("lod_bg");
            lod_icon = (GLoader)GetChild("lod_icon");
        }
    }
}