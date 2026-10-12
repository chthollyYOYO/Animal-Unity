/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Component_Intent : GComponent
    {
        public Controller ctrl_style;
        public GLoader lod_icon;
        public const string URL = "ui://jnnt3xizhcuo2e";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Component_Intent";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Component_Intent));
            UIRegisterInfoManager.Instance.Register<UI_Component_Intent>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_style = GetController("ctrl_style");
            lod_icon = (GLoader)GetChild("lod_icon");
        }
    }
}