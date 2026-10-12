/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Component_ItemInfo : GComponent
    {
        public GLoader lod_icon;
        public GTextField txt_name;
        public GTextField txt_desc;
        public const string URL = "ui://jnnt3xizhcuo2b";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Component_ItemInfo";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Component_ItemInfo));
            UIRegisterInfoManager.Instance.Register<UI_Component_ItemInfo>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lod_icon = (GLoader)GetChild("lod_icon");
            txt_name = (GTextField)GetChild("txt_name");
            txt_desc = (GTextField)GetChild("txt_desc");
        }
    }
}