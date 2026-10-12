/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Component_OpClick : GComponent
    {
        public Controller ctrl_name;
        public const string URL = "ui://jnnt3xizgr0026";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Component_OpClick";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Component_OpClick));
            UIRegisterInfoManager.Instance.Register<UI_Component_OpClick>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_name = GetController("ctrl_name");
        }
    }
}