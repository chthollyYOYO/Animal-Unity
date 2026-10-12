/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Component_CookCompleted : GComponent
    {
        public GLoader lod_icon;
        public const string URL = "ui://jnnt3xizhcuo2j";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Component_CookCompleted";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Component_CookCompleted));
            UIRegisterInfoManager.Instance.Register<UI_Component_CookCompleted>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lod_icon = (GLoader)GetChild("lod_icon");
        }
    }
}