/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Button_BoxItem : GButton
    {
        public Controller ctrl_hasCount;
        public GLoader lod_icon;
        public GTextField txt_count;
        public const string URL = "ui://jnnt3xizj4cbx3l";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Button_BoxItem";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Button_BoxItem));
            UIRegisterInfoManager.Instance.Register<UI_Button_BoxItem>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_hasCount = GetController("ctrl_hasCount");
            lod_icon = (GLoader)GetChild("lod_icon");
            txt_count = (GTextField)GetChild("txt_count");
        }
    }
}